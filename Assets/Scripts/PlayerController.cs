﻿using System;
using UnityEngine;

public class PlayerController : UnitController
{
    private enum AttackMode
    {
        Melee = 0,
        Range = 1,
        Magic = 2
    }

    private const float kTimeScale = 1;

    #region Combat Properties

    private const float kAttackCooldown = 0.25f;

    private int mMana;
    private int mStamina;
    private AttackMode mAttackMode = AttackMode.Melee;

    public Single Mana { get { return (Single)mMana / kMaxMana; } }

    public Single Stamina { get { return (Single)mStamina / kMaxStamina; } }

    public int AttackModeIndex { get { return (int)mAttackMode; } }

    private PerceptionController[] mPerceptions;
    private new const int kMaxHealth = 10;
    private const int kMaxMana = 10;
    private const int kMaxStamina = 10;
    private const int kMeleeDamage = 1;
    private const int kRangedDamage = 2;
    private const int kMagicDamage = 5;
    private const int kRangedStamina = 3;
    private const int kMagicMana = 4;

    private new const float kMeleeCooldown = 0.5f;
    private new const float kRangeCooldown = 3.0f;
    private new const float kMagicCooldown = 2.0f;
    // Per step regen

    public bool isPaused = false;

    public bool isRunning { get { return !isPaused; } }

    #region Step based regen

    private int mHealSteps;
    private int mManaSteps;
    private int mStamSteps;

    private void HealStep()
    {
        mHealSteps++;
        if (mHealSteps >= kHealReqSteps)
        {
            mHealSteps = 0;
            mHealth = Math.Min(kMaxHealth, mHealth + kHealStepRegen);
        }
    }

    private void ManaStep()
    {
        mManaSteps++;
        if (mManaSteps >= kManaReqSteps)
        {
            mManaSteps = 0;
            mMana = Math.Min(kMaxMana, mMana + kManaStepRegen);
        }
    }

    private void StamStep()
    {
        mStamSteps++;
        if (mStamSteps >= kStamReqSteps)
        {
            mStamSteps = 0;
            mStamina = Math.Min(kMaxStamina, mStamina + kStamStepRegen);
        }
    }

    private const int kHealReqSteps = 5; // 1hp per 5steps
    private const int kHealStepRegen = 1;

    private const int kManaStepRegen = 1; // 1 mana per 2 steps
    private const int kManaReqSteps = 2;

    private const int kStamStepRegen = 1; // 1 stam per 3 steps
    private const int kStamReqSteps = 3;

    #endregion Step based regen

    #endregion Combat Properties

    #region Rendering Properties

    public Sprite[] PlayerSprites;

    public Sprite ActivePlayerSprite { get { return PlayerSprites[(int)mAttackMode]; } }

    private bool[] mInroRecovered = new bool[3] { false, false, false };
    private const int kNumInro = 3;
    private const Single UI_MissingInroAlpha = 0.2f;

    public Single Inro0_UI_Color { get { return mInroRecovered != null && (mInroRecovered[0]) ? 1 : UI_MissingInroAlpha; } }

    public Single Inro1_UI_Color { get { return mInroRecovered != null && (mInroRecovered[1]) ? 1 : UI_MissingInroAlpha; } }

    public Single Inro2_UI_Color { get { return mInroRecovered != null && (mInroRecovered[2]) ? 1 : UI_MissingInroAlpha; } }

    public Single Pause_UI_Alpha { get { return (isPaused) ? 1 : 0; } }

    #endregion Rendering Properties

    // Use this for initialization
    new void Start()
    {
        base.Start();
        mTeam = UnitTeam.Player;
        mHealth = kMaxHealth;
        mMana = kMaxMana;
        mStamina = kMaxStamina;
        mPerceptions = new PerceptionController[4];
        mPerceptions[NORTH] = GO_Combat[NORTH].GetComponent<PerceptionController>();
        mPerceptions[SOUTH] = GO_Combat[SOUTH].GetComponent<PerceptionController>();
        mPerceptions[EAST] = GO_Combat[EAST].GetComponent<PerceptionController>();
        mPerceptions[WEST] = GO_Combat[WEST].GetComponent<PerceptionController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case UnitState.Idle:
                DoMovement();
                DoAttackModeInput();
                DoAttack();
                break;
            case UnitState.Dieing:
                // Do death
                break;
        }
    }

    #region StateUpdate methods

    #region Special Attack Methods

    private void AttackMelee(Vector2 attackDir)
    {
        if (attackDir != Vector2.zero && mLastMeleeAttack + kMeleeCooldown <= Time.time)
        {
            GameObject target = null;

            if (attackDir.x == 1)
            {
                target = mPerceptions[EAST].CurrentTarget;
            }
            if (attackDir.x == -1)
            {
                target = mPerceptions[WEST].CurrentTarget;
            }
            if (attackDir.y == 1)
            {
                target = mPerceptions[NORTH].CurrentTarget;
            }
            if (attackDir.y == -1)
            {
                target = mPerceptions[SOUTH].CurrentTarget;
            }

            if (target != null)
            {
                // Do the attack
                Debug.DrawLine(transform.position, target.transform.position, Color.red, 0.5f);
                UnitController uc = target.GetComponent<UnitController>();
                if (uc != null)
                {
                    uc.TakeDamage(kMeleeDamage, EffectsController.EffectType.EffectMelee);
                    mLastMeleeAttack = Time.time;
                }
            }
        }
    }

    private void AttackRanged(Vector2 attackDir)
    {
        if (attackDir != Vector2.zero &&
            mStamina >= kRangedStamina &&
            mLastRangedAttack + kRangeCooldown <= Time.time)
        {
            GameObject arrow = FireArrow(attackDir, kRangedDamage);
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), arrow.GetComponent<BoxCollider2D>());
            mStamina -= kRangedStamina;
        }
    }

    private void AttackMagic(Vector2 attackDir)
    {
        if (attackDir != Vector2.zero &&
            mMana >= kMagicMana &&
            mLastMagicAttack + kMagicCooldown <= Time.time)
        {
            FireMagic(attackDir, kMagicDamage);
            mMana -= kMagicMana;
        }
    }

    #endregion Special Attack Methods

    #region State-based methods

    private void DoAttackModeInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mAttackMode = AttackMode.Melee;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mAttackMode = AttackMode.Range;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mAttackMode = AttackMode.Magic;
        }
    }

    private void DoMovement()
    {
        if (!mMoving)
        {
            if (Input.GetKey(KeyCode.A))
            {
                MoveTo(-1, 0);
                return;
            }
            if (Input.GetKey(KeyCode.D))
            {
                MoveTo(1, 0);
                return;
            }
            if (Input.GetKey(KeyCode.W))
            {
                MoveTo(0, 1);
                return;
            }
            if (Input.GetKey(KeyCode.S))
            {
                MoveTo(0, -1);
                return;
            }
        }
    }

    private void DoAttack()
    {
        Vector2 attackDir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            attackDir.Set(0, 1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            attackDir.Set(0, -1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            attackDir.Set(1, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            attackDir.Set(-1, 0);
        }
        if (attackDir == Vector2.zero)
        {
            return;
        }
        switch (mAttackMode)
        {
            case AttackMode.Melee:
                AttackMelee(attackDir);
                break;
            case AttackMode.Range:
                AttackRanged(attackDir);
                break;
            case AttackMode.Magic:
                AttackMagic(attackDir);
                break;
        }
    }

    #endregion State-based methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Inro"))
        {
            string inroName = other.gameObject.name;
            int inro_Id = int.Parse(inroName.Substring(inroName.IndexOf("_") + 1));
            mInroRecovered[inro_Id] = true;
            GameObject.Destroy(other.gameObject);
        }
        if (other.tag.Equals("Portal"))
        {
            other.GetComponent<EffectsController>().EmitEffect(EffectsController.EffectType.Portal);
        }
    }

    protected override void RefreshStats()
    {
        HealStep();
        ManaStep();
        StamStep();
        mLastMeleeAttack = float.MinValue;
    }

    #endregion StateUpdate methods

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0 : kTimeScale;
    }
}
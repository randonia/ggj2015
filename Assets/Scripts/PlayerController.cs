using System;
using System.Collections;
using UnityEngine;

public class PlayerController : UnitController
{
    private enum AttackMode
    {
        Melee = 0,
        Range = 1,
        Magic = 2
    }

    #region Consts

    private new const int kMaxHealth = 10;
    private const int kMaxMana = 10;
    private const int kMaxStamina = 10;

    #endregion Consts

    #region Combat Properties

    private new const float kAttackCooldown = 0.25f;

    private int mMana;
    private int mStamina;
    private AttackMode mAttackMode = AttackMode.Melee;

    public Single Mana { get { return (Single)mMana / kMaxMana; } }

    public Single Stamina { get { return (Single)mStamina / kMaxStamina; } }

    public int AttackModeIndex { get { return (int)mAttackMode; } }

    private PerceptionController[] mPerceptions;

    #endregion Combat Properties

    // Use this for initialization
    private void Start()
    {
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
    private void Update()
    {
        DoMovement();
        DoAttackModeInput();
        DoAttack();
    }

    #region StateUpdate methods

    private void DoAttack()
    {
        GameObject target = null;
        if (mLastAttack + kAttackCooldown < Time.time)
        {
            Vector2 attackDir = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                target = mPerceptions[NORTH].CurrentTarget;
                attackDir.Set(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                target = mPerceptions[SOUTH].CurrentTarget;
                attackDir.Set(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                target = mPerceptions[EAST].CurrentTarget;
                attackDir.Set(1, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                target = mPerceptions[WEST].CurrentTarget;
                attackDir.Set(-1, 0);
            }
            if (target != null)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.red);
            }
        }
    }

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

    #endregion StateUpdate methods
}
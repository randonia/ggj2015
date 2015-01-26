using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    #region Directions

    /// <summary>
    /// Make sure this is sync'd with WorldController
    /// </summary>

    public const int NORTH = 0;
    public const int SOUTH = 1;
    public const int EAST = 2;
    public const int WEST = 3;

    #endregion Directions

    public enum UnitTeam
    {
        Neutral,
        Player,
        Enemy
    }

    public enum UnitState
    {
        Idle,
        Moving,
        Attacking,
        Dieing,
        Dead,
        Menu,
        Winning
    }

    public UnitState State { get { return mState; } set { mState = value; } }

    public GameObject GO_EffectSystem;
    public GameObject GO_Perception;
    public GameObject PREFAB_Arrow;
    public GameObject PREFAB_Magic;
    public GameObject[] GO_Combat;

    public float mMoveTweenDuration = 0.25f;
    protected bool mMoving = false;

    protected UnitState mState = UnitState.Idle;
    public UnitTeam mTeam = UnitTeam.Neutral;

    public UnitTeam Team { get { return mTeam; } }

    private EffectsController mEffectController;

    protected const float kMeleeCooldown = 1.0f;
    protected const float kRangeCooldown = 3.0f;
    protected const float kMagicCooldown = 2.0f;

    protected float mLastMeleeAttack = float.MinValue;
    protected float mLastRangedAttack = float.MinValue;
    protected float mLastMagicAttack = float.MinValue;

    protected const int kMaxHealth = 10;
    protected int mHealth = kMaxHealth;

    private Dictionary<string, AudioSource> mAudioClips;

    public Single Health
    {
        get { return (Single)mHealth / kMaxHealth; }
    }

    // Use this for initialization
    protected void Start()
    {
        mHealth = kMaxHealth;
        if (GO_EffectSystem != null)
        {
            mEffectController = GO_EffectSystem.GetComponent<EffectsController>();
        }
        mAudioClips = new Dictionary<string, AudioSource>();
        foreach (AudioSource clip in GetComponents<AudioSource>())
        {
            mAudioClips.Add(clip.clip.name, clip);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected virtual void DoDeath()
    {
        GameObject.Destroy(gameObject);
    }

    protected bool MoveTo(int xDir, int yDir)
    {
        if (CanMove(xDir, yDir))
        {
            mMoving = true;
            iTween.MoveTo(gameObject, iTween.Hash(
                "x", transform.position.x + xDir,
                "y", transform.position.y + yDir,
                "time", mMoveTweenDuration,
                "oncomplete", "ClearMoveFlag",
                "easetype", "linear")
                );
            return true;
        }
        return false;
    }

    private bool CanMove(int xDir, int yDir)
    {
        if (mMoving)
        {
            return false;
        }
        int cardinal = ((xDir == 1) ? EAST : (xDir == -1) ? WEST : (yDir == 1) ? NORTH : (yDir == -1) ? SOUTH : 0);
        return WorldController.CanMove((int)transform.position.x, (int)transform.position.y, cardinal);
    }

    protected void MoveTo(Vector2 dir)
    {
        if (!mMoving)
        {
            MoveTo((int)dir.x, (int)dir.y);
        }
    }

    protected GameObject FireArrow(Vector2 dir, int damage)
    {
        GameObject arrow = (GameObject)GameObject.Instantiate(PREFAB_Arrow,
         gameObject.transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileController>().Direction = dir;
        arrow.GetComponent<ProjectileController>().Damage = damage;
        arrow.GetComponent<ProjectileController>().Team = mTeam;
        mLastRangedAttack = Time.time;
        if (mAudioClips != null)
        {
            mAudioClips["bow_fire"].Play();
        }
        return arrow;
    }

    protected GameObject FireMagic(Vector2 dir, int damage)
    {
        GameObject magicMissile = (GameObject)GameObject.Instantiate(PREFAB_Magic,
         gameObject.transform.position, Quaternion.identity);
        magicMissile.GetComponent<ProjectileController>().Direction = dir;
        magicMissile.GetComponent<ProjectileController>().Damage = damage;
        magicMissile.GetComponent<ProjectileController>().Team = mTeam;
        mLastMagicAttack = Time.time;
        if (mAudioClips != null)
        {
            mAudioClips["magic_fire"].Play();
        }
        return magicMissile;
    }

    public void TakeDamage(int damage, EffectsController.EffectType effectType)
    {
        if (mEffectController != null)
        {
            mEffectController.EmitEffect(effectType);
        }
        if (mAudioClips != null)
        {
            switch (effectType)
            {
                case EffectsController.EffectType.EffectMelee:
                    mAudioClips["katana_slash"].Play();
                    break;
                case EffectsController.EffectType.EffectRanged:
                    mAudioClips["bow_hit"].Play();
                    break;
            }
        }
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        mHealth -= damage;
        EnemyController ec = null;
        if (ec = GetComponent<EnemyController>())
        {
            ec.SetAggroOnPlayer();
        }
        if (mHealth <= 0)
        {
            // Die
            mState = UnitState.Dieing;
        }
    }

    private void ClearMoveFlag()
    {
        mMoving = false;
        RefreshStats();
    }

    protected virtual void RefreshStats()
    {
    }
}
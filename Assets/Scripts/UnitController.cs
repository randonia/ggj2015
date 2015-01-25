using System;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public const int NORTH = 0;
    public const int SOUTH = 1;
    public const int EAST = 2;
    public const int WEST = 3;

    public enum UnitTeam
    {
        Neutral,
        Player,
        Enemy
    }

    protected enum UnitState
    {
        Idle,
        Moving,
        Attacking,
        Dieing,
        Dead
    }

    public GameObject GO_Perception;
    public GameObject PREFAB_Arrow;
    public GameObject[] GO_Combat;

    public float mMoveTweenDuration = 0.25f;
    protected bool mMoving = false;

    protected UnitState mState = UnitState.Idle;
    protected UnitTeam mTeam = UnitTeam.Neutral;

    public UnitTeam Team { get { return mTeam; } }

    protected const float kMeleeCooldown = 1.0f;
    protected const float kRangeCooldown = 3.0f;
    protected const float kMagicCooldown = 2.0f;

    protected float mLastMeleeAttack = float.MinValue;
    protected float mLastRangedAttack = float.MinValue;
    protected float mLastMagicAttack = float.MinValue;

    protected const int kMaxHealth = 10;
    protected int mHealth = kMaxHealth;

    public Single Health
    {
        get { return (Single)mHealth / kMaxHealth; }
    }

    // Use this for initialization
    private void Start()
    {
        mHealth = kMaxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if (mHealth <= 0)
        {
            DoDeath();
        }
    }

    private void DoDeath()
    {
    }

    protected void MoveTo(int xDir, int yDir)
    {
        if (!mMoving)
        {
            mMoving = true;
            iTween.MoveTo(gameObject, iTween.Hash(
                "x", transform.position.x + xDir,
                "y", transform.position.y + yDir,
                "time", mMoveTweenDuration,
                "oncomplete", "ClearMoveFlag",
                "easetype", "linear")
                );
        }
    }

    protected void MoveTo(Vector2 dir)
    {
        if (!mMoving)
        {
            MoveTo((int)dir.x, (int)dir.y);
        }
    }

    protected void FireArrow(Vector2 dir, int damage)
    {
        GameObject arrow = (GameObject)GameObject.Instantiate(PREFAB_Arrow,
         gameObject.transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileController>().Direction = dir;
        arrow.GetComponent<ProjectileController>().Damage = damage;
        mLastRangedAttack = Time.time;
    }

    public void TakeDamage(int damage)
    {
        mHealth -= damage;
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
using System;
using System.Collections;
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
    public GameObject PREFAB_Magic;
    public GameObject[] GO_Combat;

    public float mMoveTweenDuration = 0.25f;
    protected bool mMoving = false;

    protected UnitState mState = UnitState.Idle;
    public UnitTeam mTeam = UnitTeam.Neutral;

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

    protected void FireArrow(Vector2 dir, int damage)
    {
        GameObject arrow = (GameObject)GameObject.Instantiate(PREFAB_Arrow,
         gameObject.transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileController>().Direction = dir;
        arrow.GetComponent<ProjectileController>().Damage = damage;
        arrow.GetComponent<ProjectileController>().Team = mTeam;
        mLastRangedAttack = Time.time;
    }

    protected void FireMagic(Vector2 dir, int damage)
    {
        GameObject magicMissile = (GameObject)GameObject.Instantiate(PREFAB_Magic,
         gameObject.transform.position, Quaternion.identity);
        magicMissile.GetComponent<ProjectileController>().Direction = dir;
        magicMissile.GetComponent<ProjectileController>().Damage = damage;
        magicMissile.GetComponent<ProjectileController>().Team = mTeam;
        mLastMagicAttack = Time.time;
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
using System.Collections;
using UnityEngine;

public class EnemyController : UnitController
{
    public int mAttackDamage;

    protected GameObject mTarget;

    /// <summary>
    /// How long the enemy tracks the target until it gives up
    /// </summary>
    protected const float kFollowTimer = 2.0f;

    /// <summary>
    /// How far an enemy has to be to keep moving
    /// </summary>
    public float kMovePerceptionThreshold = 1.5f;

    // Use this for initialization
    protected void Start()
    {
        mTeam = UnitTeam.Enemy;
        PerceptionController pc = GO_Perception.GetComponent<PerceptionController>();
        pc.mPerceptionEnterCallback = PerceptionEnter;
        pc.mPerceptionExitCallback = PerceptionExit;
    }

    // Update is called once per frame
    protected void Update()
    {
        switch (mState)
        {
            case UnitState.Attacking:
                DoAttack();
                break;
            case UnitState.Dead:
                break;
            case UnitState.Dieing:
                DoDeath();
                break;
            case UnitState.Idle:
                DoIdle();
                break;
            case UnitState.Moving:
                break;
        }
    }

    #region StateActions

    private void DoIdle()
    {
    }

    protected virtual void DoAttack()
    {
        Vector2 dist = mTarget.transform.position - transform.position;
        if (dist.sqrMagnitude < kMovePerceptionThreshold)
        {
            AttackTarget(mTarget);
        }
        else
        {
            float absX = Mathf.Abs(dist.x);
            float absY = Mathf.Abs(dist.y);
            int x;
            int y;
            if (absX == absY)
            {
                if (Random.Range(-1, 1) < 0)
                {
                    x = 1;
                    y = 0;
                }
                else
                {
                    x = 0;
                    y = 1 * ((dist.y < 0) ? -1 : 1);
                }
            }
            else if (absX > absY)
            {
                x = 1;
                y = 0;
            }
            else
            {
                x = 0;
                y = 1;
            }
            Vector2 moveDir = new Vector2(x * ((dist.x < 0) ? -1 : 1),
                                          y * ((dist.y < 0) ? -1 : 1));
            MoveTo(moveDir);
        }
    }

    #endregion StateActions

    protected virtual void AttackTarget(GameObject target)
    {
        if (mLastMeleeAttack + kMeleeCooldown < Time.time)
        {
            PerceptionController pc = target.GetComponent<PerceptionController>();
            if (pc != null)
            {
                UnitController uc = pc.Parent.GetComponent<UnitController>();
                if (uc != null)
                {
                    uc.TakeDamage(mAttackDamage);
                }
            }
            mLastMeleeAttack = Time.time;
        }
    }

    private void PerceptionEnter(Collider2D other)
    {
        if (mState.Equals(UnitState.Idle) || mState.Equals(UnitState.Attacking) ||
            mState.Equals(UnitState.Moving))
            if (other.tag.Equals("Player"))
            {
                CancelInvoke("ClearFollowing");
                mTarget = other.gameObject;
                mState = UnitState.Attacking;
            }
    }

    private void PerceptionExit(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Invoke("ClearFollowing", kFollowTimer);
        }
    }

    private void ClearFollowing()
    {
        mTarget = null;
        if (mState.Equals(UnitState.Idle) || mState.Equals(UnitState.Attacking) ||
            mState.Equals(UnitState.Moving))
        {
            mState = UnitState.Idle;
        }
    }
}
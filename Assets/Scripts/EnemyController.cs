﻿using System.Collections;
using UnityEngine;

public class EnemyController : UnitController
{
    public GameObject GO_Perception;
    public int mAttackDamage;

    private GameObject mTarget;

    /// <summary>
    /// How long the enemy tracks the target until it gives up
    /// </summary>
    private const float kFollowTimer = 2.0f;

    private const float kAttackCooldown = 1.5f;
    private float mLastAttack;

    // Use this for initialization
    private void Start()
    {
        mHealth = kMaxHealth;
        PerceptionController pc = GO_Perception.GetComponent<PerceptionController>();
        pc.mPerceptionEnterCallback = PerceptionEnter;
        pc.mPerceptionExitCallback = PerceptionExit;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (mState)
        {
            case UnitState.Attacking:
                DoAttack();
                break;

            case UnitState.Dead:
                break;

            case UnitState.Dieing:
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

    private void DoAttack()
    {
        Vector2 dist = mTarget.transform.position - transform.position;
        if (dist.sqrMagnitude < 2.3f)
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

    private void AttackTarget(GameObject target)
    {
        if (mLastAttack + kAttackCooldown < Time.time)
        {
            Debug.Log("Do attack");
            mLastAttack = Time.time;
        }
    }

    private void PerceptionEnter(Collider2D other)
    {
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
        mState = UnitState.Idle;
    }
}
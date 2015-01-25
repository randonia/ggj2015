using System.Collections;
using UnityEngine;

public class EnemyBowmanController : EnemyController
{
    protected override void AttackTarget(GameObject target)
    {
        if (mLastRangedAttack + kRangeCooldown < Time.time)
        {
            PerceptionController pc = target.GetComponent<PerceptionController>();
            if (pc != null)
            {
                UnitController uc = pc.Parent.GetComponent<UnitController>();
                if (uc != null)
                {
                    GameObject arrow = FireArrow((target.transform.position - transform.position).normalized,
                        mAttackDamage);
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), arrow.GetComponent<BoxCollider2D>());
                }
            }
        }
    }
}
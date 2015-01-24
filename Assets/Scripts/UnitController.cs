using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    protected enum UnitState
    {
        Idle,
        Moving,
        Attacking,
        Dieing,
        Dead
    }

    public float mMoveTweenDuration = 0.25f;
    protected bool mMoving = false;

    protected UnitState mState = UnitState.Idle;

    protected const int kMaxHealth = 10;
    protected int mHealth = kMaxHealth;

    public int Health
    {
        get { return mHealth; }
    }

    // Use this for initialization
    private void Start()
    {
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

    private void ClearMoveFlag()
    {
        mMoving = false;
    }
}
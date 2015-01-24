using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

    protected enum UnitState
    {
        Idle,
        Moving,
        Attacking,
        Dieing,
        Dead
    }

    protected float mTweenDuration = 0.25f;
    protected bool mMoving = false;

    protected UnitState mState = UnitState.Idle;

    protected const int kMaxHealth = 10;
    protected int mHealth = kMaxHealth;

	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
	void Update () {
        if (mHealth <= 0)
        {
            DoDeath();
        }
	}

    void DoDeath()
    {

    }

    protected void MoveTo(int xDir, int yDir)
    {
        mMoving = true;
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", transform.position.x + xDir,
            "y", transform.position.y + yDir,
            "time", mTweenDuration,
            "oncomplete", "ClearMoveFlag",
            "easetype", "linear")
            );
    }

    private void ClearMoveFlag()
    {
        mMoving = false;
    }
}

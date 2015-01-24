using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const float kTweenDuration = 0.25f;
    private bool mMoving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        doMovement();
	}

    void doMovement()
    {
        if (!mMoving){
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveTo(-1, 0);
                return;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveTo(1, 0);
                return;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveTo(0, 1);
                return;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveTo(0, -1);
                return;
            }
        }
    }

    void MoveTo(int xDir, int yDir)
    {
        mMoving = true;
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", transform.position.x + xDir,
            "y", transform.position.y + yDir,
            "time", kTweenDuration,
            "oncomplete", "ClearMoveFlag",
            "easetype", "linear")
            );
    }

    void ClearMoveFlag()
    {
        mMoving = false;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerController : UnitController {

	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
        doMovement();
	}

    void doMovement()
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
}

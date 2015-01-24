using System.Collections;
using UnityEngine;

public class PlayerController : UnitController
{
    #region Consts

    private new const int kMaxHealth = 10;
    private const int kMaxMana = 10;
    private const int kMaxStamina = 10;

    #endregion Consts

    #region Mana and Stamina properties

    private int mMana;
    private int mStamina;

    public int Mana { get { return mMana; } }

    public int Stamina { get { return mStamina; } }

    #endregion Mana and Stamina properties

    // Use this for initialization
    private void Start()
    {
        mHealth = kMaxHealth;
        mMana = kMaxMana;
        mStamina = kMaxStamina;
    }

    // Update is called once per frame
    private void Update()
    {
        doMovement();
    }

    private void doMovement()
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
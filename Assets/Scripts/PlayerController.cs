using System;
using System.Collections;
using UnityEngine;

public class PlayerController : UnitController
{
    private enum AttackMode
    {
        Melee = 0,
        Range = 1,
        Magic = 2
    }

    public GameObject GO_UI_AttackMode;

    #region Consts

    private new const int kMaxHealth = 10;
    private const int kMaxMana = 10;
    private const int kMaxStamina = 10;

    #endregion Consts

    #region Combat Properties

    private int mMana;
    private int mStamina;
    private AttackMode mAttackMode = AttackMode.Melee;

    public Single Mana { get { return (Single)mMana / kMaxMana; } }

    public Single Stamina { get { return (Single)mStamina / kMaxStamina; } }

    public int AttackModeIndex { get { return (int)mAttackMode; } }

    #endregion Combat Properties

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
        GetAttackModeInput();
        DoMovement();
    }

    private void GetAttackModeInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mAttackMode = AttackMode.Melee;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mAttackMode = AttackMode.Range;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mAttackMode = AttackMode.Magic;
        }
    }

    private void DoMovement()
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
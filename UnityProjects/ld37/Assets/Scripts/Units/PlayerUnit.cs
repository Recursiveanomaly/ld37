using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public Vector3 m_cameraOffset;

    private float m_holdKeyTimer = 0;
    const float c_holdKeyMax = 0.1f;
    const float c_firstPressMax = 0.35f;

    private void Update()
    {
        bool playerMoved = false;
        bool firstPress = false;

        if (m_holdKeyTimer > 0)
        {
            m_holdKeyTimer -= Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Keypad8) ||
                Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Keypad2) ||
                Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.Keypad4) ||
                Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.Keypad6))
            {
                m_holdKeyTimer = 0;
            }
        }

        if (m_holdKeyTimer <= 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8))
            {
                Room.Instance.MoveUnit(this, Grid.eDirection.kUp);
                playerMoved = true;
                if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
                {
                    firstPress = true;
                }
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad2))
            {
                Room.Instance.MoveUnit(this, Grid.eDirection.kDown);
                playerMoved = true;
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    firstPress = true;
                }
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))
            {
                Room.Instance.MoveUnit(this, Grid.eDirection.kLeft);
                playerMoved = true;
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    firstPress = true;
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))
            {
                Room.Instance.MoveUnit(this, Grid.eDirection.kRight);
                playerMoved = true;
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
                {
                    firstPress = true;
                }
            }
        }

        if(playerMoved)
        {
            Room.Instance.OnPlayerMoved();
            m_holdKeyTimer = c_holdKeyMax;
            if(firstPress == true)
            {
                m_holdKeyTimer = c_firstPressMax;
            }
        }
    }

    public override void KillUnit()
    {
        // game over man, game over
        GameMaster.Instance.OnGameOver();
        base.KillUnit();
    }

    public override void AwardKill()
    {
        base.AwardKill();

        LevelUp();
    }

    public void LevelUp()
    {
        m_currentLevel++;
        Room.Instance.OnPlayerLeveledUp();
    }

    public override void OnUnitWasMoved()
    {
        base.OnUnitWasMoved();

        //if (Camera.main != null)
        //{
        //    Camera.main.transform.position = transform.position + m_cameraOffset;
        //}
    }

    protected override bool IsGoodGuy()
    {
        return true;
    }
}

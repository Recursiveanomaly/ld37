using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public Vector3 m_cameraOffset;

    private float m_moveCooldownTimer = 0;
    const float c_moveCooldownMax = 0.3f;

    Grid.eDirection? m_queuedMove = null;

    bool m_dead = false;

    private void Update()
    {
        if (m_dead || Room.Instance.ControlsLocked) return;

        m_moveCooldownTimer -= Time.deltaTime;

        if (m_moveCooldownTimer < c_moveCooldownMax / 3f)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8))
            {
                m_queuedMove = Grid.eDirection.kUp;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad2))
            {
                m_queuedMove = Grid.eDirection.kDown;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))
            {
                m_queuedMove = Grid.eDirection.kLeft;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))
            {
                m_queuedMove = Grid.eDirection.kRight;
            }
        }

        if (m_queuedMove.HasValue && m_moveCooldownTimer <= 0)
        {
            Room.Instance.MoveUnit(this, m_queuedMove.Value);
            Room.Instance.OnPlayerMoved();
            m_queuedMove = null;
            m_moveCooldownTimer = c_moveCooldownMax;
        }
    }

    public override void KillUnit()
    {
        // game over man, game over
        GameMaster.Instance.OnGameOver();
        m_dead = true;
        base.KillUnit();
    }

    public override void AwardKill()
    {
        base.AwardKill();

        LevelUp();
    }

    public override void ResetForNewGame()
    {
        m_dead = false;
        base.ResetForNewGame();
    }

    public void LevelUp()
    {
        m_currentLevel++;
        Room.Instance.OnPlayerLeveledUp();
    }

    public override void OnUnitWasMoved(bool warp, float delay)
    {
        base.OnUnitWasMoved(warp, delay);

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

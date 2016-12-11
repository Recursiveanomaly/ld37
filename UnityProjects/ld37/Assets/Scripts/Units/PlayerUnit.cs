using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public Vector3 m_cameraOffset;

    private void Update()
    {
        bool playerMoved = false;

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Room.Instance.OnPlayerMoved();
            Room.Instance.MoveUnit(this, Grid.eDirection.kUp);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Room.Instance.OnPlayerMoved();
            Room.Instance.MoveUnit(this, Grid.eDirection.kDown);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Room.Instance.OnPlayerMoved();
            Room.Instance.MoveUnit(this, Grid.eDirection.kLeft);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Room.Instance.OnPlayerMoved();
            Room.Instance.MoveUnit(this, Grid.eDirection.kRight);
            playerMoved = true;
        }

        if(playerMoved)
        {
            Room.Instance.OnPlayerMoved();
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

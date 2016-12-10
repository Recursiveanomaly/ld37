using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{

    private void Update()
    {
        bool playerMoved = false;

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Room.Instance.MoveUnit(this, Grid.eDirection.kUp);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Room.Instance.MoveUnit(this, Grid.eDirection.kDown);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Room.Instance.MoveUnit(this, Grid.eDirection.kLeft);
            playerMoved = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Room.Instance.MoveUnit(this, Grid.eDirection.kRight);
            playerMoved = true;
        }

        if(playerMoved)
        {
            Room.Instance.OnPlayerMoved();
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public int m_currentLevel;
    public Grid.Coordinate m_coordinate;
    Grid.Coordinate m_startingCoordinate;

    protected void Awake()
    {
        ResetForNewGame();
    }

    protected void OnDestroy()
    {
        if(Room.Instance != null && Room.Instance.m_grid != null)
        {
            Room.Instance.m_grid.RemoveUnit(this);
        }
    }

    public virtual void ResetForNewGame()
    {
        m_currentLevel = 0;
        transform.localPosition = Grid.GetPositionFromCoordinate(new Grid.Coordinate(15, 8));
        m_coordinate = m_startingCoordinate = Grid.GetCoordinateFromPosition(transform.localPosition);
    }

    protected virtual bool IsGoodGuy()
    {
        return false;
    }

    public virtual void KillUnit()
    {

    }

    public virtual void AwardKill()
    {

    }

    public bool IsEnemyOf(UnitBase occupied)
    {
        return IsGoodGuy() != occupied.IsGoodGuy();
    }

    public virtual void OnCollision(UnitBase other)
    {
    }

    public virtual void OnUnitWasMoved()
    {
        // set new position
        transform.localPosition = Grid.GetPositionFromCoordinate(m_coordinate);
    }

    public virtual void OnPlayerMoved()
    {

    }

    public virtual bool CanShareSpace(UnitBase unit)
    {
        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public int m_currentLevel;
    public Grid.Coordinate m_coordinate;

    protected void Awake()
    {
        m_coordinate = Grid.GetCoordinateFromPosition(transform.localPosition);
    }

    public virtual void ResetForNewLevel()
    {
        m_currentLevel = 0;
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
}

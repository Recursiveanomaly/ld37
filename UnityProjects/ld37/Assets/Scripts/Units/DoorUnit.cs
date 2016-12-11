using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnit : UnitBase
{
    public DoorUnit m_linkedDoor;
    public Color m_brownOne;
    public Color m_brownTwo;
    public Color m_brownThree;
    public Color m_brownFour;
    public SpriteRenderer m_spriteRenderer;
    public int growthPhaseSpawnedIn;

    public override bool OnCollision(UnitBase other)
    {
        if(m_linkedDoor != null)
        {
            // find an open spot next to the other door
            Grid.Coordinate adjacentCoordinate = m_linkedDoor.GetOpenAdjacentSpot();
            if(adjacentCoordinate != null)
            {
                Room.Instance.m_grid.TrySetUnitCoordinate(other, adjacentCoordinate);
            }
        }
        else
        {
            // move on to the next growth phase
            Room.Instance.Grow();
            Room.Instance.DestroyObstacle(this);
        }

        return base.OnCollision(other);
    }

    public Grid.Coordinate GetOpenAdjacentSpot()
    {
        Grid.Coordinate adjacentCoordinate = null;

        adjacentCoordinate = CheckCoordinate(1, 0);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(0, 1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(-1, 0);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(0, -1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(-1, 1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(1, -1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(1, 1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        adjacentCoordinate = CheckCoordinate(-1, -1);
        if (adjacentCoordinate != null) return adjacentCoordinate;

        return null;
    }

    private Grid.Coordinate CheckCoordinate(int x, int y)
    {
        Grid.Coordinate potentialCoordinate = new Grid.Coordinate(m_coordinate);
        potentialCoordinate.x += x;
        potentialCoordinate.y += y;

        if(Room.Instance.m_grid.GetUnitAtCoordinate(potentialCoordinate) == null && Room.Instance.m_grid.IsCoordinateWithinGrid(potentialCoordinate))
        {
            return potentialCoordinate;
        }
        return null;
    }

    public void SetColor(int growthPhase)
    {
        if (growthPhase == 0)
        {
            m_spriteRenderer.color = m_brownOne;
        }
        else if (growthPhase == 1)
        {
            m_spriteRenderer.color = m_brownTwo;
        }
        else if (growthPhase == 2)
        {
            m_spriteRenderer.color = m_brownThree;
        }
        else
        {
            m_spriteRenderer.color = m_brownFour;
        }
    }

    //public override bool CanShareSpace(UnitBase unit)
    //{
    //    return unit is PlayerUnit;
    //}
}

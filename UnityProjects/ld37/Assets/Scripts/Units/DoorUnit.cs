using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnit : UnitBase
{
    public DoorUnit m_linkedDoor;

    public override void OnCollision(UnitBase other)
    {
        base.OnCollision(other);

        if(m_linkedDoor != null)
        {
            // find an open spot next to the other door
            Grid.Coordinate adjacentCoordinate = m_linkedDoor.GetOpenAdjacentSpot();
            if(adjacentCoordinate != null)
            {
                Room.Instance.m_grid.TrySetUnitCoordinate(other, adjacentCoordinate);
            }
        }
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
}

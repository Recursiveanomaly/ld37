using System;
using System.Collections.Generic;
using UnityEngine;

public class MapLogic
{
    private SettlersEngine.SpatialAStar<Grid.Coordinate, UnitBase> m_aStarPathfinder;

    const int c_width = 32;
    const int c_height = 32;

    Grid.Coordinate[,] m_pathfindingGrid = null;

    public MapLogic()
    {
        if(m_pathfindingGrid == null)
        {
            m_pathfindingGrid = new Grid.Coordinate[c_width, c_height];
            for (int widthIndex = 0; widthIndex < c_width; widthIndex++)
            {
                for (int heightIndex = 0; heightIndex < c_height; heightIndex++)
                {
                    m_pathfindingGrid[widthIndex, heightIndex] = new Grid.Coordinate(widthIndex, heightIndex);
                }
            }
        }

        m_aStarPathfinder = new SettlersEngine.SpatialAStar<Grid.Coordinate, UnitBase>(m_pathfindingGrid);
    }

    public LinkedList<Grid.Coordinate> FindPath(Grid.Coordinate start, Grid.Coordinate end, UnitBase entity)
    {
        if (!Room.Instance.m_grid.IsCoordinateWithinGrid(start) || !Room.Instance.m_grid.IsCoordinateWithinGrid(end)) return new LinkedList<Grid.Coordinate>();
        return m_aStarPathfinder.Search(start, end, entity);
    }
}

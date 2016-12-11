using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDefinition
{
    public class EnemyDefinition
    {
        public int m_level;
        public Grid.Coordinate m_startingCoordinate;
        public Sprite m_sprite;
    }

    public enum eObstacleType
    {
        kDoor,
    }

    public Grid.Coordinate m_playerStart;
    public List<EnemyDefinition> m_enemies = new List<EnemyDefinition>();
    public List<Grid.Coordinate> m_mapValidCoordinates = new List<Grid.Coordinate>();
    public List<KeyValuePair<Grid.Coordinate, eObstacleType>> m_obstacles = new List<KeyValuePair<Grid.Coordinate, eObstacleType>>();
}

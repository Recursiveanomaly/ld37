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

    public Grid.Coordinate m_playerStart;
    public List<EnemyDefinition> m_enemies = new List<EnemyDefinition>();
    public List<Grid.Coordinate> m_mapValidCoordinates = new List<Grid.Coordinate>();
    public List<Grid.Coordinate> m_doors = new List<Grid.Coordinate>();
}

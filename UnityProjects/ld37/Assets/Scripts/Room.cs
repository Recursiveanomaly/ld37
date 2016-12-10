﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : SingletonMonoBehaviour<Room>
{
    public PlayerUnit m_player;
    public List<EnemyUnit> m_enemies = new List<EnemyUnit>();

    public void OnPlayerMoved()
    {
        foreach(EnemyUnit enemyUnit in m_enemies)
        {
            MoveUnit(enemyUnit, (Grid.eDirection)Random.Range(0, 4));
        }
    }

    public void MoveUnit(UnitBase unit, Grid.eDirection direction)
    {
        // attack any enemy in that slot

        // attempt to move
        switch (direction)
        {
            case Grid.eDirection.kUp:
                unit.m_coordinate.y += 1;
                break;
            case Grid.eDirection.kDown:
                unit.m_coordinate.y -= 1;
                break;
            case Grid.eDirection.kLeft:
                unit.m_coordinate.x -= 1;
                break;
            case Grid.eDirection.kRight:
                unit.m_coordinate.x += 1;
                break;
            default:
                break;
        }

        // set new position
        unit.transform.localPosition = Grid.GetPositionFromCoordinate(unit.m_coordinate);
    }

    Grid m_grid;
}

public class Grid
{
    public enum eDirection
    {
        kUp,
        kDown,
        kLeft,
        kRight,
    }

    public class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public static Vector3 GetPositionFromCoordinate(Coordinate coordinate)
    {
        return new Vector3(coordinate.x, coordinate.y, 0);
    }

    public static Coordinate GetCoordinateFromPosition(Vector3 vector)
    {
        return new Coordinate((int)vector.x, (int)vector.y);
    }
}

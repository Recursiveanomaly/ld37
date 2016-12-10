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
}

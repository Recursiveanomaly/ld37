using DG.Tweening;
using SettlersEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : SingletonMonoBehaviour<Room>
{
    public Grid m_grid = new Grid();
    public MapLogic m_mapLogic = new MapLogic();
    int m_currentPhase;

    PlayerUnit m_player;
    List<EnemyUnit> m_enemies = new List<EnemyUnit>();
    List<GameObject> m_floorTiles = new List<GameObject>();
    Dictionary<Grid.Coordinate, GameObject> m_walls = new Dictionary<Grid.Coordinate, GameObject>();
    List<UnitBase> m_obstacles = new List<UnitBase>();

    public PlayerUnit m_playerPrefab;
    public EnemyUnit m_enemyPrefab;
    public GameObject m_wallPrefab;
    public GameObject m_floorPrefab;

    bool m_skipEnemyTurn = false;

    public void Grow()
    {
        m_currentPhase++;
        if (m_currentPhase >= 4)
        {
            GameOverWindow.Instance.Show();
        }
        else
        {
            if(m_currentPhase != 0)
            {
                m_skipEnemyTurn = true;
            }
            StartCoroutine(AdditiveLoadLevel(LevelGenerator.GenerateLevel(m_currentPhase)));
        }
    }

    // obstacle prefabs
    public DoorUnit m_doorPrefab;

    public List<Sprite> m_enemySprites = new List<Sprite>();

    public bool ControlsLocked { get; private set; }

    public void ResetRoom()
    {
        UnloadCurrentLevel();
        ControlsLocked = false;
    }

    private void UnloadCurrentLevel()
    {
        m_grid = new Grid();
        // clean up floor tiles
        foreach (GameObject floorTile in m_floorTiles)
        {
            GameObject.Destroy(floorTile.gameObject);
        }
        m_floorTiles.Clear();

        // clean up walls
        foreach (GameObject wallObject in m_walls.Values)
        {
            GameObject.Destroy(wallObject.gameObject);
        }
        m_walls.Clear();

        // clean up obstacles
        foreach (UnitBase obstacle in m_obstacles)
        {
            GameObject.Destroy(obstacle.gameObject);
        }
        m_obstacles.Clear();

        // clean up enemies
        foreach (EnemyUnit enemy in m_enemies)
        {
            GameObject.Destroy(enemy.gameObject);
        }
        m_enemies.Clear();

        if(m_player != null)
        {
            m_player.ResetForNewGame();
        }
        m_currentPhase = -1;
    }

    public void OnPlayerLeveledUp()
    {
        foreach (EnemyUnit enemy in m_enemies)
        {
            enemy.OnPlayerLevelChanged(m_player);
        }
    }

    const float c_tweenTime = 0.15f;
    const float c_stepTime = 0.005f;
    const float c_tweenDistance = 1f;

    public IEnumerator AdditiveLoadLevel(LevelDefinition levelDefiniton)
    {
        ControlsLocked = true;

        // clean up walls
        foreach (GameObject wallObject in m_walls.Values)
        {
            SpriteRenderer spriteRenderer = wallObject.GetComponentInChildren<SpriteRenderer>();
            if(spriteRenderer != null)
            {
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, new Color(1, 1, 1, 0), c_tweenTime).SetOptions(true);
            }
            wallObject.transform.DOMoveY(wallObject.transform.position.y + c_tweenDistance, c_tweenTime).OnComplete(() =>
            {
                GameObject.Destroy(wallObject.gameObject);
            });
            yield return new WaitForSeconds(c_stepTime);
        }
        m_walls.Clear();

        //// clean up dead enemies
        //for (int i = m_enemies.Count - 1; i >= 0; i--)
        //{
        //    if (m_enemies[i].m_isDead)
        //    {
        //        GameObject.Destroy(m_enemies[i].gameObject);
        //        m_enemies.RemoveAt(i);
        //    }
        //}

        // add the walkable areas
        foreach (Grid.Coordinate coordinate in levelDefiniton.m_mapValidCoordinates)
        {
            if(!m_grid.IsCoordinateWithinGrid(coordinate))
            {
                GameObject floorTile = GameObject.Instantiate(m_floorPrefab, transform);
                floorTile.transform.localPosition = Grid.GetPositionFromCoordinate(coordinate) + new Vector3(0, -c_tweenDistance, 0);
                m_floorTiles.Add(floorTile);

                SpriteRenderer spriteRenderer = floorTile.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = new Color(1, 1, 1, 0);
                    DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, new Color(1, 1, 1, 1), c_tweenTime).SetOptions(true);
                }
                floorTile.transform.DOMoveY(floorTile.transform.position.y + c_tweenDistance, c_tweenTime);
            }
            yield return new WaitForSeconds(c_stepTime);
        }
        m_grid.SetGridPositions(levelDefiniton.m_mapValidCoordinates);

        // add the walls
        foreach (Grid.Coordinate coordinate in m_grid.m_gridValidArea)
        {
            //yield return StartCoroutine(TryAddWall(coordinate, 1, 0));
            //yield return StartCoroutine(TryAddWall(coordinate, -1, 0));
            //yield return StartCoroutine(TryAddWall(coordinate, 0, 1));
            //yield return StartCoroutine(TryAddWall(coordinate, 0, -1));

            //yield return StartCoroutine(TryAddWall(coordinate, 1, 1));
            //yield return StartCoroutine(TryAddWall(coordinate, -1, 1));
            //yield return StartCoroutine(TryAddWall(coordinate, 1, -1));
            //yield return StartCoroutine(TryAddWall(coordinate, -1, -1));
            if (TryAddWall(coordinate, 1, 0)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, -1, 0)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, 0, 1)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, 0, -1)) yield return new WaitForSeconds(c_stepTime);

            if (TryAddWall(coordinate, 1, 1)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, -1, 1)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, 1, -1)) yield return new WaitForSeconds(c_stepTime);
            if (TryAddWall(coordinate, -1, -1)) yield return new WaitForSeconds(c_stepTime);
        }

        // add the obstacles
        List<DoorUnit> doors = new List<DoorUnit>();
        foreach (KeyValuePair<Grid.Coordinate, LevelDefinition.eObstacleType> obstacleDefinition in levelDefiniton.m_obstacles)
        {
            UnitBase obstacle = GameObject.Instantiate(GetObstaclePrefab(obstacleDefinition.Value), transform);
            if(obstacle is DoorUnit)
            {
                doors.Add(obstacle as DoorUnit);
            }
            m_obstacles.Add(obstacle);
            m_grid.TrySetUnitCoordinate(obstacle, obstacleDefinition.Key, true);
            obstacle.transform.localPosition = Grid.GetPositionFromCoordinate(obstacleDefinition.Key) + new Vector3(0, c_tweenDistance, 0);

            SpriteRenderer spriteRenderer = obstacle.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0);
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, new Color(1, 1, 1, 1), c_tweenTime).SetOptions(true);
            }
            obstacle.transform.DOMoveY(obstacle.transform.position.y - c_tweenDistance, c_tweenTime);
            yield return new WaitForSeconds(c_stepTime);
        }

        // link up random doors
        while(doors.Count > 0)
        {
            if(doors.Count == 1)
            {
                // this is the complete level door, leave the linked door null
                DoorUnit finalDoor = doors[0];
                finalDoor.SetColor(levelDefiniton.m_difficulty);
                doors.RemoveAt(0);
            }
            else
            {
                // pair up some doors
                int randomDoor1 = UnityEngine.Random.Range(0, doors.Count - 1);
                DoorUnit door1 = doors[randomDoor1];
                doors.RemoveAt(randomDoor1);

                int randomDoor2 = UnityEngine.Random.Range(0, doors.Count - 1);
                DoorUnit door2 = doors[randomDoor2];
                doors.RemoveAt(randomDoor2);

                door1.m_linkedDoor = door2;
                door2.m_linkedDoor = door1;
                door1.SetColor(levelDefiniton.m_difficulty);
                door2.SetColor(levelDefiniton.m_difficulty);
            }
        }

        // add the player
        if (m_player == null)
        {
            m_player = GameObject.Instantiate(m_playerPrefab, transform);
            m_player.ResetForNewGame();
            m_grid.TrySetUnitCoordinate(m_player, new Grid.Coordinate(16, 16), true);
        }

        // add the enemies
        foreach (LevelDefinition.EnemyDefinition enemyDefinition in levelDefiniton.m_enemies)
        {
            EnemyUnit enemyUnit = GameObject.Instantiate(m_enemyPrefab, transform);
            enemyUnit.m_spriteRenderer.sprite = enemyDefinition.m_sprite;
            enemyUnit.m_currentLevel = enemyDefinition.m_level;
            Grid.Coordinate startingCoordinate = enemyDefinition.m_startingCoordinate;
            if(startingCoordinate == null)
            {
                // find a random position
                float avoidDistance = 2;
                if(m_player.m_currentLevel - enemyUnit.m_currentLevel < 0)
                {
                    // spawn stronger enemies farther away
                    avoidDistance += Mathf.Min(3, enemyUnit.m_currentLevel - m_player.m_currentLevel);
                }
                startingCoordinate = m_grid.GetRandomOpenPosition(m_player.m_coordinate, avoidDistance);
            }

            if (startingCoordinate == null)
            {
                Debug.LogError("Error in Room.LoadLevel - no open spot found to place monster");
                GameObject.Destroy(enemyUnit);
            }
            else
            {
                m_enemies.Add(enemyUnit);
                m_grid.TrySetUnitCoordinate(enemyUnit, startingCoordinate, true);
            }
        }

        // this will trigger enemy color change
        OnPlayerLeveledUp();
        yield return new WaitForSeconds(c_tweenTime);

        ControlsLocked = false;
    }

    public Grid.Coordinate GetPlayerCoordinate()
    {
        if(m_player != null)
        {
            return m_player.m_coordinate;
        }
        return new Grid.Coordinate(16,16);
    }

    private UnitBase GetObstaclePrefab(LevelDefinition.eObstacleType obstacleType)
    {
        switch (obstacleType)
        {
            case LevelDefinition.eObstacleType.kDoor:
                return m_doorPrefab;
            default:
                Debug.LogError("Error in Room.GetObstaclePrefab - no prefab found for " + obstacleType);
                break;
        }
        return m_doorPrefab;
    }

    public void DestroyObstacle(UnitBase obstacle)
    {
        m_obstacles.Remove(obstacle);
        GameObject.Destroy(obstacle.gameObject);
    }

    internal void DestroyEnemy(EnemyUnit enemyUnit)
    {
        m_enemies.Remove(enemyUnit);
        GameObject.Destroy(enemyUnit.gameObject);
    }

    private bool TryAddWall(Grid.Coordinate baseCoordinate, int xOffset, int yOffset)
    {
        Grid.Coordinate offsetCoordinate = new Grid.Coordinate(baseCoordinate);
        offsetCoordinate.x += xOffset;
        offsetCoordinate.y += yOffset;

        if(!m_walls.ContainsKey(offsetCoordinate) && !m_grid.IsCoordinateWithinGrid(offsetCoordinate))
        {
            GameObject wallObject = GameObject.Instantiate(m_wallPrefab, transform);
            wallObject.transform.localPosition = Grid.GetPositionFromCoordinate(offsetCoordinate) + new Vector3(0, -c_tweenDistance, 0);
            m_walls.Add(offsetCoordinate, wallObject);

            SpriteRenderer spriteRenderer = wallObject.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0);
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, new Color(1, 1, 1, 1), c_tweenTime).SetOptions(true);
            }
            wallObject.transform.DOMoveY(wallObject.transform.position.y + c_tweenDistance, c_tweenTime);

            return true;
        }
        return false;
    }

    public void OnPlayerMoved()
    {
        if(m_skipEnemyTurn)
        {
            m_skipEnemyTurn = false;
            return;
        }

        List<UnitBase> obstacles = new List<UnitBase>();
        obstacles.AddRange(m_obstacles);
        foreach (UnitBase obstacle in m_obstacles)
        {
            obstacle.OnPlayerMoved();
        }

        List<EnemyUnit> enemies = new List<EnemyUnit>();
        enemies.AddRange(m_enemies);
        foreach (EnemyUnit enemyUnit in enemies)
        {
            enemyUnit.OnPlayerMoved();
        }
    }

    public void MoveUnit(UnitBase unit, Grid.eDirection direction)
    {
        Grid.Coordinate potentialCoordinate = new Grid.Coordinate(unit.m_coordinate);
        switch (direction)
        {
            case Grid.eDirection.kUp:
                potentialCoordinate.y += 1;
                break;
            case Grid.eDirection.kDown:
                potentialCoordinate.y -= 1;
                break;
            case Grid.eDirection.kLeft:
                potentialCoordinate.x -= 1;
                break;
            case Grid.eDirection.kRight:
                potentialCoordinate.x += 1;
                break;
            default:
                break;
        }

        MoveUnit(unit, potentialCoordinate, false);
    }

    public void MoveUnit(UnitBase unit, Grid.Coordinate coordinate, bool warp)
    {
        UnitBase occupied = m_grid.GetUnitAtCoordinate(coordinate);
        // attack any enemy in that slot
        bool collisionStoppedMove = false;
        if (occupied != null)
        {
            collisionStoppedMove |= occupied.OnCollision(unit);
            collisionStoppedMove |= unit.OnCollision(occupied);
        }

        // attempt to move
        if(!collisionStoppedMove)
        {
            m_grid.TrySetUnitCoordinate(unit, coordinate, warp);
        }
    }
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

    public class Coordinate : IEquatable<Coordinate>, IPathNode<UnitBase>
    {
        public int x;
        public int y;
        private Coordinate m_coordinate;

        public Coordinate(Coordinate m_coordinate)
        {
            x = m_coordinate.x;
            y = m_coordinate.y;
        }

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public bool Equals(Coordinate other)
        {
            return other != null && x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if(obj is Coordinate)
            {
                Coordinate other = obj as Coordinate;
                return x == other.x && y == other.y;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (x.GetHashCode() * 10) + y.GetHashCode();
        }

        public bool IsWalkable(UnitBase inContext)
        {
            UnitBase occupied = Room.Instance.m_grid.GetUnitAtCoordinate(this);
            if(occupied != null && !occupied.IsEnemyOf(inContext))
            {
                return false;
            }
            return Room.Instance.m_grid.IsCoordinateWithinGrid(this);
        }

        public float Distance(Coordinate other)
        {
            if(other == null)
            {
                return 0;
            }
            return Vector2.Distance(new Vector2(x, y), new Vector2(other.x, other.y));
        }
    }

    public HashSet<Coordinate> m_gridValidArea = new HashSet<Coordinate>();
    Dictionary<Coordinate, UnitBase> m_unitPositions = new Dictionary<Coordinate, UnitBase>();

    public void SetGridPositions(List<Coordinate> validCoordinates)
    {
        foreach(Coordinate coordinate in validCoordinates)
        {
            if(!m_gridValidArea.Contains(coordinate))
            {
                m_gridValidArea.Add(coordinate);
            }
        }
    }

    public bool TrySetUnitCoordinate(UnitBase unit, Coordinate coordinate, bool warp)
    {
        if (!m_gridValidArea.Contains(coordinate))
        {
            return false;
        }
        if (m_unitPositions.ContainsKey(coordinate))
        {
            UnitBase otherUnit = m_unitPositions[coordinate];
            if(!otherUnit.CanShareSpace(unit))
            {
                return false;
            }
        }
        m_unitPositions.Remove(unit.m_coordinate);
        m_unitPositions.Add(coordinate, unit);
        unit.m_coordinate = coordinate;
        // set new position
        unit.OnUnitWasMoved(warp, 0f);
        return true;
    }

    public void RemoveUnit(UnitBase unit)
    {
        if(unit != null && unit.m_coordinate != null && GetUnitAtCoordinate(unit.m_coordinate) == unit)
        {
            m_unitPositions.Remove(unit.m_coordinate);
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

    public UnitBase GetUnitAtCoordinate(Coordinate potentialCoordinate)
    {
        UnitBase unit = null;
        m_unitPositions.TryGetValue(potentialCoordinate, out unit);
        return unit;
    }

    public bool IsCoordinateWithinGrid(Coordinate potentialCoordinate)
    {
        return m_gridValidArea.Contains(potentialCoordinate);
    }

    public Coordinate GetRandomOpenPosition(Coordinate avoidCoordinate, float avoidDistance)
    {
        List<Coordinate> potentialSpots = new List<Coordinate>();
        potentialSpots.AddRange(m_gridValidArea);

        while(potentialSpots.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, potentialSpots.Count - 1);

            Coordinate potentialCoordinate = potentialSpots[randomIndex];
            potentialSpots.RemoveAt(randomIndex);

            if(avoidCoordinate != null && avoidDistance > 0 && potentialCoordinate.Distance(avoidCoordinate) <= avoidDistance)
            {
                continue;
            }

            if(GetUnitAtCoordinate(potentialCoordinate) == null)
            {
                return potentialCoordinate;
            }
        }

        if(avoidCoordinate != null)
        {
            // try again closer
            if(avoidDistance - 1f > 0f)
            {
                return GetRandomOpenPosition(avoidCoordinate, avoidDistance - 1f);
            }
            else
            {
                // try anything
                return GetRandomOpenPosition(null, 0);
            }
        }
        return null;
    }
}

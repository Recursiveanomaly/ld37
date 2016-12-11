using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public static LevelDefinition GenerateLevel(int difficulty)
    {
        LevelDefinition newLevel = ParseMap(GrowthController.Instance.GetRandomMap(difficulty), difficulty);
        newLevel.m_difficulty = difficulty;
        return newLevel;
    }

    private static Sprite GetRandomEnemy()
    {
        return Room.Instance.m_enemySprites[Random.Range(0, Room.Instance.m_enemySprites.Count - 1)];
    }

    private static LevelDefinition ParseMap(string map, int difficulty)
    {
        LevelDefinition level = new LevelDefinition();

        List<Grid.Coordinate> potentialDoors = new List<Grid.Coordinate>();

        map = map.Replace("-1", "X");
        map = map.Replace("\t", "");
        map = map.Replace(" ", "");
        map = map.Replace(",", "");
        map = map.Replace("\r", "");
        int x = 0;
        int y = -32;
        while(map.Length > 0)
        {
            string front = map.Substring(0, 1);
            map = map.Substring(1);
            switch (front)
            {
                case "0":
                    // Empty Floor
                    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    break;
                case "\n":
                    // Newline
                    x = -1;
                    y++;
                    break;
                case "1":
                    // Door
                    potentialDoors.Add(new Grid.Coordinate(x, -y));
                    break;



                    //case "_":
                    //    // Empty Floor
                    //    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    //    break;
                    //case "\n":
                    //    // Newline
                    //    x = -1;
                    //    y++;
                    //    break;
                    //case "D":
                    //    // Door
                    //    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    //    level.m_obstacles.Add(new KeyValuePair<Grid.Coordinate, LevelDefinition.eObstacleType>(new Grid.Coordinate(x, -y), LevelDefinition.eObstacleType.kDoor));
                    //    break;
                    //case "0":
                    //    // Player
                    //    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    //    level.m_playerStart = new Grid.Coordinate(x, -y);
                    //    break;
                    //case "1":
                    //case "2":
                    //case "3":
                    //case "4":
                    //case "5":
                    //case "6":
                    //case "7":
                    //case "8":
                    //case "9":
                    //    // Monster
                    //    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    //    LevelDefinition.EnemyDefinition enemy = new LevelDefinition.EnemyDefinition();
                    //    enemy.m_startingCoordinate = new Grid.Coordinate(x, -y);
                    //    enemy.m_sprite = GetRandomEnemy();
                    //    enemy.m_level = int.Parse(front);
                    //    level.m_enemies.Add(enemy);
                    //    break;
                    //default:
                    //    //x--;
                    //    break;
            }
            x++;
        }

        // add random doors
        int doorsToAdd = GrowthController.Instance.GetDoorCount(difficulty);
        for(int doorCounter = 0; doorCounter < doorsToAdd && potentialDoors.Count > 0; doorCounter++)
        {
            int potentialIndex = Random.Range(0, potentialDoors.Count - 1);
            Grid.Coordinate doorCoordinate = potentialDoors[potentialIndex];
            potentialDoors.RemoveAt(potentialIndex);
            level.m_mapValidCoordinates.Add(new Grid.Coordinate(doorCoordinate));
            level.m_obstacles.Add(new KeyValuePair<Grid.Coordinate, LevelDefinition.eObstacleType>(doorCoordinate, LevelDefinition.eObstacleType.kDoor));
        }

        // add random monsters
        List<int> monstersToAdd = GrowthController.Instance.GetMonsters(difficulty);
        while(monstersToAdd.Count > 0)
        {
            int monsterLevel = monstersToAdd[0];
            monstersToAdd.RemoveAt(0);

            LevelDefinition.EnemyDefinition enemy = new LevelDefinition.EnemyDefinition();
            enemy.m_startingCoordinate = null;
            enemy.m_sprite = GetRandomEnemy();
            enemy.m_level = monsterLevel;
            level.m_enemies.Add(enemy);
        }

        return level;
    }

    const string c_levelMap =
      @"XXXXXXXXXXXXXXXXXXXXXXXXXX
X________________________X
X________________________X
X________________________X
X________________________X
X________________________X
X____2______1____________X
X________________________X
X________________________X
X________________________X
X____3______0____________X
X________________________X
X________________________X
X________________________X
X________________________X
X________________________X
X_____D_________D________X
XXXXXXXXXXXXXXXXXXXXXXXXXX";
}

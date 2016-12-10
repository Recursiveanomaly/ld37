using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public static LevelDefinition GenerateLevel(int difficulty)
    {
        LevelDefinition newLevel = ParseMap(c_levelMap);
        return newLevel;
    }

    private static Sprite GetRandomEnemy()
    {
        return Room.Instance.m_enemySprites[Random.Range(0, Room.Instance.m_enemySprites.Count - 1)];
    }

    private static LevelDefinition ParseMap(string map)
    {
        LevelDefinition level = new LevelDefinition();
        
        map = map.Replace("\t", "");
        map = map.Replace(" ", "");
        int x = 0;
        int y = 0;
        while(map.Length > 0)
        {
            string front = map.Substring(0, 1);
            map = map.Substring(1);
            switch (front)
            {
                case "_":
                    // Empty Floor
                    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    break;
                case "\n":
                    // Newline
                    x = -1;
                    y++;
                    break;
                case "D":
                    // Door
                    level.m_doors.Add(new Grid.Coordinate(x, -y));
                    break;
                case "0":
                    // Player
                    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    level.m_playerStart = new Grid.Coordinate(x, -y);
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    // Monster
                    level.m_mapValidCoordinates.Add(new Grid.Coordinate(x, -y));
                    LevelDefinition.EnemyDefinition enemy = new LevelDefinition.EnemyDefinition();
                    enemy.m_startingCoordinate = new Grid.Coordinate(x, -y);
                    enemy.m_sprite = GetRandomEnemy();
                    enemy.m_level = int.Parse(front);
                    level.m_enemies.Add(enemy);
                    break;
                default:
                    //x--;
                    break;
            }
            x++;
        }

        return level;
    }

    const string c_levelMap = 
      @"XXDXXXXXXX
        X___XXX3_X
        X1_______D
        X________X
        XDXX_0___X
        XXXX___XXX
        XXXX_2_XXX
        XXXXXXDXXX";
}

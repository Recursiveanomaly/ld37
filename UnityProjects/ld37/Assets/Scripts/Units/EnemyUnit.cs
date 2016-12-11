using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public SpriteRenderer m_spriteRenderer;
    public Sprite m_graveSprite;

    public Color m_green;
    public Color m_yellow;
    public Color m_orange;
    public Color m_red;
    public Color m_deathColor;

    public bool m_isDead = false;

    //float m_animationTimer = 0;
    //float c_animationMaxTime = 1f;

    void Update()
    {
        //m_animationTimer -= Time.deltaTime;
        //if(m_animationTimer <= 0)
        //{
        //    m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
        //    m_animationTimer = c_animationMaxTime;
        //}
    }

    public void OnPlayerLevelChanged(PlayerUnit player)
    {
        if(m_isDead)
        {
            return;
        }

        int difference = player.m_currentLevel - m_currentLevel;
        if(difference > 0)
        {
            m_spriteRenderer.color = m_green;
        }
        else if(difference == 0)
        {
            m_spriteRenderer.color = m_yellow;
        }
        else if(difference == -1)
        {
            m_spriteRenderer.color = m_orange;
        }
        else
        {
            m_spriteRenderer.color = m_red;
        }
    }

    public override void OnUnitWasMoved()
    {
        base.OnUnitWasMoved();
        if (m_isDead)
        {
            return;
        }

        m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
    }

    public override void KillUnit()
    {
        base.KillUnit();
        if (m_isDead)
        {
            return;
        }

        m_spriteRenderer.sprite = m_graveSprite;
        m_spriteRenderer.color = m_deathColor;
        m_isDead = true;
    }

    public override void OnCollision(UnitBase other)
    {
        base.OnCollision(other);

        if (m_isDead)
        {
            return;
        }

        if (IsEnemyOf(other))
        {
            // someone is going to die
            int difference = other.m_currentLevel - m_currentLevel;
            if (difference >= 0)
            {
                other.AwardKill();
                KillUnit();
            }
            else
            {
                other.KillUnit();
            }
        }
    }

    public override void OnPlayerMoved()
    {
        base.OnPlayerMoved();

        if (!m_isDead)
        {
            // try to move towards the player
            LinkedList<Grid.Coordinate> path = Room.Instance.m_mapLogic.FindPath(new Grid.Coordinate(m_coordinate), new Grid.Coordinate(Room.Instance.GetPlayerCoordinate()), this);
            if(path != null && path.Count > 1 && path.First != null && path.First.Next != null)
            {
                Grid.Coordinate firstStep = path.First.Next.Value;
                if(firstStep != null)
                {
                    Room.Instance.MoveUnit(this, firstStep);
                }
            }
            else
            {
                Debug.LogError("Error in EnemyUnit.OnPlayerMoved - no path found to player.");
            }


            //Room.Instance.MoveUnit(this, (Grid.eDirection)UnityEngine.Random.Range(0, 4));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public SpriteRenderer m_spriteRenderer;

    public Color m_green;
    public Color m_yellow;
    public Color m_orange;
    public Color m_red;

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
        m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
        base.OnUnitWasMoved();
    }
}

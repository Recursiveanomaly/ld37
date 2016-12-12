using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : SingletonMonoBehaviour<GameMaster>
{

    override protected void Awake()
    {
        DOTween.Init();
        m_triggerNewGame = true;
    }

    bool m_triggerNewGame = false;
    public void StartNewGame()
    {
        Room.Instance.ResetRoom();
        Room.Instance.Grow();
    }

    private void Update()
    {
        if(m_triggerNewGame)
        {
            m_triggerNewGame = false;
            StartNewGame();
        }        
    }

    public void OnGameOver()
    {
        m_triggerNewGame = true;
    }
}

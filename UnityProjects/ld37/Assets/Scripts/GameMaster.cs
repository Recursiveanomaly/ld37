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
        m_triggerNewGame = true;
    }

    private void Update()
    {
        if(m_triggerNewGame)
        {
            m_triggerNewGame = false;
            Room.Instance.ResetRoom();
            Room.Instance.Grow();
        }        
    }

    public void OnGameOver()
    {
        GameOverWindow.Instance.Show();
    }
}

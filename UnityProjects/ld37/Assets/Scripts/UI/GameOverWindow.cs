using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : SingletonMonoBehaviour<GameOverWindow>
{
    public UnityEngine.UI.Text m_label;
    public Color m_gameOver;
    public Color m_victory;

    public void Show(bool victory)
    {
        if (victory)
        {
            m_label.text = "You Escaped!";
            m_label.color = m_victory;
        }
        else
        {
            m_label.text = "Game Over";
            m_label.color = m_gameOver;
        }
        gameObject.SetActive(true);
    }

    public void OnRestartPressed()
    {
        gameObject.SetActive(false);
        GameMaster.Instance.StartNewGame();
    }
}

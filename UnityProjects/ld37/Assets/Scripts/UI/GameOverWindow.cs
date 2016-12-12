using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : SingletonMonoBehaviour<GameOverWindow>
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnRestartPressed()
    {
        gameObject.SetActive(false);
        GameMaster.Instance.StartNewGame();
    }
}

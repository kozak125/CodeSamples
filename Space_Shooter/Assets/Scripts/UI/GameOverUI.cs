using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverText;

    private void Awake()
    {
        EventBroker.OnGameOver += GameOver;
    }

    void GameOver()
    {
        gameOverText.SetActive(true);
        EventBroker.OnGameOver -= GameOver;
    }
}

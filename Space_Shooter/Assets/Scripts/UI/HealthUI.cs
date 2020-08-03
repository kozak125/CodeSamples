using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for displaing remaining player's lives
/// </summary>

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    Text livesText;

    [SerializeField]
    FloatVariable livesAmount;

    private void Start()
    {
        livesText.text = livesAmount.Value.ToString();

        EventBroker.OnPlayerDestroyed += UpdateLives;
        EventBroker.OnGameOver += GameOver;
    }

    void UpdateLives()
    {
        livesText.text = livesAmount.Value.ToString();
    }

    void GameOver()
    {
        EventBroker.OnPlayerDestroyed -= UpdateLives;
        EventBroker.OnGameOver -= GameOver;
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class responsible for spawning enemies
/// </summary>
public class EnemiesSpawner : MonoBehaviour
{
    EnemiesSpawnerLogic spawnerLogic;

    [SerializeField]
    List<EnemyMovementPattern> movementPatterns;
    [SerializeField]
    List<EnemiesRuntimeCollection> enemies;

    [SerializeField]
    GameObject boss;

    [SerializeField]
    FloatPair timeBetweenWaves, amountOfWaves, amountOfEnemiesGroupsPerWave;

    private void Awake()
    {
        spawnerLogic = new EnemiesSpawnerLogic(enemies, movementPatterns, timeBetweenWaves, amountOfWaves, amountOfEnemiesGroupsPerWave, boss);

        EventBroker.OnGameOver += GameOver;

        foreach (EnemiesRuntimeCollection enemy in enemies)
        {
            enemy.PoolObjects(8);
        }
    }

    private void Update()
    {
        spawnerLogic.UpdateEnemySpawnerLogic();
    }

    void GameOver()
    {
        gameObject.SetActive(false);
        EventBroker.OnGameOver -= GameOver;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerLogic
{
    List<EnemyMovementPattern> movementPatterns;
    List<EnemiesRuntimeCollection> enemies;
    List<int> currentEnemiesIndexes;

    GameObject boss;

    int amountOfEnemiesGroups;
    int amountOfWaves;
    int currentWave = 0;
    int currentGroup = 0;

    float timeUntilNextWave = 0;
    float timeUntilNextGroup = 0;

    bool shouldSpawnNextWave = false;

    FloatPair amountOfEnemiesGroupsPerWave;
    FloatPair timeBetweenWaves;

    EnemyMovementPattern movementPatternToUse;
    public EnemiesSpawnerLogic(List<EnemiesRuntimeCollection> _enemies, List<EnemyMovementPattern> _movementPatterns, FloatPair _timeBetweenWaves, FloatPair _amountOfWaves, FloatPair _amountOfEnemyGroupsPerWave, GameObject _boss)
    {
        enemies = _enemies;
        movementPatterns = _movementPatterns;
        amountOfEnemiesGroupsPerWave = _amountOfEnemyGroupsPerWave;
        amountOfWaves = Random.Range(Mathf.RoundToInt(_amountOfWaves.min), Mathf.RoundToInt( _amountOfWaves.max + 1));
        timeBetweenWaves = _timeBetweenWaves;
        boss = _boss;

        currentEnemiesIndexes = new List<int>(enemies.Count);
        SetNextWave();
    }

    public void UpdateEnemySpawnerLogic()
    {
        // If it's time to spawn new wave
        if (Time.time > timeUntilNextWave)
        {
            currentWave++;

            // If we are not on the last wave
            if (currentWave < amountOfWaves)
            {
                // spawn wave
                shouldSpawnNextWave = true;
                currentGroup = 0;
                timeUntilNextGroup = 0;

                // Prepare information for next wave
                SetNextWave();
                timeUntilNextWave = Time.time + Random.Range(timeBetweenWaves.min, timeBetweenWaves.max);
            }
            // If we are on the last wave
            else if (currentWave == amountOfWaves)
            {
                // Spawn a Boss
                boss.SetActive(true);
            }
        }

        // If spawner should spawn next wave and we waited some time
        if (shouldSpawnNextWave && timeUntilNextGroup <= 0)
        {
            // set waiting time for next group
            timeUntilNextGroup = Random.Range(0.5f, 2f);

            SpawnNextGroup(currentEnemiesIndexes[currentGroup]);

            currentGroup++;

            // If spawner spawned all groups, wait for next wave of enemies
            if (currentGroup == amountOfEnemiesGroups)
            {
                shouldSpawnNextWave = false;
            }
        }
        // If spawner should spawn next wave but it's not time to spawn
        else if (shouldSpawnNextWave && timeUntilNextGroup > 0)
        {
            // wait
            timeUntilNextGroup -= Time.deltaTime;
        }
    }

    // Function used to prepare informations for spawning next wave
    void SetNextWave()
    {
        // Amount of groups of enemies that will spawn during one wave
        amountOfEnemiesGroups = Random.Range(Mathf.RoundToInt(amountOfEnemiesGroupsPerWave.min), Mathf.RoundToInt(amountOfEnemiesGroupsPerWave.max + 1));

        currentEnemiesIndexes.Clear();
        for (int i = 0; i < amountOfEnemiesGroups; i++)
        {
            // Add index of the enemy to use for each group
            currentEnemiesIndexes.Add(Random.Range(0, enemies.Count));
        }
    }

    // Function used to spawn group of enemies
    void SpawnNextGroup(int enemyIndex)
    {
        // Get random patern
        int patternToUse = Random.Range(0, movementPatterns.Count + 1);

        // if we have a number higher than amount of movement patterns we have, enemies from that group will use their custom pattern
        if (patternToUse >= movementPatterns.Count)
        {
            // get random amount of enemies to spawn
            int enemiesAmount = Random.Range(2, 6);

            for (int i = 0; i < enemiesAmount; i++)
            {
                var enemyInfo = enemies[enemyIndex].GetEnemyInfo();

                // Tell enemy to use custom pattern
                IInjectMovementBehaviour injectMovement = enemyInfo.MovementBehaviour;
                injectMovement.InjectMovementBehaviour();

                // Tell animator, to not use any animations
                IAnimatable animation = enemyInfo.Animations;
                animation.SetAnimator();
            }
        }
        else
        {
            movementPatternToUse = movementPatterns[patternToUse];

            // Enemy will use this to set it's initial position
            movementPatternToUse.StartingPosition = movementPatternToUse.GetRandomStartingPosition();

            // Enemy will use this to know where and how many times to move, before ending pattern
            List<Vector2> randomMovements = movementPatternToUse.GetRandomPositions();

            for (int i = 0; i < movementPatternToUse.EnemiesAmount; i++)
            {
                // We want to start index at 1 for calculation purposes
                movementPatternToUse.IndexInArray = i + 1;

                var enemyInfo = enemies[enemyIndex].GetEnemyInfo();

                // Tell enemy to use that movement pattern
                IInjectMovementBehaviour injectMovement = enemyInfo.MovementBehaviour;
                injectMovement.InjectMovementBehaviour(movementPatternToUse, randomMovements);

                // Tell animator, to use Animator Controller assosiated with that movement pattern
                IAnimatable animation = enemyInfo.Animations;
                animation.SetAnimator(movementPatternToUse.MovementAnimator, i + 1, movementPatternToUse.EnemiesAmount);
            }
        }
    }
}

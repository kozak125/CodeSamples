using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a movement from left to right along straight line.
/// Use together with up and down animation to create a wave.
/// </summary>

[CreateAssetMenu(menuName = "Enemy/PatternMovement/WavePattern")]
public class EnemyWaveMovementPattern : EnemyMovementPattern
{
    [SerializeField]
    float distanceBetweenEnemies;

    float lastMoveOffset = 2f;

    public override Vector2 SetPosition()
    {
        float xPosition = startingPosition.x - indexInArray - distanceBetweenEnemies;
        return new Vector2(xPosition, startingPosition.y);
    }

    public override Vector2 UpdatePosition(Vector2 position, Vector2 targetPosition, float movementSpeed)
    {
        float distance = (targetPosition - position).magnitude;
        if (distance < 0.1f)
        {
            position = targetPosition;
        }
        else
        {
            position += ((targetPosition - position) / distance) * movementSpeed * Time.deltaTime;
        }
        return position;
    }

    public override List<Vector2> GetRandomPositions()
    {
        List<Vector2> randomMovements = new List<Vector2>(MovementsAmount);
        Vector2 position;

        for (int i = 0; i < MovementsAmount; i++)
        {
            if ((i == 0 || i % 2 == 0) && i != MovementsAmount - 1)
            {
                position = new Vector2(ScreenBounds.right, startingPosition.y);
                randomMovements.Add(position);
            }
            else if (i % 2 == 1 && i != MovementsAmount - 1)
            {
                position = new Vector2(ScreenBounds.left, startingPosition.y);
                randomMovements.Add(position);
            }
            else if (i == MovementsAmount - 1 && i % 2 == 0)
            {
                position = new Vector2(ScreenBounds.right + lastMoveOffset, startingPosition.y);
                randomMovements.Add(position);
            }
            else if (i == MovementsAmount - 1 && i % 2 == 1)
            {
                position = new Vector2(ScreenBounds.left - lastMoveOffset, startingPosition.y);
                randomMovements.Add(position);
            }
        }
        return randomMovements;
    }

    public override Vector2 GetRandomStartingPosition()
    {
        return ScreenBounds.RandomLeftPosition();
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a random movement, where all enemies origin points spawn at one position;
/// Add circular animation and all enemies will spin around common pivot.
/// </summary>

[CreateAssetMenu(menuName = "Enemy/PatternMovement/CirclePattern")]
public class EnemyCircleMovementPattern : EnemyMovementPattern
{
    public override Vector2 SetPosition()
    {
        return startingPosition;
    }

    public override Vector2 UpdatePosition(Vector2 position, Vector2 targetPosition, float movementSpeed)
    {
        // make a proximity check to target position
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
        for (int i = 0; i < MovementsAmount; i++)
        {
            if (i == MovementsAmount - 1)
            {
                randomMovements.Add(ScreenBounds.RandomTopPosition());
            }
            else
            {
                randomMovements.Add(ScreenBounds.GetRandomPosition());
            }
        }
        return randomMovements;
    }

    public override Vector2 GetRandomStartingPosition()
    {
        return ScreenBounds.RandomTopPosition();
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Random movements for CustomMovement
/// </summary>

[CreateAssetMenu(menuName = "Enemy/CustomMovement/Random")]
public class EnemyCustomRandomMovement : EnemyCustomMovement
{
    public override List<Vector2> GenerateRandomMovements()
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

    public override Vector2 SetPosition()
    {
        return ScreenBounds.RandomTopPosition();
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
}

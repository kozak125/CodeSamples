using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class used to make custom movements for each enemy.
/// Each enemy will act independently after accepting movement derived from this class.
/// </summary>

public abstract class EnemyCustomMovement : ScriptableObject, IEnemyMovementBehaviour
{
    [Min(2)]
    public int MovementsAmount;

    [Min(0)]
    public float timeBetweenMovements;

    public float GetTimeBetweenMovements => timeBetweenMovements;

    public abstract List<Vector2> GenerateRandomMovements();

    public abstract Vector2 SetPosition();

    public abstract Vector2 UpdatePosition(Vector2 position, Vector2 targetPosition, float movementSpeed);
}

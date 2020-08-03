using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class used to make pattern movements for groups of enemies.
/// Each enemy will act as part of pattern after accepting movement derived from this class.
/// </summary>

public abstract class EnemyMovementPattern : ScriptableObject, IEnemyMovementBehaviour
{
    public int EnemiesAmount;
    [Min(2)]
    public int MovementsAmount;
    [Min(0)]
    public float timeBetweenMovements;
    public RuntimeAnimatorController MovementAnimator;

    protected Vector2 startingPosition;
    protected int indexInArray;

    public Vector2 StartingPosition { set => startingPosition = value; }

    public int IndexInArray { set => indexInArray = value; }

    public float GetTimeBetweenMovements => timeBetweenMovements;
    public abstract Vector2 SetPosition();

    public abstract Vector2 UpdatePosition(Vector2 position, Vector2 targetPosition, float movementSpeed);

    public abstract List<Vector2> GetRandomPositions();

   public abstract Vector2 GetRandomStartingPosition();
}

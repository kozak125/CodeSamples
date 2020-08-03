using UnityEngine;

public interface IEnemyMovementBehaviour
{
    float GetTimeBetweenMovements { get; }
    Vector2 UpdatePosition(Vector2 positionToChange, Vector2 targetPosition, float movementSpeed);
    Vector2 SetPosition();
}
    

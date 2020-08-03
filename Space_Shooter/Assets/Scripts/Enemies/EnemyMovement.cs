using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement
{
    IMovable positionToChange;

    public delegate Vector2 SetPositionHandeler();
    public delegate Vector2 UpdatePositionHandeler(Vector2 positionToChange, Vector2 targetPosition, float movementSpeed);

    public UpdatePositionHandeler updatePosition;
    public SetPositionHandeler setPosition;

    List<Vector2> movementPositions;

    float timeBetweenMovements, timer = 0;
    float movementSpeed;

    int nextPositionIndex = 0;

    public event Action OnMovementEnded;

    public List<Vector2> MovementPositions { set => movementPositions = value; }

    public float TimeBetweenMovements { set => timeBetweenMovements = value; }

    public EnemyMovement(IMovable _positionToChange, float _movementSpeed)
    {
        positionToChange = _positionToChange;
        movementSpeed = _movementSpeed;
    }

    // Move enemy from target to target, until enemy went through all of them, then raise event to deactivate enemy
    public void UpdatePosition()
    {
        if (updatePosition == null)
        {
            return;
        }
        Vector3 newPosition = updatePosition(positionToChange.GameObjectTransform.localPosition, movementPositions[nextPositionIndex], movementSpeed);
        positionToChange.GameObjectTransform.localPosition = newPosition;

        Vector2 newPosition2D = new Vector2(newPosition.x, newPosition.y);

        if (movementPositions[nextPositionIndex] == newPosition2D && nextPositionIndex < movementPositions.Count - 1)
        {
            if (timer < timeBetweenMovements)
            {
                timer += Time.deltaTime;
            }
            else
            {
                nextPositionIndex++;
                timer = 0f;
            }
        }
        else if (movementPositions[nextPositionIndex] == newPosition2D && nextPositionIndex == movementPositions.Count - 1)
        {
            OnMovementEnded?.Invoke();
        }
    }

    // Set initial position of the enemy
    public void SetPosition()
    {
        Vector2 newPosition = setPosition();
        positionToChange.GameObjectTransform.localPosition = newPosition;

        nextPositionIndex = 0;
    }
}

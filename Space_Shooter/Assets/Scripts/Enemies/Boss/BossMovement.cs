using System;
using UnityEngine;

public class BossMovement
{
    IMovable positionToChange;

    Vector2 targetPosition;

    Vector3 targetPosition3D;

    readonly float movementSpeed = 1f;

    public event Action OnMovementEnded;

    public BossMovement(IMovable _positionToChange, Vector2 _targetPosition)
    {
        positionToChange = _positionToChange;
        targetPosition = _targetPosition;

        targetPosition3D = targetPosition;
    }

    public void UpdatePosition()
    {
        float distance = (targetPosition3D - positionToChange.GameObjectTransform.localPosition).magnitude;
        if (distance < 0.1f)
        {
            positionToChange.GameObjectTransform.localPosition = targetPosition;

            OnMovementEnded?.Invoke();
        }
        else
        {
            positionToChange.GameObjectTransform.localPosition += ((targetPosition3D - positionToChange.GameObjectTransform.localPosition) / distance) * movementSpeed * Time.deltaTime;
        }
    }
}

using System;
using UnityEngine;

public class ProjectileMovement : IProjectileMovement
{
    IMovable positionToChange;

    float projectileSpeed;

    Vector2 projectileMovementDirection;

    public event Action ProjectileOutOfBounds;

    public Vector2 MovementDirection { set => projectileMovementDirection = value; }

    public ProjectileMovement(IMovable _positionToChange)
    {
        positionToChange = _positionToChange;
    }

    public void UpdatePosition()
    {
        Vector3 newPosition = projectileMovementDirection * projectileSpeed * Time.deltaTime;
        positionToChange.GameObjectTransform.localPosition += newPosition;

        // If projectile is out of bounds, deactivate it
        if (ScreenBounds.OutOfBounds(positionToChange.GameObjectTransform.localPosition, 0.5f))
        {
            ProjectileOutOfBounds?.Invoke();
        }
    }

    public void PrepareProjectileToShoot(Vector2 position, float speed)
    {
        positionToChange.GameObjectTransform.localPosition = position;
        positionToChange.GameObjectTransform.localRotation = GetAnlgeFromDirection(projectileMovementDirection);
        projectileSpeed = speed;
    }

    // Determine projectiles rotation based on it's direction
    Quaternion GetAnlgeFromDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        angle = angle < 0 ? angle += 360 : angle;

        return Quaternion.Euler(0f, 0f, angle);
    }
}

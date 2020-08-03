using System;
using UnityEngine;

public class ProjectileLogic
{
    IProjectileMovement projectileMovement;

    bool shouldMove = false;

    public event Action OnDeactivating;

    public ProjectileLogic(IProjectileMovement _projectileMovement)
    {
        projectileMovement = _projectileMovement;
        projectileMovement.ProjectileOutOfBounds += () => OnDeactivating?.Invoke();
    }

    public void HandleCollision(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
        {
            OnDeactivating?.Invoke();
        }
    }

    public void ShootProjectile(Vector2 direction, Vector2 origin, float speed)
    {
        projectileMovement.MovementDirection = direction;
        projectileMovement.PrepareProjectileToShoot(origin, speed);
        shouldMove = true;
    }

    public void UpdateProjectileLogic()
    {
        if (shouldMove)
        {
            projectileMovement.UpdatePosition();
        }
    }
}

using UnityEngine;
public interface IProjectileLogic : ICollisionObserver, IGameObjectDeactivater
{
    void ShootProjectile(Vector2 direction, Vector2 origin, float speed);

    void UpdateProjectileLogic();
}

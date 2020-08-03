using System;
using UnityEngine;
public interface IProjectileMovement : IMovementBehaviour
{
    event Action ProjectileOutOfBounds;
    void PrepareProjectileToShoot(Vector2 position, float speed);
}

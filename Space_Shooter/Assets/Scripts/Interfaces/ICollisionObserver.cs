using UnityEngine;

public interface ICollisionObserver
{
    void HandleCollision(Collider2D collider);
}

using UnityEngine;

/// <summary>
/// Base class for projectiles
/// </summary>

public class Projectile : MonoBehaviour, IProjectile, IMovable
{
    ProjectileLogic projectileLogic;
    ProjectileMovement projectileMovement;

    [HideInInspector]
    public int Damage;

    bool shouldShoot = false;

    public Transform GameObjectTransform => transform;

    public int SetDamage { set => Damage = value; }

    private void Awake()
    {
        projectileMovement = new ProjectileMovement(this);
        projectileLogic = new ProjectileLogic(projectileMovement);

        projectileLogic.OnDeactivating += () => gameObject.SetActive(false);
    }

    private void Update()
    {
        if (shouldShoot)
        {
            projectileLogic.UpdateProjectileLogic();
        }
    }

    public void ShootProjectile(Vector2 direction, Vector2 origin, float speed, int layer)
    {
        projectileLogic.ShootProjectile(direction, origin, speed);

        // Set projectiles layer to ignore/detect desired collisions
        gameObject.layer = layer;

        shouldShoot = true;
    }

    private void OnDisable()
    {
        shouldShoot = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        projectileLogic.HandleCollision(collision.collider);
    }
}

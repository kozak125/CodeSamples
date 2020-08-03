using UnityEngine;
public interface IProjectile
{
    int SetDamage { set; }

    void ShootProjectile(Vector2 direction, Vector2 origin, float speed, int layer);
}

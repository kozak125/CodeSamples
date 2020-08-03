using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic
{
    IShootingBehaviour shoot;
    BossMovement movement;

    int maxHealth;
    int currentHealth;

    bool shouldMove = true;
    bool shouldShoot = false;

    public event Action OnDeactivating;

    public BossLogic(IShootingBehaviour _shoot, BossMovement _movement, int _maxHealth)
    {
        shoot = _shoot;
        movement = _movement;
        maxHealth = _maxHealth;
        currentHealth = maxHealth;

        movement.OnMovementEnded += () => shouldMove = false;
        movement.OnMovementEnded += () => shouldShoot = true;
    }

    public void HandleCollision(Collider2D collider)
    {
        // If enemy is hit by projectile, substract enemy's health by projectile damage
        if (collider.CompareTag("Projectile"))
        {
            currentHealth -= collider.GetComponent<Projectile>().Damage;
        }

        // If enemy has no more life points, deactivate it
        if (currentHealth <= 0)
        {
            OnDeactivating?.Invoke();
            return;
        }
        
    }

    public void UpdateBossLogic()
    {
        if (shouldMove)
        {
            movement.UpdatePosition();
        }
        if (shouldShoot)
        {
            shoot.ShootProjectiles();
        }
    }
}

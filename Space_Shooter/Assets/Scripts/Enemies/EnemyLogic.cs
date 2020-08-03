using System;
using UnityEngine;

public class EnemyLogic
{
    IShootingBehaviour shoot;
    EnemyMovement movement;

    int maxHealth;
    int currentHealth;

    bool shouldCollide = true;

    public event Action OnDeactivating;

    public EnemyLogic(IShootingBehaviour _shoot, EnemyMovement _movement, int _maxHealth)
    {
        shoot = _shoot;
        movement = _movement;
        maxHealth = _maxHealth;
        currentHealth = maxHealth;

        movement.OnMovementEnded += () => OnDeactivating?.Invoke();
    }

    public void HandleCollision(Collider2D collider)
    {
        if (shouldCollide)
        {
            // If enemy is hit by projectile, substract enemy's health by projectile damage
            if (collider.CompareTag("Projectile"))
            {
                currentHealth -= collider.GetComponent<Projectile>().Damage;
            }

            // If enemy is hit by player or has no more life points, deactivate it
            if (currentHealth <= 0 || collider.CompareTag("Player"))
            {
                OnDeactivating?.Invoke();
                shouldCollide = false;
                return;
            }
        }
    }

    public void UpdateEnemyLogic()
    {
        shoot.ShootProjectiles();
        movement.UpdatePosition();
    }

    // Reset varabiles when enemy is activated
    public void PrepareEnemy()
    {
        movement.SetPosition();

        shouldCollide = true;
        currentHealth = maxHealth;
    }
}

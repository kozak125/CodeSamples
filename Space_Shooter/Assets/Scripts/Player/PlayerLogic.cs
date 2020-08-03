using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic
{
    PlayerInput playerInput;
    IPlayerMovementBehaviour movement;
    IShootingBehaviour shoot;

    FloatVariable livesAmount;

    bool isMoving = false;
    bool isShooting = false;

    public event Action OnDeactivating;
    public event Action OnActivating;

    delegate void CurrentMovement();

    CurrentMovement currentMovement;

    public PlayerLogic(IPlayerMovementBehaviour _movement, IShootingBehaviour _shoot,PlayerInput _playerInput, FloatVariable _livesAmount)
    {
        playerInput = _playerInput;
        movement = _movement;
        shoot = _shoot;
        livesAmount = _livesAmount;

        playerInput.onActionTriggered += ReadInput;
        movement.OnFinishedSpawnMovement += OnSpawned;
        currentMovement = movement.UpdatePosition;
    }

    public void HandleCollision(Collider2D collider)
    {
        if (collider.CompareTag("Projectile") || collider.CompareTag("Enemy"))
        {
            // Raise the event when player has been destroyed
            OnDeactivating?.Invoke();

            // Turn off all damage and input, reset movement
            playerInput.onActionTriggered -= ReadInput;
            isMoving = true;
            isShooting = false;
            movement.MovementDirection = Vector2.zero;

            // Go to point outside of screen
            movement.SetSpawnPositions();

            // Substruct lives
            livesAmount.Value--;

            EventBroker.CallOnPlayerDestroyed();

            // If player has no more lives, end game
            if (livesAmount.Value == 0)
            {
                EventBroker.CallOnGameOver();
                return;
            }

            // Move player inside screen
            currentMovement = movement.SpawnPlayer;
        }
    }

    public void UpdatePlayerLogic()
    {
        if (isMoving)
        {
            currentMovement();
        }
        if (isShooting)
        {
            shoot.ShootProjectiles();
        }
    }

    void ReadInput(InputAction.CallbackContext context)
    {
        if (!context.performed && !context.canceled)
        {
            return;
        }
        if (context.action == playerInput.actions.FindAction("Fire"))
        {
            if (context.ReadValue<float>() != 0)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }
        }
        if (context.action == playerInput.actions.FindAction("Movement"))
        {
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                movement.MovementDirection = context.ReadValue<Vector2>();
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    // Call it, when player has moved to point inside screen after being destroyed
    void OnSpawned()
    {
        playerInput.onActionTriggered += ReadInput;

        currentMovement = movement.UpdatePosition;

        OnActivating?.Invoke();
    }
}

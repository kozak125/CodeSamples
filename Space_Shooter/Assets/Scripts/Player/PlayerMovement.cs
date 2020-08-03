using System;
using UnityEngine;

public class PlayerMovement : IPlayerMovementBehaviour
{
    IMovable positionToChange;

    float movementSpeed;
    readonly float spawnSpeed = 2;
    readonly float errorMarigin = 0.1f;

    Vector2 playerMovementDirection;
    Vector2 spawnTargetPosition;
    Vector2 spawnStartPosition;

    Vector3 spawnTargetPosition3D;
    Vector3 spawnStartPosition3D;

    public event Action OnFinishedSpawnMovement;

    public Vector2 MovementDirection { set => playerMovementDirection = value; }

    public PlayerMovement(float _movementSpeed, IMovable _positionToChange)
    {
        movementSpeed = _movementSpeed;
        positionToChange = _positionToChange;
    }

    public void UpdatePosition()
    {
        Vector3 newPosition = playerMovementDirection * movementSpeed * Time.deltaTime;
        Vector3 positionToCheck = positionToChange.GameObjectTransform.position + newPosition;

        //prevent player from going out of bounds
        if (!ScreenBounds.OutOfBounds(positionToCheck, -0.5f))
        {
            positionToChange.GameObjectTransform.position = positionToCheck;
        }
    }

    // Function used animate player's movement after player has been destroyed
    public void SpawnPlayer()
    {
        // If player is close to spawn target position, end the movement and raise event
        if ((spawnTargetPosition3D - positionToChange.GameObjectTransform.position).sqrMagnitude < errorMarigin * errorMarigin)
        {
            positionToChange.GameObjectTransform.position = spawnTargetPosition3D;
            spawnTargetPosition3D = spawnTargetPosition;
            OnFinishedSpawnMovement?.Invoke();
        }
        // If player is far, move player towards target
        else
        {
            spawnStartPosition3D += Vector3.up * spawnSpeed * Time.deltaTime;
            positionToChange.GameObjectTransform.position = spawnStartPosition3D;
        }
    }

    // Set the start and target positions for player spawning
    public void SetSpawnPositions()
    {
        spawnStartPosition = new Vector2(0f, ScreenBounds.bottom - 1);
        spawnStartPosition3D = spawnStartPosition;

        spawnTargetPosition = new Vector2(0f, ScreenBounds.bottom + 0.3f);
        spawnTargetPosition3D = spawnTargetPosition;

        positionToChange.GameObjectTransform.position = spawnStartPosition;
    }
}

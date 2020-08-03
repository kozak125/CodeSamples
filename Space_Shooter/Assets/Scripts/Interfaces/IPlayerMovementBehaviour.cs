using System;
using UnityEngine;

public interface IPlayerMovementBehaviour : IMovementBehaviour
{

    event Action OnFinishedSpawnMovement;
    void SpawnPlayer();
    void SetSpawnPositions();
}

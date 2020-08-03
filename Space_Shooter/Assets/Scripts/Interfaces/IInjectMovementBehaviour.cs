using System.Collections.Generic;
using UnityEngine;

public interface IInjectMovementBehaviour
{
    void InjectMovementBehaviour();
    void InjectMovementBehaviour(IEnemyMovementBehaviour enemyMovement, List<Vector2> movementPositions);
}

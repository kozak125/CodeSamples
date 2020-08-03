using UnityEngine;

public interface IMovementBehaviour
{
    Vector2 MovementDirection { set; }
    void UpdatePosition();
}

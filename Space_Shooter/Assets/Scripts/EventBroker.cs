using System;
using UnityEngine;

public class EventBroker
{
    // Event raised by destroyed character to spawn praticles at it's presentation position
    public static event Action<Vector2> OnCharacterDestroyedSpawnParticles;

    public static void CallOnCharacterDestroyedSpawnParticles(Vector2 particlesTarget)
    {
        OnCharacterDestroyedSpawnParticles?.Invoke(particlesTarget);
    }

    // Event raised, when player has been destroyed
    public static event Action OnPlayerDestroyed;

    public static void CallOnPlayerDestroyed()
    {
        OnPlayerDestroyed?.Invoke();
    }

    // Event raised when player lost all lives or defeated a boss
    public static event Action OnGameOver;

    public static void CallOnGameOver()
    {
        OnGameOver?.Invoke();
    }
}

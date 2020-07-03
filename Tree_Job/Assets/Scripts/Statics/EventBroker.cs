using System;
using UnityEngine;

/// <summary>
/// Class used to broker events between clases. Part of Publisher/Subscriber pattern
/// </summary>
public class EventBroker
{
    /// <summary>
    /// Event used to signal, that animation has ended
    /// </summary>
    public static event Action OnAnimationEnded;

    /// <summary>
    /// Raises the OnAnimationEnded event, while there are listeners to it
    /// </summary>
    public static void CallOnAnimationEnded()
    {
        OnAnimationEnded?.Invoke();
    }

    /// <summary>
    /// Event used to signal, that tree has been destroyed
    /// </summary>
    public static event Action OnTreeDestroyed;

    /// <summary>
    /// Raises the OnTreeDestroyed event, while there are listeners to it
    /// </summary>
    public static void CallOnTreeDestroyed()
    {
        OnTreeDestroyed?.Invoke();
    }

    /// <summary>
    /// Event used to signal, that tree is about to change
    /// </summary>
    public static event Action<Vector3> OnTreeChanging;

    /// <summary>
    /// Raises the OnTreeChanging event, while there are listeners to it
    /// </summary>
    public static void CallOnTreeChanging(Vector3 distanceBetweenTrees)
    {
        OnTreeChanging?.Invoke(distanceBetweenTrees);
    }

    /// <summary>
    /// Event used to signal, that game has ended
    /// </summary>
    public static event Action OnGameEnded;

    /// <summary>
    /// Raises the OnGameEnded event, while there are listeners to it
    /// </summary>
    public static void CallOnGameEnded()
    {
        OnGameEnded?.Invoke();
    }
}

using System;

public interface IPlayerLogic : ICollisionObserver, IGameObjectDeactivater
{
    event Action OnActivating;
    void UpdatePlayerLogic();
}

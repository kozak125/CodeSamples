public interface IEnemyLogic : IGameObjectDeactivater, ICollisionObserver
{
    void PrepareEnemy();
    void UpdateEnemyLogic();
}

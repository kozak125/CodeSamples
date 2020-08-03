using UnityEngine;
using System;

/// <summary>
/// Class for storing and obtaining information about enemies
/// </summary>

[CreateAssetMenu(menuName = "Enemy/Enemies Collection")]
public class EnemiesRuntimeCollection : GameObjectsRuntimeCollection
{
    public (IInjectMovementBehaviour MovementBehaviour, IAnimatable Animations) GetEnemyInfo()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                if (pooledObject.TryGetComponent(out IInjectMovementBehaviour enemy))
                {
                    pooledObject.SetActive(true);
                    return new ValueTuple<IInjectMovementBehaviour, IAnimatable>(enemy, pooledObject.GetComponentInChildren<IAnimatable>());
                }
                else
                {
                    Debug.LogError("Enemy GameObject is missing an IGetMovementBehaviour interface.");
                    return default;
                }
            }
        }

        GameObject newObject = Instantiate(objectToPool);
        pooledObjects.Add(newObject);

        if (newObject.TryGetComponent(out IInjectMovementBehaviour newEnemy))
        {
            return new ValueTuple<IInjectMovementBehaviour, IAnimatable>(newEnemy, newObject.GetComponentInChildren<IAnimatable>());
        }
        else
        {
            Debug.LogError("Enemy GameObject is missing an IGetMovementBehaviour interface.");
            return default;
        }
    }
}

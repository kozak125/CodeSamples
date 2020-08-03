using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to be dervied from by any SO gameobject collection, that want's to pool objects
/// </summary>

public abstract class GameObjectsRuntimeCollection : ScriptableObject
{
    [SerializeField]
    protected GameObject objectToPool;

    protected List<GameObject> pooledObjects;

    public void PoolObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObject = Instantiate(objectToPool);
            newObject.SetActive(false);
            pooledObjects.Add(newObject);
        }
    }

    private void OnEnable()
    {
        pooledObjects = new List<GameObject>();
    }
}

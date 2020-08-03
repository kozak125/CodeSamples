using UnityEngine;

/// <summary>
/// Class for storing and obtaining information about projectiles
/// </summary>

[CreateAssetMenu(menuName = "Projectiles/Projectiles Collection")]
public class ProjectilesRuntimeCollection : GameObjectsRuntimeCollection
{
    public IProjectile GetPooledProjectile()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                if (pooledObject.TryGetComponent(out IProjectile projectile))
                {
                    pooledObject.SetActive(true);
                    return projectile;
                }
                else
                {
                    Debug.LogError("Projectile GameObject is missing an IProjectile interface.");
                    return null;
                }
            }
        }

        GameObject newObject = Instantiate(objectToPool);
        pooledObjects.Add(newObject);

        if (newObject.TryGetComponent(out IProjectile newProjectile))
        {
            return newProjectile;
        }
        else
        {
            Debug.LogError("Projectile GameObject is missing an IProjectile interface.");
            return null;
        }
    }
}

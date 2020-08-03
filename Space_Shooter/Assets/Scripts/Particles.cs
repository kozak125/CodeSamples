using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField]
    ParticleSystem explosionParticles;

    List<ParticleSystem> pooledParticles;

    private void Awake()
    {
        EventBroker.OnCharacterDestroyedSpawnParticles += SpawnExplosionParticles;
        EventBroker.OnGameOver += GameOver;

        PoolParticles(3);
    }

    void SpawnExplosionParticles(Vector2 particlesTarget)
    {
        ParticleSystem explosion = GetPooledParticle();
        explosion.gameObject.transform.position = particlesTarget;
        StartCoroutine(DeactivateParticleCoroutine(explosion));
    }

    // Pools explosion particles, can be changed to pool any particles if needed
    void PoolParticles(int amountToPool)
    {
        pooledParticles = new List<ParticleSystem>(amountToPool);
        for (int i = 0; i < amountToPool; i++)
        {
            pooledParticles.Add(Instantiate(explosionParticles, transform));
            pooledParticles[i].gameObject.SetActive(false);
        }
    }

    // Gets pooled explosion particles, can be changed to get any particles if needed
    ParticleSystem GetPooledParticle()
    {
        foreach (ParticleSystem pooledParticle in pooledParticles)
        {
            if (!pooledParticle.gameObject.activeInHierarchy)
            {
                pooledParticle.gameObject.SetActive(true);
                return pooledParticle;
            }
        }

        ParticleSystem newParticle = Instantiate(explosionParticles, transform);
        pooledParticles.Add(newParticle);

        return newParticle;
    }

    IEnumerator DeactivateParticleCoroutine(ParticleSystem particle)
    {
        while (particle.isPlaying)
        {
            yield return null;
        }
        particle.gameObject.SetActive(false);
    }

    void GameOver()
    {
        EventBroker.OnCharacterDestroyedSpawnParticles -= SpawnExplosionParticles;
        EventBroker.OnGameOver -= GameOver;
    }
}

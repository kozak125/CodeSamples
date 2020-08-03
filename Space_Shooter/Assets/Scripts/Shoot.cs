using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base shooting class, that can be used by any ICanShoot game object
/// </summary>
/// 
public class Shoot : IShootingBehaviour
{
    List<Weapons> weapons;
    List<float> timeBetweenRounds;
    List<float> timeBetweenBulletShots;
    List<float> timeBetweenGunShots;
    List<int> consecutiveShotsLeft;
    List<int> gunsToShoot;
    List<bool> shouldFireNextRound;
    List<bool> shouldFireFromGun;
    List<Vector2> shootingDirections;

    int projectilesLayer;

    public Shoot(ICanShoot _projectiles, int _projectilesLayer)
    {
        weapons = _projectiles.Weapons;
        projectilesLayer = _projectilesLayer;

        SetShotsProperties();
    }

    // Function used to set properties for each weapon separately
    void SetShotsProperties()
    {
        shootingDirections = new List<Vector2>(weapons.Count);
        timeBetweenRounds = new List<float>(weapons.Count);
        timeBetweenBulletShots = new List<float>(weapons.Count);
        timeBetweenGunShots = new List<float>(weapons.Count);
        consecutiveShotsLeft = new List<int>(weapons.Count);
        gunsToShoot = new List<int>(weapons.Count);
        shouldFireNextRound = new List<bool>(weapons.Count);
        shouldFireFromGun = new List<bool>(weapons.Count);

        for (int i = 0; i < weapons.Count; i++)
        {
            shootingDirections.Add(weapons[i].weaponsProperties.ProjectileDirection);

            timeBetweenRounds.Add(0f + Random.Range(weapons[i].weaponsProperties.RandomShotDelay.min, weapons[i].weaponsProperties.RandomShotDelay.max));
            timeBetweenBulletShots.Add(0f + Random.Range(weapons[i].weaponsProperties.RandomShotDelay.min, weapons[i].weaponsProperties.RandomShotDelay.max));
            timeBetweenGunShots.Add(0f + Random.Range(weapons[i].weaponsProperties.RandomShotDelay.min, weapons[i].weaponsProperties.RandomShotDelay.max));

            shouldFireNextRound.Add(default);
            shouldFireFromGun.Add(default);

            consecutiveShotsLeft.Add(weapons[i].weaponsProperties.ConsecutiveShots);
            gunsToShoot.Add(default);
        }
    }

    // Set up function for individual shots
    public void ShootProjectiles()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            // Amount of shots in each "round" consists of number of consecutive shots and amount guns assosiated with that weapon
            if (Time.time >= timeBetweenRounds[i])
            {
                shouldFireNextRound[i] = true;
                timeBetweenRounds[i] = Time.time + weapons[i].weaponsProperties.FireRate;
            }

            // If we are ready to shoot next round and we will fire from all guns at once
            if (shouldFireNextRound[i] && weapons[i].weaponsProperties.ShouldFireAllAtOnce)
            {
                // Shoot until we have no consecutive shots left
                if (Time.time >= timeBetweenBulletShots[i] && consecutiveShotsLeft[i] > 0)
                {
                    ShootProjectile(weapons[i].guns, weapons[i].weaponsProperties, shootingDirections[i]);

                    timeBetweenBulletShots[i] = Time.time + weapons[i].weaponsProperties.TimeBetweenBulletShots;
                    consecutiveShotsLeft[i]--;
                }

                if (consecutiveShotsLeft[i] == 0)
                {
                    consecutiveShotsLeft[i] = weapons[i].weaponsProperties.ConsecutiveShots;
                    shouldFireNextRound[i] = false;
                }
            }
            // If we are ready to shoot next round and we will fire from one gun at a time
            else if (shouldFireNextRound[i] && !weapons[i].weaponsProperties.ShouldFireAllAtOnce)
            {
                // If we are ready to shoot from individual gun
                if (Time.time >= timeBetweenGunShots[i] && gunsToShoot[i] < weapons[i].guns.Count)
                {
                    shouldFireFromGun[i] = true;
                    timeBetweenGunShots[i] = Time.time + weapons[i].weaponsProperties.TimeBetweenGunShots;
                    gunsToShoot[i]++;
                }

                // Shoot until we have no consecutive shots left
                if (Time.time >= timeBetweenBulletShots[i] && consecutiveShotsLeft[i] > 0 && shouldFireFromGun[i])
                {
                    ShootProjectile(weapons[i].guns[gunsToShoot[i] - 1], weapons[i].weaponsProperties, shootingDirections[i]);

                    timeBetweenBulletShots[i] = Time.time + weapons[i].weaponsProperties.TimeBetweenBulletShots;
                    consecutiveShotsLeft[i]--;
                }

                // If we have no consecutive shots left, try shooting from another gun
                if (consecutiveShotsLeft[i] == 0)
                {
                    consecutiveShotsLeft[i] = weapons[i].weaponsProperties.ConsecutiveShots;
                    shouldFireFromGun[i] = false;
                }

                // If we have no guns left to shoot, wait for next "round"
                if (gunsToShoot[i] == weapons[i].guns.Count && !shouldFireFromGun[i])
                {
                    gunsToShoot[i] = 0;
                    shouldFireNextRound[i] = false;
                }
            }
        }
    }

    // Use this function to shoot from all guns at once
    void ShootProjectile(List<GameObject> guns, WeaponProperties weaponsProperties, Vector2 shootingDirection)
    {
        for (int i = 0; i < guns.Count; i++)
        {
            IProjectile projectile = weaponsProperties.Projectiles.GetPooledProjectile();
            projectile.SetDamage = weaponsProperties.Damage;
            projectile.ShootProjectile(shootingDirection, guns[i].transform.position, weaponsProperties.ProjectileSpeed, projectilesLayer);
        }
    }

    // Use this function to shoot from individual guns
    void ShootProjectile(GameObject gun, WeaponProperties weaponsProperties, Vector2 shootingDirection)
    {
        IProjectile projectile = weaponsProperties.Projectiles.GetPooledProjectile();
        projectile.SetDamage = weaponsProperties.Damage;
        projectile.ShootProjectile(shootingDirection, gun.transform.position, weaponsProperties.ProjectileSpeed, projectilesLayer);
    }
}

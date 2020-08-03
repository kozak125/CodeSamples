using UnityEngine;

/// <summary>
/// Properties for custom weapons
/// </summary>

[CreateAssetMenu(menuName = "Projectiles/Weapon")]
public class WeaponProperties : ScriptableObject
{
    [Tooltip("Projectiles to use with this weapon")]
    public ProjectilesRuntimeCollection Projectiles;

    [Tooltip("Speed of projectiles")]
    public float ProjectileSpeed = 5;

    [Tooltip("Shots fired one after another as a part of one round of shots")]
    public int ConsecutiveShots = 1;

    [Tooltip("Should all guns fire at once, or one at a time")]
    public bool ShouldFireAllAtOnce = false;

    [Min(0.1f)]
    [Tooltip("Time between individual guns shooting. It should be smaller than FireRate/Amount of guns to shoot and bigger than TimeBetweenBulletShots. Only important when ShouldFireAllAtOnce = false")]
    public float TimeBetweenGunShots = 0.1f;
    [Min(0.1f)]
    [Tooltip("Time between bullets in consecutive shots fire. Should be smaller than FireRate/ConsecutiveShots.")]
    public float TimeBetweenBulletShots = 0.1f;
    [Min(0.1f)]
    [Tooltip("Time between individual rounds. This should be higher than TimeBetweenGunShots and TimeBetweenBulletShots")]
    public float FireRate = 1f;

    [Tooltip("Delay shoots randomly to add randomness to enemies")]
    public FloatPair RandomShotDelay;

    [Min(1)]
    [Tooltip("Damage caused by the projectile")]
    public int Damage;

    [Tooltip("Direction of projectile, this will always be normilized on play")]
    public Vector2 ProjectileDirection;

    private void OnEnable()
    {
        ProjectileDirection.Normalize();
    }
}

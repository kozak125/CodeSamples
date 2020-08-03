using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Base class for player, it's responsible for communication of player and scene
/// </summary>
public class Player : MonoBehaviour, IMovable, ICanShoot
{
    PlayerMovement playerMovement;
    PlayerLogic playerLogic;
    Shoot playerShoot;
    PlayerInput playerInput;

    [SerializeField]
    List<Weapons> weapons;

    [SerializeField]
    float movementSpeed = 1;

    [SerializeField]
    int startingLives;

    [SerializeField]
    FloatVariable currentLives;

    Action gameOverHandler;

    bool isInvincible = false;
    bool isGameOver = false;

    WaitForSeconds spawnWait = new WaitForSeconds(1f);

    public Transform GameObjectTransform => transform;

    public List<Weapons> Weapons { get => weapons; }

    private void Awake()
    {
        // set dependencies
        playerMovement = new PlayerMovement(movementSpeed, this);
        playerShoot = new Shoot(this, 10);
        playerInput = GetComponent<PlayerInput>();
        playerLogic = new PlayerLogic(playerMovement, playerShoot, playerInput, currentLives);

        playerLogic.OnDeactivating += Deactivate;
        playerLogic.OnActivating += Activate;

        // reset lives
        currentLives.Value = startingLives;

        gameOverHandler = () => isGameOver = true;
        EventBroker.OnGameOver += gameOverHandler;

        // pool projectiles to shoot
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weaponsProperties.Projectiles.PoolObjects(10 * weapons[i].guns.Count);
        }
    }

    void Deactivate()
    {
        isInvincible = true;
        EventBroker.CallOnCharacterDestroyedSpawnParticles(transform.position);
    }

    void Activate()
    {
        StartCoroutine(WaitForDamage());
    }

    private void Update()
    {
        if (!isGameOver)
        {
            playerLogic.UpdatePlayerLogic();
        }
        else if (isGameOver)
        {
            EventBroker.OnGameOver -= gameOverHandler;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvincible && !isGameOver)
        {
            playerLogic.HandleCollision(collision.collider);
        }
    }

    IEnumerator WaitForDamage()
    {
        yield return spawnWait;

        isInvincible = false;
    }
}

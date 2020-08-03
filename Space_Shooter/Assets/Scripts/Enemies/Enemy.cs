using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMovable, ICanShoot, IInjectMovementBehaviour
{
    EnemyLogic enemyLogic;
    Shoot shoot;
    EnemyMovement movement;

    [SerializeField]
    EnemyCustomMovement customMovement;

    [SerializeField]
    List<Weapons> weapons;

    [SerializeField]
    GameObject presentation;

    [SerializeField][Min(1)]
    int health;

    [SerializeField]
    float movementSpeed;

    Action gameOverHandler;

    bool isGameOver = false;

    public Transform GameObjectTransform => transform;

    public List<Weapons> Weapons => weapons;

    private void Awake()
    {
        shoot = new Shoot(this, 11);
        movement = new EnemyMovement(this, movementSpeed);
        enemyLogic = new EnemyLogic(shoot, movement, health);

        enemyLogic.OnDeactivating += Deactivate;

        gameOverHandler = () => isGameOver = true;
        EventBroker.OnGameOver += gameOverHandler;

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weaponsProperties.Projectiles.PoolObjects(2 * weapons[i].guns.Count);
        }
    }

    void Deactivate()
    {
        EventBroker.CallOnCharacterDestroyedSpawnParticles(presentation.transform.position);

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            enemyLogic.UpdateEnemyLogic();
        }
        else if (isGameOver)
        {
            EventBroker.OnGameOver -= gameOverHandler;
        }
    }

    // Function used to inject enemy with it's own custom movement
    public void InjectMovementBehaviour()
    {
        movement.setPosition = customMovement.SetPosition;
        movement.updatePosition = customMovement.UpdatePosition;
        movement.MovementPositions = customMovement.GenerateRandomMovements();
        movement.TimeBetweenMovements = customMovement.timeBetweenMovements;

        enemyLogic.PrepareEnemy();
    }

    // Function used to inject enemy with movement pattern
    public void InjectMovementBehaviour(IEnemyMovementBehaviour enemyMovement, List<Vector2> movementPositions)
    {
        if (enemyMovement != null)
        {
            movement.setPosition = enemyMovement.SetPosition;
            movement.updatePosition = enemyMovement.UpdatePosition;
            movement.MovementPositions = movementPositions;
            movement.TimeBetweenMovements = enemyMovement.GetTimeBetweenMovements;

            enemyLogic.PrepareEnemy();
        }
    }  
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameOver)
        {
            enemyLogic.HandleCollision(collision.collider);
        }
    }
}

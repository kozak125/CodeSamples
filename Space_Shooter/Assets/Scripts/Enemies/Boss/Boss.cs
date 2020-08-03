using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IMovable, ICanShoot
{
    BossLogic bossLogic;
    BossMovement bossMovement;
    Shoot shoot;

    [SerializeField]
    List<Weapons> weapons;

    Vector2 targetPosition = new Vector2(0, 1.7f);

    [SerializeField]
    [Min(1)]
    int health;

    bool isGameOver;

    private void Awake()
    {
        shoot = new Shoot(this, 11);
        bossMovement = new BossMovement(this, targetPosition);
        bossLogic = new BossLogic(shoot, bossMovement, health);

        bossLogic.OnDeactivating += Deactivate;

        EventBroker.OnGameOver += () => isGameOver = true;
    }

    public Transform GameObjectTransform => transform;

    public List<Weapons> Weapons => weapons;

    void Update()
    {
        if (!isGameOver)
        {
            bossLogic.UpdateBossLogic();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameOver)
        {
            bossLogic.HandleCollision(collision.collider);
        }
    }

    void Deactivate()
    {
        // when boss dies, it's game over
        EventBroker.CallOnGameOver();
        gameObject.SetActive(false);
    }
}

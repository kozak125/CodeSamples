using UnityEngine;

/// <summary>
/// Animate presentation game object of enemy
/// </summary>

[RequireComponent(typeof(Animator))]
public class EnemiesAnimations : MonoBehaviour, IAnimatable
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        EventBroker.OnGameOver += StopAnimations;
    }

    public void SetAnimator()
    {
        animator.runtimeAnimatorController = null;
    }

    public void SetAnimator(RuntimeAnimatorController animatorController, float enemyIndex, float amountOfEnemies)
    {
        animator.runtimeAnimatorController = animatorController;

        // Offset animations for pattern movement, so enemies can be spawned at the same time
        if (animatorController != null)
        {
            float offset = enemyIndex / amountOfEnemies;
            animator.SetFloat("Offset", offset);
        }
    }

    void StopAnimations()
    {
        animator.enabled = false;
    }
}

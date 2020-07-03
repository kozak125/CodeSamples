using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
/// <summary>
/// Animator controller for the GenericTree class
/// </summary>
public class GenericTreeAnimationManager : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Function used to start bouncing animation
    /// </summary>
    public void Bounce()
    {
        animator.SetBool("ShouldBounce", true);

        StartCoroutine(CheckIfBouncingCoroutine());
    }

    /// <summary>
    /// Coroutine used to reset value for bouncing animation, so it returns and stays in the default state
    /// </summary>
    IEnumerator CheckIfBouncingCoroutine()
    {
        // make sure to wait a frame so our animator state has changed
        yield return null;

        animator.SetBool("ShouldBounce", false);
    }
}
    

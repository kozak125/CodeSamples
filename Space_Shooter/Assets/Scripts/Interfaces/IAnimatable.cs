using UnityEngine;

public interface IAnimatable
{
    void SetAnimator();
    void SetAnimator(RuntimeAnimatorController animatorController, float index, float amountOfEnemies);
}

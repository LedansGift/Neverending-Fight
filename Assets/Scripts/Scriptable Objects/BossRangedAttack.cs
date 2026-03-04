using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Ranged Attack",
    menuName = "Boss Attack/Ranged Attack",
    order = 2
)]
public class BossRangedAttack : BossAttackNode
{
    [SerializeField]
    private float patternStartDelay = 0.5f;

    [SerializeField]
    private float patternEndDelay = 0.5f;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private ProjectilePattern pattern;

    [SerializeField]
    private string animationTrigger;

    public override void PerformAttack(Transform attackTransform, Action OnAttackFinished)
    {
        CheckAvailableProjectiles();

        ProjectileManager.Instance.SpawnProjectilePattern(
            projectile,
            pattern,
            patternStartDelay,
            patternEndDelay,
            attackTransform,
            ProjectilePatternFinished
        );
        this.OnAttackFinished = OnAttackFinished;
    }

    private void CheckAvailableProjectiles()
    {
        if (
            !ProjectileManager.Instance.CheckAreProjectilesInitialised(
                projectile,
                out int projectileInitialised
            ) || (projectileInitialised < pattern.projectileNumber * pattern.patternWaves)
        )
        {
            ProjectileManager.Instance.InitialiseProjectileSet(
                projectile,
                pattern.projectileNumber * pattern.patternWaves
            );
        }
    }

    private void ProjectilePatternFinished()
    {
        FinishAttack();
    }

    public int GetAnimationTrigger()
    {
        return Animator.StringToHash(animationTrigger);
    }
}

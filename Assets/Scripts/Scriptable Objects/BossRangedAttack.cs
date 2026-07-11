using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Ranged Attack",
    menuName = "Boss Attack/Ranged Attack",
    order = 2
)]
public class BossRangedAttack : BossAttackNode
{
    [SerializeField]
    private bool waitForSpawnsToFinish = true;

    [SerializeField]
    private bool worldPositionPattern = false;

    [SerializeField]
    private float patternStartDelay = 0.5f;

    [SerializeField]
    private float patternEndDelay = 0.5f;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private ProjectilePattern pattern;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    )
    {
        CheckAvailableProjectiles();

        Action patternFinish = null;
        Transform spawnTransform = attacker.transform;

        if (waitForSpawnsToFinish)
        {
            patternFinish = FinishProjectilePattern;
        }
        else
        {
            ProjectileManager.Instance.StartCoroutine(PatternFinishDelay());
        }

        if (worldPositionPattern)
        {
            spawnTransform = ProjectileManager.Instance.transform;
        }

        ProjectileManager.Instance.SpawnProjectilePattern(
            projectile,
            pattern,
            patternStartDelay,
            patternEndDelay,
            spawnTransform,
            patternFinish
        );

        this.OnAttackFinished = OnAttackFinished;
    }

    private void FinishProjectilePattern()
    {
        FinishAttack();
    }

    private IEnumerator PatternFinishDelay()
    {
        yield return new WaitForSeconds(patternEndDelay);
        FinishProjectilePattern();
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
}

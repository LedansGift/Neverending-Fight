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
    private ProjectilePatternStruct pattern;

    [SerializeField]
    private ProjectilePatternSO patternOverride;

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
            GetActivePattern(),
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

    private ProjectilePatternStruct GetActivePattern()
    {
        return (patternOverride != null) ? patternOverride.projectilePattern : pattern;
    }

    private IEnumerator PatternFinishDelay()
    {
        yield return new WaitForSeconds(patternEndDelay);
        FinishProjectilePattern();
    }

    private void CheckAvailableProjectiles()
    {
        ProjectilePatternStruct activePattern = GetActivePattern();

        int desiredProjectiles = activePattern.projectileNumber * activePattern.patternWaves;

        // foreach (ProjectilePatternSO additionalPattern in activePattern.additionalWaves)
        // {
        //     desiredProjectiles +=
        //         activePattern.projectileNumber * activePattern.patternWaves;
        // }

        if (
            !ProjectileManager.Instance.CheckAreProjectilesInitialised(
                projectile,
                out int projectileInitialised
            ) || (projectileInitialised < desiredProjectiles)
        )
        {
            ProjectileManager.Instance.InitialiseProjectileSet(projectile, desiredProjectiles);
        }
    }
}

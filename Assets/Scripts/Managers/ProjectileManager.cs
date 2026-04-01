using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }

    private int mapperIndex = 0;
    private List<int> projectileIndeces;
    private List<List<Projectile>> projectileSets;
    private Dictionary<GameObject, int> projectileMapper;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        mapperIndex = 0;
        projectileIndeces = new List<int>();
        projectileSets = new List<List<Projectile>>();
        projectileMapper = new Dictionary<GameObject, int>();
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += DeactivateAllProjectiles;
        BossFormManager.OnPhaseFinished += DeactivateAllProjectiles;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= DeactivateAllProjectiles;
        BossFormManager.OnPhaseFinished -= DeactivateAllProjectiles;
    }

    private bool CheckAreProjectilesInitialised(
        GameObject projectileType,
        out int projectileIndex,
        out int projectileSetIndex
    )
    {
        if (!projectileMapper.TryGetValue(projectileType, out int outProjectileSetIndex))
        {
            //Debug.Log("Projectile type has not been instantiated");
            projectileIndex = -1;
            projectileSetIndex = outProjectileSetIndex;
            return false;
        }

        projectileIndex = projectileIndeces[outProjectileSetIndex];
        projectileSetIndex = outProjectileSetIndex;
        return true;
    }

    public bool CheckAreProjectilesInitialised(
        GameObject projectileType,
        out int projectilesInitialised
    )
    {
        if (!projectileMapper.TryGetValue(projectileType, out int outProjectileSetIndex))
        {
            projectilesInitialised = 0;
            return false;
        }

        projectilesInitialised = projectileSets[outProjectileSetIndex].Count;
        return true;
    }

    public void InitialiseProjectileSet(GameObject newProjectilePrefab, int amount)
    {
        if (
            CheckAreProjectilesInitialised(
                newProjectilePrefab,
                out int projectileIndex,
                out int projectileSetIndex
            )
        )
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile newProjectile = Instantiate(newProjectilePrefab, transform)
                    .GetComponent<Projectile>();

                projectileSets[projectileSetIndex].Add(newProjectile);
            }

            return;
        }

        projectileMapper.Add(newProjectilePrefab, mapperIndex);
        mapperIndex++;

        List<Projectile> newProjectileSet = new List<Projectile>();

        for (int i = 0; i < amount; i++)
        {
            Projectile newProjectile = Instantiate(newProjectilePrefab, transform)
                .GetComponent<Projectile>();

            newProjectileSet.Add(newProjectile);
        }

        projectileSets.Add(newProjectileSet);
        projectileIndeces.Add(0);
    }

    public Projectile SpawnProjectile(
        GameObject projectileType,
        Vector3 spawnPosition,
        Vector3 flightDirection
    )
    {
        if (
            !CheckAreProjectilesInitialised(
                projectileType,
                out int projectileIndex,
                out int projectileSetIndex
            )
        )
        {
            return null;
        }

        return ResolveProjectileSpawn(
            projectileSetIndex,
            projectileIndex,
            spawnPosition,
            flightDirection
        );
    }

    private Projectile ResolveProjectileSpawn(
        int projectileSetIndex,
        int projectileIndex,
        Vector3 spawnPosition,
        Vector3 flightDirection
    )
    {
        Projectile projectile = projectileSets[projectileSetIndex][projectileIndex];

        projectile.transform.position = spawnPosition;
        projectile.transform.forward = flightDirection;

        projectile.ActivateProjectile();

        projectileIndeces[projectileSetIndex]++;

        if (projectileIndeces[projectileSetIndex] >= projectileSets[projectileSetIndex].Count)
        {
            projectileIndeces[projectileSetIndex] = 0;
        }

        return projectile;
    }

    public void SpawnProjectilePattern(
        GameObject projectileType,
        ProjectilePattern pattern,
        float patternStartDelay,
        float patternEndDelay,
        Transform spawnTransform,
        Action OnPatternFinished = null
    )
    {
        if (
            !CheckAreProjectilesInitialised(
                projectileType,
                out int projectileIndex,
                out int projectileSetIndex
            )
        )
        {
            return;
        }

        StartCoroutine(
            ProjectilePatternSpawner(
                pattern,
                patternStartDelay,
                patternEndDelay,
                projectileSetIndex,
                spawnTransform,
                OnPatternFinished
            )
        );
    }

    private IEnumerator ProjectilePatternSpawner(
        ProjectilePattern pattern,
        float patternStartDelay,
        float patternEndDelay,
        int projectileSet,
        Transform spawnTransform,
        Action OnPatternFinished
    )
    {
        yield return new WaitForSeconds(patternStartDelay);

        ProjectilePattern activePattern = pattern;

        for (int j = 0; j < pattern.patternWaves; j++)
        {
            Vector3 positionOffset = activePattern.startingPosition;
            float angleOffset = activePattern.startingAngle;

            for (int i = 0; i < activePattern.projectileNumber; i++)
            {
                Vector3 flightDirection = spawnTransform.forward;
                flightDirection = Quaternion.Euler(0f, angleOffset, 0f) * flightDirection;

                int projectileIndex = projectileIndeces[projectileSet];

                ResolveProjectileSpawn(
                    projectileSet,
                    projectileIndex,
                    spawnTransform.position + positionOffset,
                    flightDirection
                );

                positionOffset += activePattern.positionChangePerSpawn;
                angleOffset += activePattern.angleChangePerSpawn;

                yield return new WaitForSeconds(activePattern.timeBetweenSpawns);
            }

            float waveDelay = pattern.timeBetweenWaves;

            if (pattern.additionalWaves.Count > 0)
            {
                int waveIndex = (int)AdditionalMath.Modulus(j, pattern.additionalWaves.Count);

                activePattern = pattern.additionalWaves[waveIndex];

                if (pattern.additionalWaveDelay.Count > waveIndex)
                {
                    waveDelay = pattern.additionalWaveDelay[waveIndex];
                }
            }

            yield return new WaitForSeconds(waveDelay);
        }

        yield return new WaitForSeconds(patternEndDelay);

        if (OnPatternFinished != null)
        {
            OnPatternFinished();
        }
    }

    private void DeactivateAllProjectiles()
    {
        StopAllCoroutines();

        foreach (List<Projectile> set in projectileSets)
        {
            foreach (Projectile projectile in set)
            {
                projectile.DeactivateProjectile();
            }
        }
    }
}

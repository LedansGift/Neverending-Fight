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
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= DeactivateAllProjectiles;
    }

    private bool CheckAreProjectilesInitialised(
        GameObject projectileType,
        out int projectileIndex,
        out int projectileSetIndex
    )
    {
        if (!projectileMapper.TryGetValue(projectileType, out int outProjectileSetIndex))
        {
            Debug.Log("Projectile type has not been instantiated");
            projectileIndex = -1;
            projectileSetIndex = outProjectileSetIndex;
            return false;
        }

        projectileIndex = projectileIndeces[outProjectileSetIndex];
        projectileSetIndex = outProjectileSetIndex;
        return true;
    }

    public void InitialiseProjectileSet(GameObject newProjectilePrefab, int amount)
    {
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

    public void SpawnProjectile(
        GameObject projectileType,
        Vector3 spawnPosition,
        Vector3 flightDirection,
        int projectileDamage = -1,
        float projectileSpeed = -1f,
        Transform homingTarget = null,
        float homingStrength = 0f
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

        ResolveProjectileSpawn(
            projectileSetIndex,
            projectileIndex,
            spawnPosition,
            flightDirection,
            projectileDamage,
            projectileSpeed,
            homingTarget,
            homingStrength
        );
    }

    private void ResolveProjectileSpawn(
        int projectileSetIndex,
        int projectileIndex,
        Vector3 spawnPosition,
        Vector3 flightDirection,
        int projectileDamage,
        float projectileSpeed,
        Transform homingTarget,
        float homingStrength
    )
    {
        Projectile projectile = projectileSets[projectileSetIndex][projectileIndex];

        projectile.transform.position = spawnPosition;
        projectile.transform.forward = flightDirection;

        projectile.ActivateProjectile(
            projectileDamage,
            projectileSpeed,
            homingTarget,
            homingStrength
        );

        projectileIndeces[projectileSetIndex]++;

        if (projectileIndeces[projectileSetIndex] >= projectileSets[projectileSetIndex].Count)
        {
            projectileIndeces[projectileSetIndex] = 0;
        }
    }

    public void SpawnProjectilePattern(
        GameObject projectileType,
        ProjectilePattern pattern,
        Transform spawnTransform,
        int projectileDamage = -1,
        float projectileSpeed = -1f,
        Transform homingTarget = null,
        float homingStrength = 0f
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
                projectileSetIndex,
                spawnTransform,
                projectileDamage,
                projectileSpeed,
                homingTarget,
                homingStrength
            )
        );
    }

    private IEnumerator ProjectilePatternSpawner(
        ProjectilePattern pattern,
        int projectileSet,
        Transform spawnTransform,
        int projectileDamage,
        float projectileSpeed,
        Transform homingTarget = null,
        float homingStrength = 0f
    )
    {
        yield return new WaitForSeconds(pattern.initialSpawnDelay);

        Vector3 positionOffset = pattern.startingPosition;
        float angleOffset = pattern.startingAngle;

        for (int i = 0; i < pattern.projectileNumber; i++)
        {
            Vector3 flightDirection = spawnTransform.forward;
            flightDirection = Quaternion.Euler(0f, angleOffset, 0f) * flightDirection;

            int projectileIndex = projectileIndeces[projectileSet];

            ResolveProjectileSpawn(
                projectileSet,
                projectileIndex,
                spawnTransform.position + positionOffset,
                flightDirection,
                projectileDamage,
                projectileSpeed,
                homingTarget,
                homingStrength
            );

            positionOffset += pattern.positionChangePerSpawn;
            angleOffset += pattern.angleChangePerSpawn;

            yield return new WaitForSeconds(pattern.timeBetweenSpawns);
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

using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static Transform managerTransform;
    private static int mapperIndex = 0;
    private static List<int> projectileIndeces;
    private static List<List<Projectile>> projectileSets;
    private static Dictionary<GameObject, int> projectileMapper;

    private void Awake()
    {
        managerTransform = transform;
        mapperIndex = 0;
        projectileIndeces = new List<int>();
        projectileSets = new List<List<Projectile>>();
        projectileMapper = new Dictionary<GameObject, int>();
    }

    public static void InitialiseProjectileSet(GameObject newProjectilePrefab, int amount)
    {
        projectileMapper.Add(newProjectilePrefab, mapperIndex);
        mapperIndex++;

        List<Projectile> newProjectileSet = new List<Projectile>();

        for (int i = 0; i < amount; i++)
        {
            Projectile newProjectile = Instantiate(newProjectilePrefab, managerTransform)
                .GetComponent<Projectile>();

            newProjectileSet.Add(newProjectile);
        }

        projectileSets.Add(newProjectileSet);
        projectileIndeces.Add(0);
    }

    public static void SpawnProjectile(
        GameObject projectileType,
        Vector3 spawnPosition,
        Vector3 flightDirection,
        int projectileDamage,
        float projectileSpeed
    )
    {
        if (!projectileMapper.TryGetValue(projectileType, out int projectileSetIndex))
        {
            Debug.Log("Projectile type has not been instantiated");
            return;
        }

        int projectileIndex = projectileIndeces[projectileSetIndex];

        Projectile projectile = projectileSets[projectileSetIndex][projectileIndex];

        projectile.transform.position = spawnPosition;
        projectile.transform.forward = flightDirection;

        projectile.ActivateProjectile(projectileDamage, projectileSpeed);

        projectileIndeces[projectileSetIndex]++;

        if (projectileIndeces[projectileSetIndex] >= projectileSets[projectileSetIndex].Count)
        {
            projectileIndeces[projectileSetIndex] = 0;
        }
    }
}

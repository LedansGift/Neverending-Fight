using System.Collections;
using UnityEngine;

public class ProjectileInitialiser : MonoBehaviour
{
    private const int PROJECTILE_SPAWNS_PER_TICK = 5;
    private const float TIME_PER_SPAWN_TICK = 0.01f;

    private ProjectileManager projectileManager;

    private void Awake()
    {
        projectileManager = GetComponent<ProjectileManager>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator InitialiseProjectilesOverTime(
        GameObject newProjectilePrefab,
        int projectileSetIndex,
        int amount
    )
    {
        int spawnCounter = 0;

        for (int i = 0; i < amount; i++)
        {
            Projectile newProjectile = Instantiate(newProjectilePrefab, transform)
                .GetComponent<Projectile>();

            projectileManager.AddToProjectileSet(newProjectile, projectileSetIndex);

            spawnCounter++;

            if (spawnCounter >= PROJECTILE_SPAWNS_PER_TICK)
            {
                spawnCounter = 0;
                yield return new WaitForSeconds(TIME_PER_SPAWN_TICK);
            }
        }
    }

    public void StartInitialiseProjectilesOverTime(
        GameObject newProjectilePrefab,
        int projectileSetIndex,
        int amount
    )
    {
        StartCoroutine(
            InitialiseProjectilesOverTime(newProjectilePrefab, projectileSetIndex, amount)
        );
    }
}

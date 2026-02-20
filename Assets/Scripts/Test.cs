using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject projectile;
    public ProjectilePattern pattern;

    private void Start()
    {
        ProjectileManager.Instance.InitialiseProjectileSet(projectile, 10);

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(5f);

        ProjectileManager.Instance.SpawnProjectilePattern(projectile, pattern, transform);
        StartCoroutine(Shoot());
    }
}

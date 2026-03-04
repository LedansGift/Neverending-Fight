using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    // public GameObject projectile;
    // public ProjectilePattern pattern;

    private void Start()
    {
        //ProjectileManager.Instance.InitialiseProjectileSet(projectile, 10);

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);

        DamageZoneManager.Instance.SpawnDamageZone(
            transform.position + new Vector3(-5f, 0f, 0f),
            transform.forward,
            DamageZoneType.circle,
            new Vector2(3f, 1f),
            3f
        );

        yield return new WaitForSeconds(1f);

        DamageZoneManager.Instance.SpawnDamageZone(
            transform.position,
            transform.forward,
            DamageZoneType.box,
            new Vector2(3f, 5f),
            3f
        );

        yield return new WaitForSeconds(1f);

        DamageZoneManager.Instance.SpawnDamageZone(
            transform.position + new Vector3(5f, 0f, 0f),
            transform.right,
            DamageZoneType.circle,
            new Vector2(10f, 0.15f),
            5f
        );
    }
}

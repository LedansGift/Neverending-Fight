using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    // public GameObject projectile;
    // public ProjectilePattern pattern;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform bossTransform;

    private void Update()
    {
        // Vector3 playerDirection = (bossTransform.position - playerTransform.position).normalized;
        // float dot = Vector3.Dot(bossTransform.forward, playerDirection);
        // float dotDivision =
        //     dot
        //     / Mathf.Clamp(bossTransform.forward.magnitude * playerDirection.magnitude, 0.0001f, 1f);
        // float angle = Mathf.Acos(dotDivision) / Mathf.PI;
        // Debug.Log(angle);
    }

    private void Start()
    {
        //ProjectileManager.Instance.InitialiseProjectileSet(projectile, 10);

        // StartCoroutine(Shoot());

        //BossCastBarUI.InitiateCastEvent(new CastInfo("X-Slash", 5f));
    }

    // private IEnumerator Shoot()
    // {
    //     yield return new WaitForSeconds(1f);

    //     DamageZoneManager.Instance.SpawnDamageZone(
    //         transform.position + new Vector3(-5f, 0f, 0f),
    //         transform.forward,
    //         DamageZoneType.circle,
    //         new Vector2(3f, 1f),
    //         3f
    //     );

    //     yield return new WaitForSeconds(1f);

    //     DamageZoneManager.Instance.SpawnDamageZone(
    //         transform.position,
    //         transform.forward,
    //         DamageZoneType.box,
    //         new Vector2(3f, 5f),
    //         3f
    //     );

    //     yield return new WaitForSeconds(1f);

    //     DamageZoneManager.Instance.SpawnDamageZone(
    //         transform.position + new Vector3(5f, 0f, 0f),
    //         transform.right,
    //         DamageZoneType.circle,
    //         new Vector2(10f, 0.15f),
    //         5f
    //     );
    // }
}

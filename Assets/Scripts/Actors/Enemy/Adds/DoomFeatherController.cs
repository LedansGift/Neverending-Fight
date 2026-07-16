using System;
using System.Collections;
using UnityEngine;

public class DoomFeatherController : MonoBehaviour
{
    [SerializeField]
    private float damageZoneAppearTime = 5f;

    [SerializeField]
    private float damageTime = 5.995f;
    private float featherStartDistance = 30f;

    private Projectile projectile;

    [SerializeField]
    private Rigidbody featherRB;

    [SerializeField]
    private MeleeAttack featherAttack;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleDoomFeather;
    }

    private void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleDoomFeather;

        StopAllCoroutines();
    }

    private IEnumerator DelayedDamageZoneSpawn()
    {
        featherRB.position = new Vector3(
            transform.position.x,
            featherStartDistance,
            transform.position.z
        );

        yield return new WaitForSeconds(damageZoneAppearTime);
        AttackTelegraphManager.Instance.StartAttack(transform, featherAttack);

        yield return new WaitForSeconds(damageTime - damageZoneAppearTime);
        PerformAttack();
    }

    private void PerformAttack()
    {
        AttackHitResolver.HitCircleArea(
            transform,
            featherAttack,
            LayerMaskManager.GetAttackLayerMask()
        );
    }

    private void ToggleDoomFeather(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(DelayedDamageZoneSpawn());
        }
        else
        {
            StopAllCoroutines();
        }
    }
}

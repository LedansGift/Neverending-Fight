using System.Collections;
using UnityEngine;

public class LaserBookController : MonoBehaviour
{
    [SerializeField]
    private float spawnTime = 2f;

    [SerializeField]
    private MeleeAttack laserAttack;

    private Projectile projectile;
    private DamageZone laserDamageZone;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleBookActive;
    }

    private void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleBookActive;
    }

    private IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(spawnTime);

        laserDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, laserAttack);

        yield return new WaitForSeconds(laserAttack.zoneWarningTime);

        AttackHitResolver.HitBoxArea(transform, laserAttack, LayerMaskManager.GetAttackLayerMask());
    }

    private void ToggleBookActive(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(StartAttack());
        }
        else
        {
            if (laserDamageZone)
            {
                laserDamageZone.DeactivateZone();
            }

            StopAllCoroutines();
        }
    }
}

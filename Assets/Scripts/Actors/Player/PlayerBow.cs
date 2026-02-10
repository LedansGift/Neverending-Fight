using System.Collections;
using UnityEngine;

public class PlayerBow : PlayerWeapon
{
    private bool inputHeld = false;
    private bool chargeBow = false;

    private float chargeAmount = 0f;
    private float lowSpeedModifier = 0.75f;
    private float highSpeedModifier = 1.25f;

    private Coroutine cooldownCoroutine;

    [SerializeField]
    private Transform bowShootTransform;

    [SerializeField]
    private GameObject bowProjectilePrefab;

    private void Start()
    {
        //Initialise bow projectiles in projectile manager
        ProjectileManager.InitialiseProjectileSet(bowProjectilePrefab, 5);
    }

    public override void WeaponAttackStart()
    {
        inputHeld = true;

        if (isBusy)
        {
            return;
        }

        weaponAnimator.SetTrigger("attack");

        chargeBow = true;
        canSwap = false;

        chargeAmount = 0f;
    }

    public override void WeaponAttackEnd()
    {
        inputHeld = false;

        if (!chargeBow)
        {
            return;
        }

        weaponAnimator.SetTrigger("loose");

        chargeBow = false;
        canSwap = true;

        ShootArrow();

        cooldownCoroutine = StartCoroutine(ShootCooldown());
    }

    public override void WeaponSpecial()
    {
        //Physics.OverlapBox(transform.position,);
    }

    private void Update()
    {
        if (chargeBow)
        {
            chargeAmount = Mathf.Min(chargeAmount + Time.deltaTime, playerStats.GetBowChargeTime());
        }
    }

    private IEnumerator ShootCooldown()
    {
        isBusy = true;
        yield return new WaitForSeconds(playerStats.GetBowShootCooldown());
        isBusy = false;

        if (inputHeld)
        {
            WeaponAttackStart();
        }
    }

    private void ShootArrow()
    {
        int bowDamage;
        float projectileSpeed;

        if (
            (chargeAmount > playerStats.GetBowPerfectChargeTime())
            && (
                chargeAmount
                <= (
                    playerStats.GetBowPerfectChargeTime()
                    + playerStats.GetBowPerfectChargeDuration()
                )
            )
        )
        {
            bowDamage = playerStats.GetPerfectBowDamage();
            projectileSpeed = playerStats.GetBowProjectileSpeed() * highSpeedModifier;
        }
        else
        {
            bowDamage = Mathf.RoundToInt(chargeAmount * (float)playerStats.GetBowDamage());
            float speedModifier = Mathf.Lerp(lowSpeedModifier, 1f, chargeAmount);
            projectileSpeed = playerStats.GetBowProjectileSpeed() * speedModifier;
        }

        //Debug.Log(projectileSpeed);

        ProjectileManager.SpawnProjectile(
            bowProjectilePrefab,
            bowShootTransform.transform.position,
            bowShootTransform.forward,
            bowDamage,
            projectileSpeed
        );
    }

    public override void StowWeapon()
    {
        base.StowWeapon();

        inputHeld = false;

        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
    }
}

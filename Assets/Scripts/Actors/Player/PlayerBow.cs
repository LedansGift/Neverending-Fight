using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : PlayerWeapon
{
    private bool inputHeld = false;
    private bool chargeBow = false;

    private bool specialReady = false;

    private float chargeAmount = 0f;
    private float lowSpeedModifier = 0.75f;
    private float highSpeedModifier = 1.25f;

    private float specialCharge = 0f;

    private Vector3 specialHitbox = new Vector3(2f, 3f, 200f);

    private Coroutine cooldownCoroutine;

    [SerializeField]
    private Transform bowShootTransform;

    [SerializeField]
    private Transform bowSpecialVisual;

    [SerializeField]
    private GameObject bowProjectilePrefab;

    private void Start()
    {
        bowSpecialVisual.SetParent(null);

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
        if (!specialReady)
        {
            return;
        }

        StartCoroutine(SpecialShoot());

        specialCharge = 0f;
        specialReady = false;
    }

    private IEnumerator SpecialShoot()
    {
        isBusy = true;
        canSwap = false;
        playerMovement.SetWeaponModifier(playerStats.GetBowSpecialMovementModifier());

        bowSpecialVisual.gameObject.SetActive(true);

        yield return new WaitForSeconds(playerStats.GetBowSpecialShootTime());

        bowSpecialVisual.gameObject.SetActive(false);

        Health[] hitTargets = GetSpecialShootTargets();

        foreach (Health target in hitTargets)
        {
            target.TakeDamage(playerStats.GetBowSpecialDamage());
        }

        playerMovement.SetWeaponModifier();
        isBusy = false;
        canSwap = true;
    }

    private void Update()
    {
        //Temp
        bowSpecialVisual.position = mouseTarget.position;
        bowSpecialVisual.rotation = transform.rotation;

        if (chargeBow)
        {
            chargeAmount = Mathf.Min(chargeAmount + Time.deltaTime, playerStats.GetBowChargeTime());
        }
    }

    private Health[] GetSpecialShootTargets()
    {
        Collider[] colliders = Physics.OverlapBox(
            mouseTarget.position,
            specialHitbox,
            transform.rotation,
            attackLayerMask
        );

        List<Health> healths = new List<Health>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Health>(out Health hitHealth) && !hitHealth.GetIsPlayer())
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
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

    private void CheckSpecialStatus()
    {
        if (specialReady)
        {
            specialCharge = playerStats.GetBowSpecialChargeThreshold();
            return;
        }

        if (specialCharge >= playerStats.GetBowSpecialChargeThreshold())
        {
            specialCharge = playerStats.GetBowSpecialChargeThreshold();
            specialReady = true;
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
            specialCharge += playerStats.GetBowSpecialPerfectCharge();
        }
        else
        {
            bowDamage = Mathf.RoundToInt(chargeAmount * (float)playerStats.GetBowDamage());
            float speedModifier = Mathf.Lerp(lowSpeedModifier, 1f, chargeAmount);
            projectileSpeed = playerStats.GetBowProjectileSpeed() * speedModifier;
            specialCharge += chargeAmount;
        }

        //Debug.Log(projectileSpeed);

        CheckSpecialStatus();

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

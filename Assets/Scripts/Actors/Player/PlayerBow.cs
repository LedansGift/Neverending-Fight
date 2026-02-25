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

    private const int BOW_WEAPON_INDEX = 1;

    private Vector3 specialHitbox = new Vector3(2f, 3f, 200f);

    private Coroutine cooldownCoroutine;

    [SerializeField]
    private BowShootFX shootFX;

    [SerializeField]
    private Transform bowShootTransform;

    [SerializeField]
    private GameObject bowProjectilePrefab;

    [SerializeField]
    private Renderer shootPointerRenderer;
    private Material shootPointerShader;

    [SerializeField]
    private GameObject shootPointerPerfect;

    private void Start()
    {
        shootPointerShader = shootPointerRenderer.material;
        shootPointerShader.SetFloat("_YReveal", 0f);

        ChargeSpecial(999f);

        //Initialise bow projectiles in projectile manager
        ProjectileManager.Instance.InitialiseProjectileSet(bowProjectilePrefab, 5);
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

        shootPointerPerfect.SetActive(true);
        shootFX.StartCharge(
            playerStats.GetBowPerfectChargeTime() + (playerStats.GetBowPerfectChargeDuration() / 2f)
        );
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

        shootPointerPerfect.SetActive(false);
        shootPointerShader.SetFloat("_YReveal", 0f);
        shootFX.FinishCharge();
    }

    public override void WeaponSpecial()
    {
        if (!specialReady)
        {
            return;
        }

        weaponAnimator.SetTrigger("special");

        StartCoroutine(SpecialShoot());

        specialCharge = 0f;
        specialReady = false;

        OnWeaponAbilityCharge?.Invoke(
            this,
            new WeaponAbilityCharge(BOW_WEAPON_INDEX, specialCharge)
        );
    }

    private IEnumerator SpecialShoot()
    {
        isBusy = true;
        canSwap = false;
        playerMovement.SetWeaponModifier(playerStats.GetBowSpecialMovementModifier());

        shootFX.StartSpecialVisual();

        yield return new WaitForSeconds(playerStats.GetBowSpecialShootTime());

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
        if (chargeBow)
        {
            chargeAmount = Mathf.Min(chargeAmount + Time.deltaTime, playerStats.GetBowChargeTime());
            shootPointerShader.SetFloat("_YReveal", chargeAmount);
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

        OnWeaponAbilityCharge?.Invoke(
            this,
            new WeaponAbilityCharge(
                BOW_WEAPON_INDEX,
                specialCharge / playerStats.GetBowSpecialChargeThreshold()
            )
        );
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

            ChargeSpecial(playerStats.GetBowSpecialPerfectCharge());
        }
        else
        {
            bowDamage = Mathf.Max(
                1,
                Mathf.RoundToInt(chargeAmount * (float)playerStats.GetBowDamage())
            );
            float speedModifier = Mathf.Lerp(lowSpeedModifier, 1f, chargeAmount);
            projectileSpeed = playerStats.GetBowProjectileSpeed() * speedModifier;
            ChargeSpecial(chargeAmount);
        }

        //Debug.Log(projectileSpeed);

        ProjectileManager.Instance.SpawnProjectile(
            bowProjectilePrefab,
            bowShootTransform.transform.position,
            bowShootTransform.forward,
            bowDamage,
            projectileSpeed
        );
    }

    private void ChargeSpecial(float chargeAmount)
    {
        specialCharge += chargeAmount;

        CheckSpecialStatus();
    }

    public override void StowWeapon()
    {
        base.StowWeapon();

        inputHeld = false;

        shootFX.ResetFX();

        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
    }

    public override void ResetWeapon()
    {
        chargeBow = false;
        specialReady = false;
        chargeAmount = 0f;

        shootPointerPerfect.SetActive(false);

        shootPointerShader.SetFloat("_YReveal", 0f);

        ChargeSpecial(999f);

        StopAllCoroutines();

        StowWeapon();
    }

    public override int GetWeaponIndex()
    {
        return BOW_WEAPON_INDEX;
    }
}

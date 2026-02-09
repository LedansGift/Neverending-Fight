using UnityEngine;

public class PlayerBow : PlayerWeapon
{
    private bool chargeBow = false;
    private float chargeAmount = 0f;

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
        weaponAnimator.SetTrigger("attack");

        chargeBow = true;
        canSwap = false;

        chargeAmount = 0f;
    }

    public override void WeaponAttackEnd()
    {
        weaponAnimator.SetTrigger("loose");

        chargeBow = false;
        canSwap = true;

        ShootArrow();
    }

    public override void WeaponSpecial() { }

    private void Update()
    {
        if (chargeBow)
        {
            chargeAmount = Mathf.Min(chargeAmount + Time.deltaTime, playerStats.GetBowChargeTime());
        }
    }

    private void ShootArrow()
    {
        int bowDamage = 0;

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
        }
        else
        {
            bowDamage = Mathf.RoundToInt(chargeAmount * (float)playerStats.GetBowDamage());
        }

        //Debug.Log(bowDamage);

        ProjectileManager.SpawnProjectile(
            bowProjectilePrefab,
            bowShootTransform.transform.position,
            bowShootTransform.forward,
            bowDamage,
            10f
        );
    }
}

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 100;

    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 7.5f;

    [SerializeField]
    private float dashTime = 1f;

    [SerializeField]
    private float dashSpeed = 20f;

    [SerializeField]
    private float dashRecharge = 1.5f;

    [Header("Glaive")]
    [SerializeField]
    private int glaiveDamage = 5;

    [SerializeField]
    private float glaiveAttackRange = 5f;

    [SerializeField]
    [Range(0, 1)]
    private float glaiveAttackArc = 0.5f;

    [SerializeField]
    private float glaiveAttackCooldown = 1.25f;

    [SerializeField]
    private float glaiveRhythmStart = 0.5f;

    [SerializeField]
    private float glaiveRhythmDuration = 0.25f;

    [SerializeField]
    private int glaiveSpecialDamage = 15;

    [SerializeField]
    private float glaiveSpecialDamageRange = 10f;

    [SerializeField]
    private float glaiveSpecialJumpTime = 2.5f;

    [SerializeField]
    private float glaiveSpecialSpeedModifier = 0.75f;

    [SerializeField]
    private float glaiveSpecialChargeThreshold = 10f;

    [SerializeField]
    private float glaiveSpecialPerfectCharge = 2.5f;

    [Header("Bow")]
    [SerializeField]
    private int bowDamage = 4;

    [SerializeField]
    private int bowPerfectDamage = 6;

    [SerializeField]
    private float bowProjectileSpeed = 10f;

    [SerializeField]
    private float bowChargeTime = 1f;

    [SerializeField]
    private float bowShootCooldown = 0.5f;

    [SerializeField]
    private float bowPerfectChargeTime = 0.7f;

    [SerializeField]
    private float bowPerfectChargeDuration = 0.15f;

    [SerializeField]
    private int bowSpecialDamage = 10;

    [SerializeField]
    private int bowSpecialShotHitNumber = 4;

    [SerializeField]
    private float bowSpecialShootDelay = 0.5f;

    [SerializeField]
    private float bowSpecialShootTime = 0.5f;

    [SerializeField]
    private float bowSpecialMovementModifier = 0.5f;

    [SerializeField]
    private float bowSpecialChargeThreshold = 10f;

    [SerializeField]
    private float bowSpecialPerfectCharge = 2f;

    [Header("Tome")]
    [SerializeField]
    private int tomeMaxDamage = 15;

    [SerializeField]
    private float tomeMaxRadius = 10f;

    [SerializeField]
    private float tomeMaxMovementModifier = 0.4f;

    [SerializeField]
    private float tomeChargeTime = 2.5f;

    [SerializeField]
    private float tomeAttackCooldown = 0.5f;

    [SerializeField]
    private int tomeSpecialMaxBuff = 10;

    [SerializeField]
    private float tomeSpecialCastDuration = 2.5f;

    [SerializeField]
    private float tomeSpecialBuffDuration = 10f;

    [SerializeField]
    private float tomeSpecialCooldown = 30f;

    private int attackDamageBuff = 0;

    private void Awake()
    {
        attackDamageBuff = 0;
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetDashTime()
    {
        return dashTime;
    }

    public float GetDashSpeedModifier()
    {
        return dashSpeed;
    }

    public float GetDashRechargeTime()
    {
        return dashRecharge;
    }

    public int GetGlaiveDamage()
    {
        return glaiveDamage + attackDamageBuff;
    }

    public float GetGlaiveAttackRange()
    {
        return glaiveAttackRange;
    }

    public float GetGlaiveAttackArc()
    {
        return glaiveAttackArc;
    }

    public float GetGlaiveAttackCooldown()
    {
        return glaiveAttackCooldown;
    }

    public float GetGlaiveRhythmStart()
    {
        return glaiveRhythmStart;
    }

    public float GetGlaiveRhythmDuration()
    {
        return glaiveRhythmDuration;
    }

    public int GetGlaiveSpecialDamage()
    {
        return glaiveSpecialDamage + attackDamageBuff;
    }

    public float GetGlaiveSpecialDamageRange()
    {
        return glaiveSpecialDamageRange;
    }

    public float GetGlaiveSpecialJumpTime()
    {
        return glaiveSpecialJumpTime;
    }

    public float GetGlaiveSpecialMovementModifier()
    {
        return glaiveSpecialSpeedModifier;
    }

    public float GetGlaiveSpecialChargeThreshold()
    {
        return glaiveSpecialChargeThreshold;
    }

    public float GetGlaiveSpecialPerfectCharge()
    {
        return glaiveSpecialPerfectCharge;
    }

    public int GetBowDamage()
    {
        return bowDamage + attackDamageBuff;
    }

    public int GetPerfectBowDamage()
    {
        return bowPerfectDamage;
    }

    public float GetBowProjectileSpeed()
    {
        return bowProjectileSpeed;
    }

    public float GetBowChargeTime()
    {
        return bowChargeTime;
    }

    public float GetBowShootCooldown()
    {
        return bowShootCooldown;
    }

    public float GetBowPerfectChargeTime()
    {
        return bowPerfectChargeTime;
    }

    public float GetBowPerfectChargeDuration()
    {
        return bowPerfectChargeDuration;
    }

    public int GetBowSpecialDamage()
    {
        return bowSpecialDamage + attackDamageBuff;
    }

    public int GetBowSpecialShotHitNumber()
    {
        return bowSpecialShotHitNumber;
    }

    public float GetBowSpecialShootDelay()
    {
        return bowSpecialShootDelay;
    }

    public float GetBowSpecialShootTime()
    {
        return bowSpecialShootTime;
    }

    public float GetBowSpecialMovementModifier()
    {
        return bowSpecialMovementModifier;
    }

    public float GetBowSpecialChargeThreshold()
    {
        return bowSpecialChargeThreshold;
    }

    public float GetBowSpecialPerfectCharge()
    {
        return bowSpecialPerfectCharge;
    }

    public int GetTomeMaxDamage()
    {
        return tomeMaxDamage + attackDamageBuff;
    }

    public float GetTomeMaxRadius()
    {
        return tomeMaxRadius;
    }

    public float GetTomeMaxMovementModifier()
    {
        return tomeMaxMovementModifier;
    }

    public float GetTomeChargeTime()
    {
        return tomeChargeTime;
    }

    public float GetTomeAttackCooldown()
    {
        return tomeAttackCooldown;
    }

    public int GetTomeSpecialMaxBuff()
    {
        return tomeSpecialMaxBuff;
    }

    public float GetTomeSpecialCastDuration()
    {
        return tomeSpecialCastDuration;
    }

    public float GetTomeSpecialBuffDuration()
    {
        return tomeSpecialBuffDuration;
    }

    public float GetTomeSpecialCooldown()
    {
        return tomeSpecialCooldown;
    }

    public void SetAttackBuff(int attackBuff)
    {
        attackDamageBuff = attackBuff;
    }
}

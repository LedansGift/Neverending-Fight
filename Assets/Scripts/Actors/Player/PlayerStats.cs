using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 7.5f;

    [SerializeField]
    private float dashTime = 1f;

    [SerializeField]
    private float dashSpeed = 20f;

    [SerializeField]
    private float dashRecharge = 1.5f;

    [Header("Weapons")]
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
    private int bowDamage = 4;

    [SerializeField]
    private int bowPerfectDamage = 6;

    [SerializeField]
    private float bowChargeTime = 1f;

    [SerializeField]
    private float bowPerfectChargeTime = 0.7f;

    [SerializeField]
    private float bowPerfectChargeDuration = 0.15f;

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
        return glaiveDamage;
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

    public int GetBowDamage()
    {
        return bowDamage;
    }

    public int GetPerfectBowDamage()
    {
        return bowPerfectDamage;
    }

    public float GetBowChargeTime()
    {
        return bowChargeTime;
    }

    public float GetBowPerfectChargeTime()
    {
        return bowPerfectChargeTime;
    }

    public float GetBowPerfectChargeDuration()
    {
        return bowPerfectChargeDuration;
    }
}

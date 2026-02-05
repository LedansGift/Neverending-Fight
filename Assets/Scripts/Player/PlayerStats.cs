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
    private float glaiveDamage = 5f;

    [SerializeField]
    private float glaiveAttackRange = 5f;

    [SerializeField]
    private float glaiveAttackCooldown = 1.25f;

    [SerializeField]
    private float glaiveRhythmStart = 0.5f;

    [SerializeField]
    private float glaiveRhythmDuration = 0.25f;

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

    public float GetGlaiveDamage()
    {
        return glaiveDamage;
    }

    public float GetGlaiveAttackRange()
    {
        return glaiveAttackRange;
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
}

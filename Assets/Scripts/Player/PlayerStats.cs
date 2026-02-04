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
}

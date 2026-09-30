using System;
using UnityEngine;

public class DifferentialShield : MonoBehaviour
{
    private bool shieldActive = false;
    private const int SHIELDED_DAMAGE = 1;
    private const int SHIELD_DAMAGE_THRESHOLD = 8;

    [SerializeField]
    private GameObject shieldVisual;

    public static EventHandler<int> OnShieldDamageResolved;

    private void Awake()
    {
        ToggleShield(false);
    }

    public void PulseShield()
    {
        //play pulse effect
    }

    public bool GetIsShieldActive()
    {
        return shieldActive;
    }

    public int ResolveDamage(int damageIn)
    {
        if (shieldActive && (damageIn < SHIELD_DAMAGE_THRESHOLD))
        {
            PulseShield();

            return SHIELDED_DAMAGE;
        }

        OnShieldDamageResolved?.Invoke(this, damageIn);

        return damageIn;
    }

    public void ToggleShield(bool toggle)
    {
        shieldVisual.SetActive(toggle);
        shieldActive = toggle;
    }
}

using UnityEngine;

public class TomeAbsorber : Health
{
    private int absorbedDamage;

    [SerializeField]
    private Collider absorberCollider;

    private void Awake()
    {
        ToggleAbsorber(false);
    }

    public override void TakeDamage(int damageAmount)
    {
        absorbedDamage += damageAmount;
    }

    public void ToggleAbsorber(bool toggle)
    {
        absorberCollider.enabled = toggle;

        if (toggle)
        {
            absorbedDamage = 0;
        }
    }

    public int GetAbsorbedDamage()
    {
        return absorbedDamage;
    }
}

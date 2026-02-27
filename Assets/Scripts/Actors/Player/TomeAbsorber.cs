using UnityEngine;

public class TomeAbsorber : Health
{
    //private int maxDamageMultiplier = 3;
    private int absorbedDamage;
    private int maxAbsorbedDamage;

    [SerializeField]
    private Collider absorberCollider;

    [SerializeField]
    private ParticleSystem absorbParticles;

    [SerializeField]
    AbsorbFlameVisual absorbFlameVisual;

    private void Awake()
    {
        ToggleAbsorber(false);
    }

    public override void TakeDamage(int damageAmount, bool arenaWideDamage = false)
    {
        if (arenaWideDamage)
        {
            return;
        }

        absorbedDamage = Mathf.Min(absorbedDamage + 1, maxAbsorbedDamage);
        absorbFlameVisual.SetVisualSize((float)absorbedDamage / maxAbsorbedDamage);
    }

    public void ToggleAbsorber(bool toggle, int maxAbsorb = 10)
    {
        absorberCollider.enabled = toggle;

        if (toggle)
        {
            absorbedDamage = 0;
            maxAbsorbedDamage = maxAbsorb;
            absorbParticles.gameObject.SetActive(false);
            absorbParticles.gameObject.SetActive(true);
        }
    }

    public int GetAbsorbedDamage(float buffDuration)
    {
        absorbFlameVisual.StartBuffEffect(buffDuration);

        return absorbedDamage;
    }
}

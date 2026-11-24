using UnityEngine;

public class BossMagusHealth : BossHealth
{
    [SerializeField]
    private DifferentialShield shieldVisual;

    public override bool TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (shieldVisual)
        {
            damage = shieldVisual.ResolveDamage(damage);
        }

        return base.TakeDamage(damage, arenaWideDamage);
    }

    public void ToggleShield(bool toggle) => shieldVisual.ToggleShield(toggle);
}

using UnityEngine;

public class BossMagpieHealth : BossMagusHealth
{
    protected override void Awake()
    {
        base.Awake();
        SetUnkillable(true);
    }

    public override void InitialiseHealth(int bossHealth = -1)
    {
        if (isUnkillable)
        {
            isInvincible = false;

            if (bossHealth < 0)
            {
                bossHealth = maxHealth;
            }

            SetMaxHealth(bossHealth);

            return;
        }

        base.InitialiseHealth(bossHealth);
    }
}

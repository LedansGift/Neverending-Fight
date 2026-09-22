using UnityEngine;

public class BossMagusHealth : BossHealth
{
    //differential shield reference

    public override void TakeDamage(int damage, bool arenaWideDamage = false)
    {
        //if differential shield active, pass damage to it to change damage value to 1 if below threshold


        base.TakeDamage(damage, arenaWideDamage);
    }
}

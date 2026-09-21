using System;
using UnityEngine;

public class BossGuardState : BossState
{
    private float damagePool = 0;
    private float guardDamageThreshold = 25;
    private float damagePoolDecayRate = 4f;

    private BossHealth bossHealth;

    //Guard visual that provides feedback for attacks being guarded against + showing meter of guard break

    public BossGuardState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        bossHealth = stateMachine?.GetComponent<BossHealth>();
    }

    public override void Enter()
    {
        damagePool = 0;
        bossHealth?.SetInvincibility(true);
        bossHealth.OnIncomingDamage += PoolIncomingDamage;
    }

    public override void Exit()
    {
        bossHealth?.SetInvincibility(false);
        bossHealth.OnIncomingDamage -= PoolIncomingDamage;

        TryFinishState();
    }

    public override void Tick(float deltaTime)
    {
        if (damagePool > 0)
        {
            damagePool -= damagePoolDecayRate * deltaTime;
        }

        Debug.Log("Damage Pooled: " + damagePool);

        //pooled damage amount continually lowers
        //if surpasses threshold, break guard and go through Exit state
    }

    //on take damage, pool damage

    private void PoolIncomingDamage(object sender, int incomingDamage)
    {
        damagePool += incomingDamage;

        if (damagePool > guardDamageThreshold)
        {
            BreakGuardStance();
        }
    }

    private void BreakGuardStance()
    {
        //cleanup
        stateMachine.SwitchState(null);
    }
}

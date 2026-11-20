using System;
using UnityEngine;

public class BossGuardState : BossState
{
    private float damagePool = 0;
    private float guardDamageThreshold = 25;
    private float damagePoolDecayRate = 4f;

    private const string shieldKey = "GUARD_SHIELD";
    private const string shieldHitKey = "GUARD_HIT";
    private const string guardBreakKey = "GUARD_BREAK";

    private BossHealth bossHealth;

    public static EventHandler<bool> OnToggleGuardState;
    public static EventHandler<float> OnNewGuardValue;

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

        OnToggleGuardState?.Invoke(this, true);
    }

    public override void Exit()
    {
        bossHealth?.SetInvincibility(false);
        bossHealth.OnIncomingDamage -= PoolIncomingDamage;

        OnToggleGuardState?.Invoke(this, false);

        TryFinishState();
    }

    public override void Tick(float deltaTime)
    {
        if (damagePool > 0)
        {
            //damagePool -= damagePoolDecayRate * deltaTime;

            SetNewDamagePool(damagePool - (damagePoolDecayRate * deltaTime));
        }

        Debug.Log("Damage Pooled: " + damagePool);

        //pooled damage amount continually lowers
        //if surpasses threshold, break guard and go through Exit state
    }

    private void SetNewDamagePool(float newDamagePool)
    {
        damagePool = newDamagePool;

        float damagePoolRatio = Mathf.Clamp01(damagePool / guardDamageThreshold);
        OnNewGuardValue?.Invoke(this, damagePoolRatio);
    }

    //on take damage, pool damage

    private void PoolIncomingDamage(object sender, int incomingDamage)
    {
        //damagePool += incomingDamage;

        SetNewDamagePool(damagePool + incomingDamage);

        VFXManager.Instance.TryPlayVFX(shieldHitKey, bossStateMachine.transform);

        if (damagePool > guardDamageThreshold)
        {
            BreakGuardStance();
        }
    }

    private void BreakGuardStance()
    {
        //cleanup

        VFXManager.Instance.StopVFX(shieldKey);
        VFXManager.Instance.TryPlayVFX(guardBreakKey, bossStateMachine.transform);

        stateMachine.SwitchState(null);
    }
}

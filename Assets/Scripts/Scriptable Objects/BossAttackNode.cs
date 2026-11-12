using System;
using UnityEngine;

[Serializable]
public struct FXHolder
{
    public string fxKey;
    public float fxDelay;
}

public abstract class BossAttackNode : BossNode
{
    [SerializeField]
    protected bool failableAttack = true;

    [SerializeField]
    protected FXHolder[] attackFX;

    [SerializeField]
    private BossAttackNode empoweredAttack;
    public static EventHandler OnAttackFailCheck;

    protected void PlayFX(Transform attackerTransform)
    {
        foreach (FXHolder fx in attackFX)
        {
            VFXManager.Instance.TryPlayVFX(fx.fxKey, attackerTransform.transform, fx.fxDelay);
        }
    }

    public bool TryGetEmpoweredAttack(out BossAttackNode empoweredAttack)
    {
        empoweredAttack = null;
        if (this.empoweredAttack == null)
        {
            return false;
        }

        empoweredAttack = this.empoweredAttack;
        return true;
    }

    public override bool GetIsAttackNode()
    {
        return true;
    }
}

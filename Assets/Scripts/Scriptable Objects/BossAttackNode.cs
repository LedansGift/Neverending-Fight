using System;
using UnityEngine;

public abstract class BossAttackNode : BossNode
{
    [SerializeField]
    protected bool failableAttack = true;

    [SerializeField]
    private BossAttackNode empoweredAttack;
    public static EventHandler OnAttackFailCheck;

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

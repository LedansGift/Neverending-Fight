using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    fileName = "New Boss Interrupt Phase",
    menuName = "Boss Phase/Interrupt Phase",
    order = 3
)]
public class BossInterruptPhase : ScriptableObject
{
    [SerializeField]
    protected BossNode[] bossAttackPattern;

    public BossNode[] GetAttackPattern()
    {
        return bossAttackPattern;
    }
}

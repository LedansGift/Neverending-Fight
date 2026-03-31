using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Boss Phase", menuName = "Boss Phase", order = 0)]
public class BossPhase : ScriptableObject
{
    [SerializeField]
    private BossAttackNode[] bossAttackPattern;

    public BossAttackNode[] GetAttackPattern()
    {
        return bossAttackPattern;
    }
}

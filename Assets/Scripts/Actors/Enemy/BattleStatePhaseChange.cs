using System;
using UnityEngine;

[Serializable]
public class BattleStatePhaseChange
{
    private BattleState battleState;

    [SerializeField]
    private BattleState battleStatePrefab;

    public void InitialiseBattleState(Transform spawnTransform)
    {
        if (!battleStatePrefab)
        {
            return;
        }

        if (battleState)
        {
            battleState.ResetBattleState();
        }
        else
        {
            battleState = GameObject.Instantiate(battleStatePrefab, spawnTransform);
            battleState.ActivateListeners();
        }
    }

    public void DeactivateBattleState()
    {
        if (battleState)
        {
            battleState.DeactivateListeners();
        }
    }

    public void ResetBattleState()
    {
        if (battleState)
        {
            battleState.ResetBattleState();
        }
    }

    public bool ResolveBattleState()
    {
        if (!battleState)
        {
            Debug.Log("No battle state created");
            return false;
        }

        return battleState.ResolveBattleState();
    }

    public bool GetBattleState()
    {
        return battleStatePrefab;
    }

    public BossPhase GetNewPhase()
    {
        return battleState.GetNewPhase();
    }
}

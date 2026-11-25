using UnityEngine;

public enum MagusUniqueAttacks
{
    DiffShieldOn,
    DiffShieldOff,
    ElementalCardinalsSingle,
    ElementalCardinalsDouble,
    Godslayer
}

public class MagusStateDictionary : StateDictionary
{
    protected override void SetupDictionary()
    {
        stateDictionary.Add(
            (int)MagusUniqueAttacks.DiffShieldOff,
            new BossDiffShieldState(stateMachine as BossStateMachine, false)
        );
        stateDictionary.Add(
            (int)MagusUniqueAttacks.DiffShieldOn,
            new BossDiffShieldState(stateMachine as BossStateMachine, true)
        );
        stateDictionary.Add(
            (int)MagusUniqueAttacks.ElementalCardinalsSingle,
            new BossElementalCardinalsState(stateMachine as BossStateMachine, 0)
        );
        stateDictionary.Add(
            (int)MagusUniqueAttacks.ElementalCardinalsDouble,
            new BossElementalCardinalsState(stateMachine as BossStateMachine, 1)
        );
        stateDictionary.Add(
            (int)MagusUniqueAttacks.Godslayer,
            new BossGodslayerState(stateMachine as BossStateMachine)
        );
    }
}

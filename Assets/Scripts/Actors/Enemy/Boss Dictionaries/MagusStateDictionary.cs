using UnityEngine;

public enum MagusUniqueAttacks
{
    DiffShieldOn,
    DiffShieldOff,
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
    }
}

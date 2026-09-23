using UnityEngine;

public enum MagpieUniqueAttacks
{
    XSlash,
    InescapablePinions,
    Whirlwind,
    DramaticLunge,
    FellWingV1,
    FellWingV2,
    FellWingV3,
    GuardStance,
    DiffShieldOn,
    DiffShieldOff
}

public class MagpieStateDictionary : StateDictionary
{
    protected override void SetupDictionary()
    {
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.XSlash,
            new BossStateTest1(stateMachine as BossStateMachine)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.InescapablePinions,
            new BossInescapablePinionsState(stateMachine as BossStateMachine)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.Whirlwind,
            new BossWhirlwindState(stateMachine as BossStateMachine)
        );
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.DramaticLunge,
            new BossDramaticLungeState(stateMachine as BossStateMachine)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.FellWingV1,
            new BossFellWingState(stateMachine as BossStateMachine, 0)
        );
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.FellWingV2,
            new BossFellWingState(stateMachine as BossStateMachine, 1)
        );
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.FellWingV3,
            new BossFellWingState(stateMachine as BossStateMachine, 2)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.DiffShieldOff,
            new BossDiffShieldState(stateMachine as BossStateMachine, false)
        );
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.DiffShieldOn,
            new BossDiffShieldState(stateMachine as BossStateMachine, true)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.GuardStance,
            new BossGuardState(stateMachine as BossStateMachine)
        );
    }
}

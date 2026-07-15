using UnityEngine;

public enum MagpieUniqueAttacks
{
    XSlash,
    InescapablePinions,
    Whirlwind,
    DramaticLunge,
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
            new BossStateTest2(stateMachine as BossStateMachine)
        );

        stateDictionary.Add(
            (int)MagpieUniqueAttacks.Whirlwind,
            new BossWhirlwindState(stateMachine as BossStateMachine)
        );
        stateDictionary.Add(
            (int)MagpieUniqueAttacks.DramaticLunge,
            new BossDramaticLungeState(stateMachine as BossStateMachine)
        );
    }
}

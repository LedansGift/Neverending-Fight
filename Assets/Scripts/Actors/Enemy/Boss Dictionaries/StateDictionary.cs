using System.Collections.Generic;
using UnityEngine;

public abstract class StateDictionary : MonoBehaviour
{
    protected Dictionary<int, State> stateDictionary = new Dictionary<int, State>();

    [SerializeField]
    protected StateMachine stateMachine;

    private void Awake()
    {
        SetupDictionary();
    }

    protected abstract void SetupDictionary();

    public bool TryGetState(int stateEnum, out State state)
    {
        bool gotState = stateDictionary.TryGetValue(stateEnum, out State storedState);
        state = storedState;
        return gotState;
    }

    public StateMachine GetStateMachine()
    {
        return stateMachine;
    }
}

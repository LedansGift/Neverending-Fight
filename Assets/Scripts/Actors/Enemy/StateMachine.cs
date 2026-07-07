using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        // if (currentState?.GetType() == newState.GetType())
        // {
        //     return;
        // }


        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    protected State GetCurrentState()
    {
        return currentState;
    }
}

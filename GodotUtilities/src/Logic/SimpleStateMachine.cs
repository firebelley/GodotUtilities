using System.Collections.Generic;

namespace GodotUtilities.Logic;

public partial class SimpleStateMachine : RefCounted
{
    public delegate void State();

    private class StateFlow(State enterState = null, State leaveState = null)
    {
        public State EnterState { get; } = enterState;
        public State LeaveState { get; } = leaveState;
    }

    private State currentState;
    private readonly Dictionary<State, StateFlow> states = [];

    public void AddState(State enterState, State leaveState = null)
    {
        states[enterState] = new StateFlow(enterState, leaveState);
    }

    public void ChangeState(State toState)
    {
        if (!states.TryGetValue(toState, out var stateFlow)) return;

        Callable.From(() => SetState(stateFlow)).CallDeferred();
    }

    public void SetInitialState(State state)
    {
        if (states.TryGetValue(state, out var stateFlow))
        {
            SetState(stateFlow);
        }
    }

    public State GetCurrentState() => currentState;

    private void SetState(StateFlow stateFlow)
    {
        if (currentState != null && states.TryGetValue(currentState, out var currentFlow))
        {
            currentFlow.LeaveState?.Invoke();
        }

        currentState = stateFlow.EnterState;
        stateFlow.EnterState?.Invoke();
    }
}

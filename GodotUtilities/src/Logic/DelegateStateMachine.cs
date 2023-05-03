using System.Collections.Generic;
using Godot;

namespace GodotUtilities.Logic;

public class DelegateStateMachine : RefCounted
{
    public delegate void State();

    private State currentState;

    private readonly Dictionary<State, StateFlows> states = new();

    public void AddStates(State normal, State enterState = null, State leaveState = null)
    {
        var stateFlows = new StateFlows(normal, enterState, leaveState);
        states[normal] = stateFlows;
    }

    public void ChangeState(State toStateDelegate)
    {
        states.TryGetValue(toStateDelegate, out var stateDelegates);
        Callable.From(() => SetState(stateDelegates)).CallDeferred();
    }

    public void SetInitialState(State stateDelegate)
    {
        states.TryGetValue(stateDelegate, out var stateFlows);
        SetState(stateFlows);
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void Update()
    {
        currentState?.Invoke();
    }

    private void SetState(StateFlows stateFlows)
    {
        if (currentState != null)
        {
            states.TryGetValue(currentState, out var currentStateDelegates);
            currentStateDelegates?.LeaveState?.Invoke();
        }
        currentState = stateFlows.Normal;
        stateFlows?.EnterState?.Invoke();
    }

    private class StateFlows
    {
        public State Normal { get; private set; }
        public State EnterState { get; private set; }
        public State LeaveState { get; private set; }

        public StateFlows(State normal, State enterState = null, State leaveState = null)
        {
            Normal = normal;
            EnterState = enterState;
            LeaveState = leaveState;
        }
    }
}

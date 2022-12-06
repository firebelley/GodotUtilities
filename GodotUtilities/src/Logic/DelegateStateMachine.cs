using System.Collections.Generic;
using Godot;

namespace GodotUtilities.Logic
{
    public class DelegateStateMachine : Reference
    {
        public delegate void StateDelegate();

        private StateDelegate currentState;

        private readonly Dictionary<StateDelegate, StateFlows> states = new Dictionary<StateDelegate, StateFlows>();

        public void AddStates(StateDelegate normal, StateDelegate enterState = null, StateDelegate leaveState = null)
        {
            var stateFlows = new StateFlows(normal, enterState, leaveState);
            states[normal] = stateFlows;
        }

        public void ChangeState(StateDelegate toStateDelegate)
        {
            states.TryGetValue(toStateDelegate, out var stateDelegates);
            CallDeferred(nameof(SetState), stateDelegates);
        }

        public void SetInitialState(StateDelegate stateDelegate)
        {
            states.TryGetValue(stateDelegate, out var stateFlows);
            SetState(stateFlows);
        }

        public StateDelegate GetCurrentState()
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

        private class StateFlows : Reference
        {
            public StateDelegate Normal { get; private set; }
            public StateDelegate EnterState { get; private set; }
            public StateDelegate LeaveState { get; private set; }

            public StateFlows(StateDelegate normal, StateDelegate enterState = null, StateDelegate leaveState = null)
            {
                Normal = normal;
                EnterState = enterState;
                LeaveState = leaveState;
            }
        }
    }
}

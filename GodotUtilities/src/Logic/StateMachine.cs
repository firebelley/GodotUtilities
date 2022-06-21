using System.Collections.Generic;
using Godot;

namespace GodotUtilities.Logic
{
    public class StateMachine<T> : Resource
    {
        public delegate void StateDelegate();

        private T currentState;

        private readonly Dictionary<T, StateDelegate> states = new Dictionary<T, StateDelegate>();
        private readonly Dictionary<StateDelegate, T> delegates = new Dictionary<StateDelegate, T>();
        private readonly Dictionary<T, StateDelegate> leaveStates = new Dictionary<T, StateDelegate>();
        private readonly Dictionary<T, StateDelegate> enterStates = new Dictionary<T, StateDelegate>();

        public void AddState(T state, StateDelegate del)
        {
            states.Add(state, del);
            delegates.Add(del, state);
        }

        public void AddLeaveState(T stateToLeave, StateDelegate del)
        {
            leaveStates.Add(stateToLeave, del);
        }

        public void AddEnterState(T enterState, StateDelegate del)
        {
            enterStates.Add(enterState, del);
        }

        public void ChangeState(T state)
        {
            CallDeferred(nameof(SetState), state);
        }

        public void ChangeState(StateDelegate stateDelegate)
        {
            ChangeState(delegates[stateDelegate]);
        }

        public void SetInitialState(T state)
        {
            SetState(state);
        }

        public void SetInitialState(StateDelegate del)
        {
            SetInitialState(delegates[del]);
        }

        public T GetCurrentState()
        {
            return currentState;
        }

        public void Update()
        {
            if (states.ContainsKey(currentState))
            {
                states[currentState]();
            }
        }

        private void SetState(T state)
        {
            if (leaveStates.ContainsKey(currentState))
            {
                leaveStates[currentState]();
            }
            currentState = state;
            if (enterStates.ContainsKey(currentState))
            {
                enterStates[currentState]();
            }
        }
    }
}

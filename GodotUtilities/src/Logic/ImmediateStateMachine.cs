using System.Collections.Generic;

namespace GodotUtilities.Logic
{
    /// <summary>
    /// A state machine designed for states that don't need to be updated every frame.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImmediateStateMachine<T>
    {
        public delegate void StateDelegate();

        public T CurrentState { get; private set; }

        private readonly Dictionary<T, StateDelegate> _states = new Dictionary<T, StateDelegate>();
        private readonly Dictionary<StateDelegate, T> _delegates = new Dictionary<StateDelegate, T>();
        private readonly Dictionary<T, StateDelegate> _leaveStates = new Dictionary<T, StateDelegate>();

        public void AddState(T state, StateDelegate del)
        {
            _states.Add(state, del);
            _delegates.Add(del, state);
        }

        public void AddLeaveState(T stateToLeave, StateDelegate del)
        {
            _leaveStates.Add(stateToLeave, del);
        }

        public void ChangeState(T state)
        {
            if (_leaveStates.ContainsKey(CurrentState))
            {
                _leaveStates[CurrentState]();
            }
            CurrentState = state;
            if (_states.ContainsKey(CurrentState))
            {
                _states[CurrentState]();
            }
        }

        public void ChangeState(StateDelegate stateDelegate)
        {
            ChangeState(_delegates[stateDelegate]);
        }
    }
}
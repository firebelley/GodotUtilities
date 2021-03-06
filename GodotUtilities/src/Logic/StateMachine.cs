﻿using System.Collections.Generic;

namespace GodotUtilities.Logic
{
    public class StateMachine<T>
    {
        public delegate void StateDelegate();

        private T _currentState;
        private T _nextState;
        private bool _forceNew;
        private bool _isStateNew;

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
            _nextState = state;
        }

        public void ChangeState(StateDelegate stateDelegate)
        {
            _nextState = _delegates[stateDelegate];
        }

        public bool IsStateNew()
        {
            return _isStateNew;
        }

        public T GetNextState()
        {
            return _nextState;
        }

        public void SetInitialState(T state)
        {
            _currentState = state;
            _nextState = state;
            _forceNew = true;
        }

        public void SetInitialState(StateDelegate del)
        {
            SetInitialState(_delegates[del]);
        }

        public T GetCurrentState()
        {
            return _currentState;
        }

        public void Update()
        {
            var statesDifferent = !_currentState.Equals(_nextState);
            _isStateNew = statesDifferent || _forceNew;

            if (statesDifferent && _leaveStates.ContainsKey(_currentState))
            {
                _leaveStates[_currentState]();
            }

            _currentState = _nextState;
            _forceNew = false;
            if (_states.ContainsKey(_currentState))
            {
                _states[_currentState]();
            }
        }
    }
}
using System.Collections.Generic;

namespace GodotTools.Logic
{
    public class StateMachine<T>
    {
        public delegate void StateDelegate(bool isStateNew);

        private T _currentState;
        private T _nextState;
        private bool _forceNew = false;

        private Dictionary<T, StateDelegate> _states = new Dictionary<T, StateDelegate>();

        public void AddState(T state, StateDelegate del)
        {
            _states.Add(state, del);
        }

        public void ChangeState(T state)
        {
            _nextState = state;
        }

        public void SetInitialState(T state)
        {
            _currentState = state;
            _nextState = state;
            _forceNew = true;
        }

        public T GetCurrentState()
        {
            return _currentState;
        }

        public void Update()
        {
            var isStateNew = !_currentState.Equals(_nextState) || _forceNew;
            _currentState = _nextState;
            _forceNew = false;
            _states[_currentState](isStateNew);
        }
    }
}
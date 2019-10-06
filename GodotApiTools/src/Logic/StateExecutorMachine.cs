using System.Collections.Generic;
using GodotApiTools.Logic.Interface;

namespace GodotApiTools.Logic
{
    public class StateExecutorMachine
    {
        HashSet<IStateExector> _states = new HashSet<IStateExector>();
        private IStateExector _currentState;
        private IStateExector _nextState;

        public void ChangeState(IStateExector state)
        {
            _nextState = state;
        }

        /// <summary>
        /// Call every frame to update the state machine.
        /// </summary>
        public void Update()
        {
            if (_currentState != _nextState)
            {
                _currentState?.StateLeft();
                _currentState = _nextState;
                _currentState?.StateEntered();
            }
            _currentState?.StateActive();
        }
    }
}
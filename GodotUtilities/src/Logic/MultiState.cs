using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotUtilities.Logic
{
    public class MultiState<T> where T : struct, IConvertible
    {
        private HashSet<T> _states = new HashSet<T>();

        public void AddState(T state)
        {
            _states.Add(state);
        }

        public bool HasState(T state)
        {
            return _states.Contains(state);
        }

        public void RemoveState(T state)
        {
            if (_states.Contains(state))
            {
                _states.Remove(state);
            }
        }

        public void Clear()
        {
            _states.Clear();
        }

        public override bool Equals(object obj)
        {
            if (obj is MultiState<T> ms)
            {
                return ms._states.Count == _states.Count && ms._states.Except(_states).Count() == 0;
            }
            return false;
        }
    }
}
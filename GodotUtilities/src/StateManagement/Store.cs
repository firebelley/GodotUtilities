namespace GodotUtilities.StateManagement
{
    public class Store<T> where T : IState, new()
    {
        public T State { get; private set; } = new T();

        private readonly Reducer<T> _reducer;

        public Store(Reducer<T> reducer)
        {
            _reducer = reducer;
        }

        public void Reset()
        {
            State = new T();
        }

        public void DispatchAction(BaseAction action)
        {
            _reducer.Reduce(State, action);
            StateManager.SendStateUpdated<T>();
            StateManager.SendEffect(action);
        }
    }
}

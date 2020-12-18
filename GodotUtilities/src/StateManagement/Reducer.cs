namespace GodotUtilities.StateManagement
{
    public abstract class Reducer<T> where T : IState
    {
        public abstract void Reduce(T state, BaseAction action);
    }
}

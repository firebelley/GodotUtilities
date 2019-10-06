namespace GodotApiTools.Logic.Interface
{
    public interface IStateExector
    {
        void StateEntered();
        void StateActive();
        void StateLeft();
    }
}
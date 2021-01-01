using System;
using Godot;

namespace GodotUtilities.StateManagement
{
    public abstract class StateManager : Node
    {
        private static StateManager _instance;

        public override void _EnterTree()
        {
            _instance = this;
            Initialize();
        }

        protected abstract void Initialize();

        public static void SendEffect(BaseAction action)
        {
            var signalName = GetEffectSignalName(action);
            if (_instance.HasUserSignal(signalName))
            {
                _instance.EmitSignal(signalName, action);
            }
        }

        public static void SendStateUpdated<T>() where T : IState
        {
            var signalName = GetStateUpdateSignalName(typeof(T));
            if (_instance.HasUserSignal(signalName))
            {
                _instance.EmitSignal(signalName);
            }
        }

        public static void CreateEffect<T>(Node node, string methodName) where T : BaseAction
        {
            var signalName = GetEffectSignalName(typeof(T));
            ConnectCustomSignal(signalName, node, methodName);
        }

        public static void CreateDeferredEffect<T>(Node node, string methodName) where T : BaseAction
        {
            var signalName = GetEffectSignalName(typeof(T));
            ConnectCustomSignal(signalName, node, methodName, true);
        }

        public static void ConnectStateUpdate<T>(Node node, string methodName) where T : IState
        {
            var signalName = GetStateUpdateSignalName(typeof(T));
            ConnectCustomSignal(signalName, node, methodName);
        }

        private static void ConnectCustomSignal(string signalName, Node node, string methodName, bool deferred = false)
        {
            if (!_instance.HasSignal(signalName))
            {
                _instance.AddUserSignal(signalName);
            }
            uint flags = 0;
            if (deferred)
            {
                flags |= (uint)ConnectFlags.Deferred;
            }
            _instance.Connect(signalName, node, methodName, null, flags);
        }

        private static string GetEffectSignalName(BaseAction action)
        {
            return GetEffectSignalName(action.GetType());
        }

        private static string GetEffectSignalName(Type actionType)
        {
            return $"{actionType}Effect";
        }

        private static string GetStateUpdateSignalName(Type stateType)
        {
            return $"{stateType}Updated";
        }
    }
}

using Godot;

namespace GodotUtilities
{
    public static class ObjectExtension
    {
        private const string DICT_NAME = "name";
        private const string DICT_TARGET = "target";
        private const string DICT_SIGNAL = "signal";
        private const string DICT_METHOD = "method";

        public static void DisconnectAllSignals(this Object target, Object source)
        {
            var signalList = source.GetSignalList();
            foreach (var signal in signalList)
            {
                var dict = (Godot.Collections.Dictionary)signal;
                var signalName = (string)dict[DICT_NAME];
                var connectedSignalList = source.GetSignalConnectionList(signalName);
                foreach (var connectedSignal in connectedSignalList)
                {
                    var connectedDict = (Godot.Collections.Dictionary)connectedSignal;
                    if (connectedDict[DICT_TARGET] == target)
                    {
                        source.Disconnect((string)connectedDict[DICT_SIGNAL], target, (string)connectedDict[DICT_METHOD]);
                    }
                }
            }
        }

        public static T InstanceOrDefault<T>(this Object _, Object obj) where T : class
        {
            return Object.IsInstanceValid(obj) ? obj as T : default;
        }
    }
}
using Godot;

namespace GodotUtilities.Extension
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
                var dict = (Godot.Collections.Dictionary) signal;
                var signalName = (string) dict[DICT_NAME];
                var connectedSignalList = source.GetSignalConnectionList(signalName);
                foreach (var connectedSignal in connectedSignalList)
                {
                    var connectedDict = (Godot.Collections.Dictionary) connectedSignal;
                    if (connectedDict[DICT_TARGET] == target)
                    {
                        source.Disconnect((string) connectedDict[DICT_SIGNAL], target, (string) connectedDict[DICT_METHOD]);
                    }
                }
            }
        }
    }
}
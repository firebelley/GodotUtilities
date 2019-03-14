using Godot;

namespace GodotTools.Util
{
    public static class NodeUtil
    {
        public static void DisconnectAllSignals(Node source, Node target)
        {
            var signalList = source.GetSignalList();
            foreach (var signal in signalList)
            {
                var dict = (Godot.Collections.Dictionary) signal;
                var signalName = (string) dict["name"];
                var connectedSignalList = source.GetSignalConnectionList(signalName);
                foreach (var connectedSignal in connectedSignalList)
                {
                    var connectedDict = (Godot.Collections.Dictionary) connectedSignal;
                    if (connectedDict["target"] == target)
                    {
                        source.Disconnect((string) connectedDict["signal"], target, (string) connectedDict["method"]);
                    }
                }
            }
        }
    }
}
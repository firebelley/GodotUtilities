using Godot;
using Godot.Collections;

namespace GodotUtilities.Util
{
    public class RaycastResult
    {
        public Vector2 Position { get; set; }
        public Vector2 Normal { get; set; }
        public GodotObject Collider { get; set; }
        public int ColliderId { get; set; }
        public Rid Rid { get; set; }
        public int Shape { get; set; }
        public Vector2 FromPosition { get; set; }
        public Vector2 ToPosition { get; set; }

        public RaycastResult(Vector2 from, Vector2 to, Dictionary resultDict)
        {
            FromPosition = from;
            ToPosition = to;
            Position = (Vector2)resultDict["position"];
            Normal = (Vector2)resultDict["normal"];
            Collider = (GodotObject)resultDict["collider"];
            ColliderId = (int)resultDict["collider_id"];
            Rid = (Rid)resultDict["rid"];
            Shape = (int)resultDict["shape"];
        }
    }
}

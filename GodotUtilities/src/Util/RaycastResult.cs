using Godot;
using Godot.Collections;

namespace GodotUtilities.Util
{
    public class RaycastResult
    {
        public Vector2 Position { get; set; }
        public Vector2 Normal { get; set; }
        public Object Collider { get; set; }
        public int ColliderId { get; set; }
        public RID RID { get; set; }
        public int Shape { get; set; }
        public Vector2 FromPosition { get; set; }
        public Vector2 ToPosition { get; set; }

        public RaycastResult(Vector2 from, Vector2 to, Dictionary resultDict)
        {
            FromPosition = from;
            ToPosition = to;
            Position = (Vector2)resultDict["position"];
            Normal = (Vector2)resultDict["normal"];
            Collider = (Object)resultDict["collider"];
            ColliderId = (int)resultDict["collider_id"];
            RID = (RID)resultDict["rid"];
            Shape = (int)resultDict["shape"];
        }
    }
}

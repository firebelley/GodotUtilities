using Godot;

namespace GodotUtilities.Util
{
    public class ShapecastResult
    {
        public Vector2 Point { get; set; }
        public Vector2 Normal { get; set; }
        public GodotObject Collider { get; set; }
        public int ColliderId { get; set; }
        public Rid Rid { get; set; }
        public int Shape { get; set; }
        public Vector2 FromPosition { get; set; }
        public Vector2 ToPosition { get; set; }
        public Vector2 LinearVelocity { get; set; }

        public ShapecastResult(Vector2 from, Vector2 to, Godot.Collections.Dictionary resultDict)
        {
            FromPosition = from;
            ToPosition = to;
            Point = (Vector2)resultDict["point"];
            Normal = (Vector2)resultDict["normal"];
            Collider = (GodotObject)resultDict["collider"];
            ColliderId = (int)resultDict["collider_id"];
            Rid = (Rid)resultDict["rid"];
            Shape = (int)resultDict["shape"];
            LinearVelocity = (Vector2)resultDict["linear_velocity"];
        }
    }
}

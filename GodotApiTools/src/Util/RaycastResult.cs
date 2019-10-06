using Godot;
using Godot.Collections;

namespace GodotApiTools.Util
{
    public class RaycastResult
    {
        public Vector2 Position { get; set; }
        public Vector2 Normal { get; set; }
        public Object Collider { get; set; }
        public int ColliderId { get; set; }
        public RID RID { get; set; }
        public int Shape { get; set; }
        public object Metadata { get; set; }

        public static RaycastResult FromResultDictionary(Dictionary dict)
        {
            var result = new RaycastResult();
            result.Position = (Vector2) dict["position"];
            result.Normal = (Vector2) dict["normal"];
            result.Collider = (Object) dict["collider"];
            result.ColliderId = (int) dict["collider_id"];
            result.RID = (RID) dict["rid"];
            result.Shape = (int) dict["shape"];
            result.Metadata = dict["metadata"];
            return result;
        }

        private RaycastResult()
        {

        }
    }
}
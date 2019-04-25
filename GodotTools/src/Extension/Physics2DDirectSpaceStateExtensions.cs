using Godot;
using GodotTools.Util;

namespace GodotTools.Extension
{
    public static class Physics2DDirectSpaceStateExtensions
    {
        public static RaycastResult Raycast(this Physics2DDirectSpaceState state, Vector2 from, Vector2 to, Godot.Collections.Array exclude = null, int collisionLayer = int.MaxValue, bool collideWithBodies = true, bool collideWithAreas = false)
        {
            var raycastDict = state.IntersectRay(from, to, exclude, collisionLayer, collideWithBodies, collideWithAreas);
            if (raycastDict != null && raycastDict.Count > 0)
            {
                return RaycastResult.FromResultDictionary(raycastDict);
            }
            return null;
        }
    }
}
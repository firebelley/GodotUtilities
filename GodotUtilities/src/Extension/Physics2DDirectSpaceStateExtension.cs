using Godot;
using GodotUtilities.Util;

namespace GodotUtilities
{
    public static class Physics2DDirectSpaceStateExtensions
    {
        /// <summary>
        /// Returns a RaycastResult if there is a collision, otherwise returns null.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="exclude"></param>
        /// <param name="collisionLayer"></param>
        /// <param name="collideWithBodies"></param>
        /// <param name="collideWithAreas"></param>
        /// <returns></returns>
        public static RaycastResult Raycast(this PhysicsDirectSpaceState2D state, PhysicsRayQueryParameters2D query)
        {
            var raycastDict = state.IntersectRay(query);
            if (raycastDict?.Count > 0)
            {
                return new RaycastResult(query.From, query.To, raycastDict);
            }
            return null;
        }
    }
}

using Godot;
using GodotUtilities.Util;

namespace GodotUtilities
{
    public static class ResourcePreloaderExtension
    {
        /// <summary>
        /// Instances a scene with the resource name. Returns null if resource was not found or was not a packed scene.
        /// </summary>
        /// <param name="preloader"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T InstanceSceneOrNull<T>(this ResourcePreloader preloader, string name) where T : Node
        {
            if (!preloader.HasResource(name))
            {
                Logger.Error("Preloader did not have a resource with name " + name);
                return null;
            }

            if (!(preloader.GetResource(name) is PackedScene resource))
            {
                Logger.Error("Resource with name " + name + " was not a " + nameof(PackedScene));
                return null;
            }

            return resource.InstantiateOrNull<T>();
        }

        public static T InstanceSceneOrNull<T>(this ResourcePreloader preloader) where T : Node
        {
            return preloader.InstanceSceneOrNull<T>(typeof(T).Name);
        }
    }
}

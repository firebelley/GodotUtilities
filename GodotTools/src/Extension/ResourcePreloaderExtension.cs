using Godot;
using GodotTools.Util;

namespace GodotTools.Extension
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
        public static T InstanceScene<T>(this ResourcePreloader preloader, string name) where T : Node
        {
            if (!preloader.HasResource(name))
            {
                Logger.Error("Preloader did not have a resource with name " + name);
                return null;
            }

            var resource = preloader.GetResource(name) as PackedScene;
            if (resource == null)
            {
                Logger.Error("Resource with name " + name + " was not a " + nameof(PackedScene));
                return null;
            }

            return resource.Instance() as T;
        }

        public static T InstanceScene<T>(this ResourcePreloader preloader) where T : Node
        {
            return preloader.InstanceScene<T>(typeof(T).Name);
        }
    }
}
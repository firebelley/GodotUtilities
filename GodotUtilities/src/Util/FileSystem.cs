using System.Collections.Generic;
using Godot;

namespace GodotUtilities.Util
{
    public static class FileSystem
    {
        public static List<T> InstanceScenesInPath<T>(string dirPath) where T : Node
        {
            if (dirPath[^1] != '/')
            {
                dirPath += "/";
            }

            var scenes = new List<T>();

            using var dir = DirAccess.Open(dirPath);
            if (dir == null)
            {
                Logger.Error("Could not open directory " + dirPath);
                return scenes;
            }

            dir.ListDirBegin();
            while (true)
            {
                var path = dir.GetNext();
                if (string.IsNullOrEmpty(path))
                {
                    break;
                }
                if (!path.Contains(".converted.res") && path.Contains(".tscn"))
                {
                    path = path.Replace(".remap", "");
                    var fullPath = dirPath + path;

                    if (GD.Load(fullPath) is PackedScene packedScene)
                    {
                        var scene = packedScene.Instantiate();
                        if (scene is T node)
                        {
                            scenes.Add(node);
                        }
                        else
                        {
                            scene.QueueFree();
                        }
                    }
                }
            }
            dir.ListDirEnd();

            return scenes;
        }

        public static List<T> LoadResourcesInPath<T>(string path) where T : Resource
        {
            using var dir = DirAccess.Open(path);
            var results = new List<T>();
            if (dir != null)
            {
                dir.ListDirBegin();
                var fileName = dir.GetNext();
                while (fileName != string.Empty)
                {
                    if (!dir.CurrentIsDir())
                    {
                        if (fileName.EndsWith(".converted.res") || fileName.EndsWith(".tres"))
                        {
                            fileName = fileName.Replace(".converted.res", string.Empty);
                            var fullPath = $"{path}/{fileName}";
                            var resource = GD.Load(fullPath);
                            if (resource is not T res)
                            {
                                GD.PushWarning($"Could not load resource at {fullPath} with type {typeof(T).Name}");
                                continue;
                            }
                            results.Add(res);
                        }
                    }
                    fileName = dir.GetNext();
                }
                dir.ListDirEnd();
            }
            else
            {
                GD.PushWarning($"Could load resources in path {path}");
            }
            return results;
        }
    }
}

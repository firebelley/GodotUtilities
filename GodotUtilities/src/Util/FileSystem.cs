using System.Collections.Generic;
using Godot;

namespace GodotUtilities.Util
{
    public static class FileSystem
    {
        public static List<T> InstanceScenesInPath<T>(string dirPath) where T : Node
        {
            if (dirPath[dirPath.Length - 1] != '/')
            {
                dirPath += "/";
            }

            var scenes = new List<T>();

            var dir = new Directory();
            var err = dir.Open(dirPath);
            if (err != Error.Ok)
            {
                Logger.Error("Could not open directory " + dirPath);
                return scenes;
            }

            dir.ListDirBegin(true);
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
                        var scene = packedScene.Instance();
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

            return scenes;
        }
    }
}
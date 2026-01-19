namespace GodotUtilities.Util;

using System.Collections.Generic;
using Godot;

public static class FileSystem
{
    public static List<T> InstantiateScenesInPath<T>(string dirPath) where T : Node
    {
        if (dirPath[^1] != '/')
        {
            dirPath += "/";
        }

        var scenes = new List<T>();
        var files = ResourceLoader.ListDirectory(dirPath);

        foreach (var fileName in files)
        {
            if (fileName.EndsWith('/')) continue;

            var fullPath = $"{dirPath}{fileName}";
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

        return scenes;
    }

    public static List<T> LoadResourcesInPath<T>(string path) where T : Resource
    {
        if (path[^1] != '/')
        {
            path += "/";
        }

        var results = new List<T>();
        var files = ResourceLoader.ListDirectory(path);

        foreach (var fileName in files)
        {
            if (fileName.EndsWith('/'))
            {
                continue;
            }

            var fullPath = $"{path}/{fileName}";
            var resource = GD.Load(fullPath);
            if (resource is not T res)
            {
                GD.PushWarning($"Could not load resource at {fullPath} with type {typeof(T).Name}");
                continue;
            }
            results.Add(res);
        }

        return results;
    }

    public static void ForResourcesInDirectory(string path, Action<string, string> fileAction, bool includeSubdirectories = false)
    {
        var files = ResourceLoader.ListDirectory(path);

        foreach (var file in files)
        {
            if (file.EndsWith('/') && includeSubdirectories)
            {
                ForResourcesInDirectory($"{path}/{file}", fileAction, includeSubdirectories);
                continue;
            }

            fileAction(file, $"{path}/{file}");
        }
    }
}

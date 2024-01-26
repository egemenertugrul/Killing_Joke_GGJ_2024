using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KillingJoke.Core.Utils
{
    public static class ResourcesExtension
    {
        public static List<string> LoadPathsRecursively(string path, ref List<string> paths) // Does not work on mobile build
        {
            var fullPath = Application.dataPath + "/Resources/" + path;
            DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Name.Contains(".prefab") && !file.Name.Contains(".meta"))
                {
                    paths.Add(path + "/" + file.Name.Replace(".prefab", ""));
                }
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                LoadPathsRecursively(path + "/" + dir.Name, ref paths);
            }

            return paths;
        }

        public static List<GameObject> LoadAllRecursively(string basePath)
        {
            List<string> paths = new List<string>();
            LoadPathsRecursively(basePath, ref paths);

            List<GameObject> gameObjects = new List<GameObject>();
            foreach (var path in paths)
            {
                gameObjects.Add(Resources.Load<GameObject>(path));
            }
            return gameObjects;
        }

        public static List<(GameObject, string)> LoadAllRecursivelyWithPaths(string basePath)
        {
            List<string> paths = new List<string>();
            LoadPathsRecursively(basePath, ref paths);

            List<(GameObject, string)> list = new List<(GameObject, string)>();
            foreach (var path in paths)
            {
                list.Add((Resources.Load<GameObject>(path), path));
            }
            return list;
        }
    }
}

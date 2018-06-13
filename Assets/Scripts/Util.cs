using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Util {
    public static int[][] CloneArray(int[][] source)
    {
        return source.Select(s => s.ToArray()).ToArray();
    }

    public static List<T> CloneList<T>(List<T> oldList) where T : ICloneable
    {
        List<T> newList = new List<T>(oldList.Count);

        oldList.ForEach((item) =>
        {
            newList.Add((T)item.Clone());
        });

        return newList;
    }

    public static GameObject FindGameObjectByName(string name, string sceneName = "")
    {
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        GameObject[] gameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

        return System.Array.Find<GameObject>(gameObjects, g => g.name == name);
    }
}

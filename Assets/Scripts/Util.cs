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

    public static GameObject[][] CloneGameObjectArray(GameObject[][] source)
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

    public static GameObject FindRootGameObjectByName(string name, string sceneName = "")
    {
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        GameObject[] gameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

        return System.Array.Find<GameObject>(gameObjects, g => g.name == name);
    }

    public static GameObject FindRootGameObjectByName_SceneIndex(string name, int sceneIndex)
    {
        GameObject[] gameObjects = SceneManager.GetSceneAt(sceneIndex).GetRootGameObjects();

        return System.Array.Find<GameObject>(gameObjects, g => g.name == name);
    }

    public static GameObject[] FindRootGameObjectsByName_SceneIndex(string name, int sceneIndex)
    {
        GameObject[] gameObjects = SceneManager.GetSceneAt(sceneIndex).GetRootGameObjects();

        return System.Array.FindAll<GameObject>(gameObjects, g => g.name.Contains(name));
    }

    public static GameObject[] FindRootGameObjectsByName(string name, string sceneName = "")
    {
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        GameObject[] gameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

        return System.Array.FindAll<GameObject>(gameObjects, g => g.name.Contains(name));
    }

    //Knuth's shuffle
    public static void ShuffleArray(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int tmp = numbers[i];
            int r = UnityEngine.Random.Range(i, numbers.Length);
            numbers[i] = numbers[r];
            numbers[r] = tmp;
        }
    }
}

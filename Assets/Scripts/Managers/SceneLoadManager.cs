using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneLoadManager : MonoBehaviour
{
    private string currentScene = null; 

    void Awake()
    {
        if (SceneManager.sceneCount < 2)
        {
            StartCoroutine(LoadScene("Menu"));
            
        }
        else
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    public void WrapLoadCoroutine(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    // Load a scene with a specified string name
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
        if (currentScene != null)
        {
            findCameraInScene(currentScene).SetActive(false);
        }
        findCameraInScene(sceneName).SetActive(true);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        if (currentScene != null)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
    }

    private GameObject findCameraInScene(string sceneName)
    {
        GameObject[] gameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
        //GameObject camera = gameObjects.ToList().Find(g => g.name == "Main Camera");
        GameObject camera = System.Array.Find<GameObject>(gameObjects, g => g.name == "Main Camera");

        return camera;
    }

    // Reload the current scene
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
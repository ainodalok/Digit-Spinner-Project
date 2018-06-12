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
            Util.FindGameObjectByName("Main Camera", currentScene).SetActive(false);
        }
        Util.FindGameObjectByName("Main Camera", sceneName).SetActive(true);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        if (currentScene != null)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
    }

    // Reload the current scene
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
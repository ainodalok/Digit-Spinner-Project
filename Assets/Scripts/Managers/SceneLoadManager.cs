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
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        Util.FindGameObjectByName_SceneIndex("Main Camera", SceneManager.sceneCount - 1).SetActive(true);
        if (currentScene != null)
        {
            
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
    }

    // Reload the current scene
    public void ReloadScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }
}
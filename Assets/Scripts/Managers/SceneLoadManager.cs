using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneLoadManager : MonoBehaviour
{
    public string currentScene = "";
    public static AudioManager audioManager;

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

    void Start()
    {
        audioManager = Util.FindRootGameObjectByName("AudioManager", "Managers").GetComponent<AudioManager>();
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
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        audioManager.pausedBGM = false;
        if (currentScene != "")
        {
            Util.FindRootGameObjectByName("Main Camera", currentScene).SetActive(false);
            if (currentScene == "Menu")
            {
                audioManager.sounds[audioManager.menuBGM[audioManager.currentMenuBGMIndex]].source.Stop();
            }
            else if (currentScene == "Game")
            {
                audioManager.sounds[audioManager.gameBGM[audioManager.currentGameBGMIndex]].source.Stop();
            }
        }
        Util.FindRootGameObjectByName_SceneIndex("Main Camera", SceneManager.sceneCount - 1).SetActive(true);
        if (currentScene != "")
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
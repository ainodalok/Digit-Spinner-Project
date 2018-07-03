using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneLoadManager : MonoBehaviour
{
    public string currentScene = "";
    public GameMode currentGameMode = GameMode.None;
    public AudioManager audioManager;
    private bool loading = false;

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

    public void WrapLoadCoroutine(string sceneName, GameMode gameMode = GameMode.None)
    {
        currentGameMode = gameMode;

        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(sceneName, gameMode));
        }
    }

    // Load a scene with a specified string name
    IEnumerator LoadScene(string sceneName, GameMode gameMode = GameMode.None)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            yield return null;
        }
        audioManager.pausedBGM = false;
        if (currentScene != "")
        {
            Util.FindRootGameObjectByName("Main Camera", currentScene).SetActive(false);
            if (currentScene == "Menu")
            {
                if (audioManager.IsPlayingBGM() || audioManager.pausedBGM)
                {
                    audioManager.sounds[audioManager.menuBGM[audioManager.currentMenuBGMIndex]].source.Stop();
                }
            }
            else if (currentScene == "Game")
            {
                if (audioManager.IsPlayingBGM() || audioManager.pausedBGM)
                {
                    audioManager.sounds[audioManager.gameBGM[audioManager.currentGameBGMIndex]].source.Stop();
                }
            }
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        if (currentScene != "")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
        Util.FindRootGameObjectByName_SceneIndex("Main Camera", SceneManager.sceneCount - 1).SetActive(true);
        loading = false;
    }

    // Reload the current scene
    public void ReloadScene()
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}
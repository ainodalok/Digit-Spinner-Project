using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using DG.Tweening;

public class SceneLoadManager : MonoBehaviour
{
    public AudioManager audioManager;
    public AdManager adManager;

    [HideInInspector]
    public string currentScene = "";
    [HideInInspector]
    public GameMode currentGameMode = GameMode.None;

    private bool loading = false;
    private bool viewportBanner = false;
    private RectTransform bgMoverRect;

    void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        Input.multiTouchEnabled = false;
        if (SceneManager.sceneCount < 2)
        {
            StartCoroutine(LoadScene("Menu"));
        }
        else
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    void Update()
    {
        if (!adManager.banner.IsShown() && adManager.banner.IsLoaded())
        {
            adManager.banner.Show();
        }
        CheckViewport();
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
        if (currentScene != sceneName && currentScene != "")
        {
            // Stopping rain gracefully
            if (currentScene == "Menu")
            {
                yield return Util.FindRootGameObjectByName("Menu Camera").transform.GetChild(0).GetChild(0)
                    .DOScale(BoardController.SPAWN_SIZE, MainMenuPanelController.fadeDuration)
                    .SetEase(Ease.InCubic)
                    .WaitForCompletion();
                Util.FindRootGameObjectByName("Rain Camera").GetComponent<RainCameraController>().Stop();
            }

            yield return WaitForBackgroundMovement(sceneName);
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        audioManager.pausedBGM = false;
        if (currentScene != "")
        {
            DeactivateCamerasInCurrentScene();
            StopAudioInCurrentScene();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        GetBgMoverRect(sceneName);
        if (currentScene != "")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
        viewportBanner = false;
        CheckViewport();
        ActivateCamerasInLastScene();
#if !UNITY_EDITOR
        if (adManager.interstitial.IsLoaded() && currentScene != "")
        {
            GL.Clear(false, true, new Color (0.0f, 0.0f, 0.0f, 1.0f));
            adManager.interstitial.Show();
            while (adManager.interstitial.IsShown())
            {
                yield return null;
            }
        }
#endif
        loading = false;
    }

    private void GetBgMoverRect(string sceneName)
    {
        if (sceneName == "Game")
        {
            bgMoverRect = Util.FindRootGameObjectByName_SceneIndex("HUDCanvas", SceneManager.sceneCount - 1).
                transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        }
        else if (sceneName == "Menu")
        {
            bgMoverRect = Util.FindRootGameObjectByName_SceneIndex("BG Camera", SceneManager.sceneCount - 1).
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>();
        }
    }

    private YieldInstruction WaitForBackgroundMovement(string sceneName)
    {
        float target = 0f;

        if (sceneName == "Menu")
        {
            target = 620f;
        }

        return DOTween.To(
                () => bgMoverRect.offsetMax.x, // this is actually the RectTransform's Right value
                (val) => bgMoverRect.offsetMax = new Vector2(val, bgMoverRect.offsetMax.y),
                target,
                1.0f //make sure this plus menu scaling duration is equal to rain fading times (called either fade time or duration in rain controller variables)
            ).SetEase(Ease.InOutCubic)
            .WaitForCompletion();
    }

    private void StopAudioInCurrentScene()
    {
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

    private void DeactivateCamerasInCurrentScene()
    {
        GameObject[] camerasOld = Util.FindRootGameObjectsByName("Camera", currentScene);
        foreach (GameObject camera in camerasOld)
        {
            camera.SetActive(false);
        }
    }

    private void ActivateCamerasInLastScene()
    {
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.SetActive(true);
        }
    }

    private void ChangeViewportFitBanner()
    {
        GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height - adManager.banner.GetHeight());
        }
        viewportBanner = true;
    }

    private void ChangeViewportDefault()
    {
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
        }
        viewportBanner = false;
    }

    private void CheckViewport()
    {
        if (!viewportBanner)
        {
            if (adManager.banner.IsShown())
            {
                ChangeViewportFitBanner();
            }
            else if (adManager.banner.IsLoaded())
            {
                ChangeViewportFitBanner();
                adManager.banner.Show();
            }
        }
        else if (!(adManager.banner.IsLoaded() || adManager.banner.IsShown()))
        {
            ChangeViewportDefault();
        }
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
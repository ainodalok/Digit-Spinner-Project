using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GameAnalyticsSDK;

public class SceneLoadManager : MonoBehaviour
{
    public AudioManager audioManager;
    public AdManager adManager;

    [HideInInspector]
    public string currentScene = "";
    [HideInInspector]
    public GameMode currentGameMode = GameMode.None;

    // MenuBtn, MuteBtn, ScoreTxt, ObjectiveTxt, EndGameBtn
    public Vector3[] gamePanelScales;

    private bool loading = false;
    private bool viewportBanner = false;
    private GamePanelController gamePanelController;

    void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        //PlayServicesManager.Init();
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
        GameAnalytics.Initialize();
    }

    public void WrapLoadCoroutine(string sceneName, GameMode gameMode = GameMode.None)
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(sceneName, gameMode));
        }
    }

    // Load a scene with a specified string name
    IEnumerator LoadScene(string sceneName, GameMode gameMode = GameMode.None)
    {
        currentGameMode = gameMode;
        UpdateGamePanelScales();
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!async.isDone || PlayServicesManager.isSigningIn)
        {
            yield return null;
        }
        audioManager.pausedBGM = false;
        //Uncomment to enable INTERSTITIALS
        
#if !UNITY_EDITOR
        if (currentScene != "")
        {
            if (adManager.interstitial.IsLoaded())
            {
                GL.Clear(false, true, new Color (0.0f, 0.0f, 0.0f, 1.0f));
                adManager.interstitial.Show();
                while (adManager.interstitial.IsShown())
                {
                    yield return null;
                }
            }
        }
#endif
        
        if (currentScene != "")
        {
            DeactivateCamerasInCurrentScene();
            StopAudioInCurrentScene();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        //Shader.WarmupAllShaders();
        if (currentScene != "")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        
        ActivateCamerasInLastScene();
        CheckViewport();
        if (currentScene == "")
        {
            PlayServicesManager.Init();
            adManager.InitAds();
        }
        if (sceneName == "Game")
        {
            if (gameMode == GameMode.LimitedTurns)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game", "LimitedTurns", "Play");
            }
            else if (gameMode == GameMode.TimeAttack)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game", "TimeAttack", "Play");
            }
            else if (gameMode == GameMode.Tutorial)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game", "Tutorial", "Play");
            }
            else
            {
                GameAnalytics.NewErrorEvent(GAErrorSeverity.Error, "Unavailable game mode requested.");
            }
        }

        currentScene = sceneName;
        loading = false;
    }

    private void UpdateGamePanelScales()
    {
        if (currentScene == "Menu")
        {
            gamePanelScales = new Vector3[6]{ new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f) };
        }
        else if (currentScene == "Game")
        {
            gamePanelScales = Util.FindRootGameObjectByName_SceneIndex("HUDCanvas", SceneManager.sceneCount - 1).transform.GetChild(1).GetComponent<GamePanelController>().GetObjectScales();
        }
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

    public void ChangeViewportFitBanner()
    {
        if (!viewportBanner)
        {
            GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
            foreach (GameObject camera in camerasNew)
            {
                camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height - adManager.banner.GetHeight());
            }
            viewportBanner = true;
            
        }
    }

    public void ChangeViewportDefault()
    {
        if(viewportBanner)
        {
            GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
            foreach (GameObject camera in camerasNew)
            {
                camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
            }
            viewportBanner = false;
        }
    }

    public void CheckViewport()
    {
        if (viewportBanner)
        {
            viewportBanner = false;
            ChangeViewportFitBanner();
        }
        else
        {
            viewportBanner = true;
            ChangeViewportDefault();
        }
    }

    // Reload the current scene
    public void ReloadScene()
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, currentGameMode));
        }
    }
}
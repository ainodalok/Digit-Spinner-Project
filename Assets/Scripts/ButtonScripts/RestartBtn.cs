using System.Collections;
using UnityEngine;
using GameAnalyticsSDK;

public class RestartBtn : MonoBehaviour {
    public MenuOpener menuOpener;
    public GamePanelController gamePanel;

    private SceneLoadManager LoaderScript;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
    }

    public void RestartBtnWrapper()
    {
        StartCoroutine(RestartBtnSceneLoad());
    }

    private IEnumerator RestartBtnSceneLoad()
    {
        yield return StartCoroutine(menuOpener.SlideOffScreenAnimation());
        if (!menuOpener.gameModeManager.tracker.gameOver)
        {
            menuOpener.gameModeManager.playerGaveUp = true;
            menuOpener.FireGameOverAnalyticsEvent();
        }
        else
        {
            GameAnalytics.NewDesignEvent("AfterGame:Button:MainMenu");
        }
        LoaderScript.ReloadScene();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class MainMenuBtn : MonoBehaviour {
    public MenuOpener menuOpener;
    public BackgroundMover bgMover;
    public GamePanelController hudController;

    private SceneLoadManager LoaderScript;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
    }

    public void MainMenuBtnWrapper()
    {
        StartCoroutine(MainMenuBtnSceneLoad());
    }

    private IEnumerator MainMenuBtnSceneLoad()
    {

        StartCoroutine(hudController.Minimize());
        yield return StartCoroutine(menuOpener.SlideOffScreenAnimation());
        yield return StartCoroutine(bgMover.MoveBackground());
        if (!menuOpener.gameModeManager.tracker.gameOver)
        {
            menuOpener.gameModeManager.playerGaveUp = true;
            menuOpener.FireGameOverAnalyticsEvent();
        }
        else
        {
            GameAnalytics.NewDesignEvent("AfterGame:Button:MainMenu");
        }
        LoaderScript.WrapLoadCoroutine("Menu");
    }
}

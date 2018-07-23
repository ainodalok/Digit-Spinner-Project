using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour {
    public MenuOpener menuOpener;
    public BackgroundMover bgMover;
    public GamePanelController hudController;

    private SceneLoadManager LoaderScript;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        GetComponent<Button>().onClick.AddListener(() => MainMenuBtnWrapper());
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
        LoaderScript.WrapLoadCoroutine("Menu");
    }
}

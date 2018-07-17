using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour {
    public MenuOpener menuOpener;

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
        yield return StartCoroutine(menuOpener.SlideOffScreenAnimation());
        LoaderScript.WrapLoadCoroutine("Menu");
    }
}

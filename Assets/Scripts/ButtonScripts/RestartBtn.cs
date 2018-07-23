using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartBtn : MonoBehaviour {
    public MenuOpener menuOpener;
    public GamePanelController gamePanel;

    private SceneLoadManager LoaderScript;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        GetComponent<Button>().onClick.AddListener(() => RestartBtnWrapper());
    }

    public void RestartBtnWrapper()
    {
        StartCoroutine(RestartBtnSceneLoad());
    }

    private IEnumerator RestartBtnSceneLoad()
    {
        StartCoroutine(menuOpener.SlideOffScreenAnimation());
        yield return gamePanel.Minimize();
        LoaderScript.ReloadScene();
    }
}

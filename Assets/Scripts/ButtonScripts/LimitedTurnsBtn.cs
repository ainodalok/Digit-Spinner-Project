using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedTurnsBtn : MonoBehaviour {
    public ScalingObjectController menuController;
    public BackgroundMover bgMover;
    public RainCameraController rainController;

    private SceneLoadManager LoaderScript;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
    }

    public void LimitedTurnsBtnWrapper()
    {
        StartCoroutine(AnimateSceneChange());
    }

    private IEnumerator AnimateSceneChange()
    {
        rainController.Stop();
        yield return StartCoroutine(menuController.ScaleOut());
        yield return StartCoroutine(bgMover.MoveBackground());
        LoaderScript.WrapLoadCoroutine("Game", GameMode.LimitedTurns);
    }
}

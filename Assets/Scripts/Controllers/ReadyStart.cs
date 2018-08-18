using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class ReadyStart : MonoBehaviour {
    [HideInInspector]
    public bool ready = false;


    public GameModeManager gameModeManager;
    public GameObject board;
    public MenuOpener menuOpener;
    public BoardController boardController;

    [HideInInspector]
    public Tweener scalingTween;
    [HideInInspector]
    public Tweener slideTween;

    const float INITIAL_SCALE_DURATION = 0.2f;

    // Use this for initialization
    void Start () {
        StartCoroutine(ReadyAnimation());
	}

    public void ScaleReadyUp()
    {
        if (scalingTween != null)
        {
            scalingTween.Kill();
        }
        scalingTween = transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine);
        scalingTween.Play();
    }

    public void ScaleReadyDown()
    {
        if (scalingTween != null)
        {
            scalingTween.Kill();
        }
        scalingTween = transform.DOScale(0, INITIAL_SCALE_DURATION).SetEase(Ease.InOutSine);
        scalingTween.Play();
    }

    public void SetEnableReadyPanel(bool enabled)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = enabled;
        gameObject.GetComponent<UnityEngine.UI.Image>().enabled = enabled;
    }

    private IEnumerator ReadyAnimation()
    {
        float time = 0;
        while (time < 0.3f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }

        slideTween = transform.DOMoveX(0, 0.4f);
        slideTween.SetEase(Ease.OutBack);
        slideTween.Play();
        yield return slideTween.WaitForCompletion();

        time = 0;
        while (time < 0.5f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }

        slideTween = transform.DOLocalMoveX((transform.parent.GetComponent<RectTransform>().rect.width + transform.GetComponent<RectTransform>().rect.width) / 2.0f, 0.4f);
        slideTween.SetEase(Ease.InBack);
        slideTween.Play();
        yield return slideTween.WaitForCompletion();

        time = 0;
        while (time < 0.3f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }

        if (GameModeManager.mode == GameMode.Tutorial)
        {
            gameModeManager.ShowTutorialMessage(true);
            gameModeManager.tutorialShown = true;
        }
        else
        {
            boardController.SetEnableBoard(true);
            boardController.ScaleTilesUp();
        }
        ready = true;
        if ((GameModeManager.mode == GameMode.TimeAttack) && !menuOpener.open)
        {
            (gameModeManager.tracker as Timer).StartTimer();
        }
        gameObject.SetActive(false);
    }
}


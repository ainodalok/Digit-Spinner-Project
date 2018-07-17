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

    [HideInInspector]
    public Tweener scalingTween;

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
        while (time < 0.5f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }

        Tweener slideToCenter = transform.DOMoveX(0, 0.5f);
        slideToCenter.SetEase(Ease.OutBack);
        slideToCenter.Play();
        yield return slideToCenter.WaitForCompletion();

        time = 0;
        while (time < 0.7f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }

        Tweener slideToRight = transform.DOLocalMoveX((transform.parent.GetComponent<RectTransform>().rect.width + transform.GetComponent<RectTransform>().rect.width) / 2.0f, 0.5f);
        slideToRight.SetEase(Ease.InBack);
        slideToRight.Play();
        yield return slideToRight.WaitForCompletion();

        time = 0;
        while (time < 0.5f)
        {
            yield return null;
            if (!menuOpener.open)
                time += Time.deltaTime;
        }
        
        ready = true;

        board.SetActive(true);
        if (gameModeManager.mode == GameMode.TimeAttack)
        {
            (gameModeManager.tracker as Timer).StartTimer();
        }
        

        gameObject.SetActive(false);
    }
}


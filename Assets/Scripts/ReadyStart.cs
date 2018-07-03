using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ReadyStart : MonoBehaviour {
    private bool paused = false;

    public bool ready = false;

    public GameModeManager gameModeManager;
    public GameObject board;

    // Use this for initialization
    void Start () {
        StartCoroutine(ReadyAnimation());
	}

    public void TogglePause()
    {
        paused = !paused;
    }

    private IEnumerator ReadyAnimation()
    {
        float time = 0;
        while (time < 0.5f)
        {
            yield return null;
            if (!paused)
                time += Time.deltaTime;
        }

        Tween slideToCenter = transform.DOMoveX(0, 0.5f);
        slideToCenter.SetEase(Ease.OutBack);
        slideToCenter.Play();
        yield return slideToCenter.WaitForCompletion();

        time = 0;
        while (time < 0.7f)
        {
            yield return null;
            if (!paused)
                time += Time.deltaTime;
        }

        Tween slideToRight = transform.DOLocalMoveX((transform.parent.GetComponent<RectTransform>().rect.width + transform.GetComponent<RectTransform>().rect.width) / 2.0f, 0.5f);
        slideToRight.SetEase(Ease.InBack);
        slideToRight.Play();
        yield return slideToRight.WaitForCompletion();

        time = 0;
        while (time < 0.5f)
        {
            yield return null;
            if (!paused)
                time += Time.deltaTime;
        }

        board.SetActive(true);
        if (gameModeManager.mode == GameMode.TimeAttack)
        {
            (gameModeManager.tracker as Timer).StartTimer();
        }

        ready = true;
        gameObject.SetActive(false);
    }
}


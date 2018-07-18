using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuOpener : MonoBehaviour {
    [HideInInspector]
    public bool open = false;
    public bool menuToggles = false;

    public BoardController boardController;
    public GameObject menuPanel;
    public GameModeManager gameModeManager;
    public GameObject menuBtn;
    public GameObject scoreTxt;
    public GameObject scoreEndTxt;
    public ReadyStart readyStart;
    public GameObject scoreEnd;
    public GameObject restartBtn;
    public GameObject mainMenuBtn;

    private Coroutine toggleMenuCoroutine;

    private const float SLIDE_DURATION = 0.2f;

    public void ToggleMenuCoroutine()
    {
        if ((gameModeManager.mode == GameMode.TimeAttack) && readyStart.ready)
        {
            if ((gameModeManager.tracker as Timer).time == 0)
            {
                return;
            }
        }

        if (menuToggles)
        {
            return;
        }

        if (toggleMenuCoroutine == null)
        {
            toggleMenuCoroutine = StartCoroutine(ToggleMenu());
        }
        else
        {
            StopCoroutine(toggleMenuCoroutine);
            toggleMenuCoroutine = StartCoroutine(ToggleMenu());
        }
    }

    public IEnumerator ToggleMenu()
    {
        menuToggles = true;
        //Closes menu
        if (open)
        {
            yield return StartCoroutine(SlideOffScreenAnimation());
            menuPanel.SetActive(!open);
            if (readyStart.ready)
            {
                boardController.SetEnableBoard(open);
                boardController.ScaleTilesUp();
                yield return boardController.scalingSequence.WaitForCompletion();
                TimerPauseSafe(!open);
                if (boardController.fallingSequence != null)
                {
                    boardController.fallingSequence.Play();
                }
            }
            else
            {
                readyStart.SetEnableReadyPanel(open);
                readyStart.ScaleReadyUp();
                yield return readyStart.scalingTween.WaitForCompletion();
                if (readyStart.slideTween != null)
                {
                    readyStart.slideTween.Play();
                }
            }
            open = false;
        }
        //Opens menu
        else
        {
            open = true;
            if (readyStart.ready)
            {
                if (boardController.fallingSequence != null)
                {
                    boardController.fallingSequence.Pause();
                }
                TimerPauseSafe(open);
                boardController.ScaleTilesDown();
                yield return boardController.scalingSequence.WaitForCompletion();
                boardController.SetEnableBoard(!open);
            }
            else
            {
                if (readyStart.slideTween != null)
                {
                    readyStart.slideTween.Pause();
                }
                readyStart.ScaleReadyDown();
                yield return readyStart.scalingTween.WaitForCompletion();
                readyStart.SetEnableReadyPanel(!open);
            }
            menuPanel.SetActive(open);
            yield return StartCoroutine(SlideToCenterAnimation());
        }
        menuToggles = false;
    }

    private IEnumerator SlideToCenterAnimation()
    {
        Sequence slideMenuPanel = DOTween.Sequence();
        Tweener restartBtnSlide = restartBtn.transform.DOMoveX(0, SLIDE_DURATION).SetEase(Ease.OutBack);
        Tweener mainMenuBtnSlide = mainMenuBtn.transform.DOMoveX(0, SLIDE_DURATION).SetEase(Ease.OutBack);
        if (gameModeManager.tracker.gameOver)
        {
            Tweener scoreEndSlide = scoreEnd.transform.DOMoveX(0, SLIDE_DURATION).SetEase(Ease.OutBack);
            restartBtnSlide.SetDelay(scoreEndSlide.Duration() / 6.0f);
            mainMenuBtnSlide.SetDelay(2 * scoreEndSlide.Duration() / 6.0f);
            slideMenuPanel.Join(scoreEndSlide);
        }
        else
        {
            mainMenuBtnSlide.SetDelay(restartBtnSlide.Duration() / 6.0f);
        }
        slideMenuPanel.Join(restartBtnSlide);
        slideMenuPanel.Join(mainMenuBtnSlide);
        slideMenuPanel.Play();
        yield return slideMenuPanel.WaitForCompletion();
    }

    public IEnumerator SlideOffScreenAnimation()
    {
        Sequence slideMenuPanel = DOTween.Sequence();
        Tweener restartBtnSlide = restartBtn.transform.DOLocalMoveX(-restartBtn.transform.parent.localPosition.x - 
                                                                    (transform.GetComponent<RectTransform>().rect.width + 
                                                                    restartBtn.transform.GetComponent<RectTransform>().rect.width) / 2.0f, 
                                                                    SLIDE_DURATION).SetEase(Ease.InBack);
        Tweener mainMenuBtnSlide = mainMenuBtn.transform.DOLocalMoveX(-mainMenuBtn.transform.parent.localPosition.x - 
                                                                      (transform.GetComponent<RectTransform>().rect.width + 
                                                                      mainMenuBtn.transform.GetComponent<RectTransform>().rect.width) / 2.0f, 
                                                                      SLIDE_DURATION).SetEase(Ease.InBack);
        if (gameModeManager.tracker.gameOver)
        {
            Tweener scoreEndSlide = scoreEnd.transform.DOLocalMoveX(-scoreEnd.transform.parent.localPosition.x - 
                                                                    (transform.GetComponent<RectTransform>().rect.width + 
                                                                    scoreEnd.transform.GetComponent<RectTransform>().rect.width) / 2.0f, 
                                                                    SLIDE_DURATION).SetEase(Ease.InBack);
            restartBtnSlide.SetDelay(scoreEndSlide.Duration() / 6.0f);
            mainMenuBtnSlide.SetDelay(2 * scoreEndSlide.Duration() / 6.0f);
            slideMenuPanel.Join(scoreEndSlide);
        }
        else
        {
            mainMenuBtnSlide.SetDelay(restartBtnSlide.Duration() / 6.0f);
        }
        slideMenuPanel.Join(restartBtnSlide);
        slideMenuPanel.Join(mainMenuBtnSlide);
        slideMenuPanel.Play();
        yield return slideMenuPanel.WaitForCompletion();
        slideMenuPanel = null;
        restartBtn.transform.localPosition = new Vector3(0.0f, restartBtn.transform.localPosition.y, restartBtn.transform.localPosition.z);
        mainMenuBtn.transform.localPosition = new Vector3(0.0f, mainMenuBtn.transform.localPosition.y, mainMenuBtn.transform.localPosition.z);
    }

    private void TimerPauseSafe(bool enabled)
    {
        if (gameModeManager.mode == GameMode.TimeAttack)
        {
            if ((gameModeManager.tracker as Timer).time > 0)
            {
                (gameModeManager.tracker as Timer).SetEnableTimer(!enabled);
            }
        }
    }

    public void EndGame()
    {
        menuBtn.SetActive(false);
        scoreEndTxt.GetComponent<TextMeshProUGUI>().text = "Score:\n" + boardController.score;
        scoreTxt.SetActive(false);
        scoreEnd.SetActive(true);
        scoreEndTxt.SetActive(true);
    }
}

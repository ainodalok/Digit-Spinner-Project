using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuOpener : MonoBehaviour {
    [HideInInspector]
    public bool open = false;
    public bool menuToggles = false;

    public BoardController boardController;
    public GameObject menuPanel;
    public GameModeManager gameModeManager;
    public GameObject scoreEndTxt;
    public ReadyStart readyStart;
    public GameObject scoreEnd;
    public GameObject restartBtn;
    public GameObject mainMenuBtn;
    public ScalingObjectController menuBtn;
    public ScalingObjectController scoreTxt;
    public ScalingObjectController endGameBtn;
    public CurrencyTextController currencyTextController;
    public GameOverPanelController gameOverPanelController;

    private Coroutine toggleMenuCoroutine;

    private const float SLIDE_DURATION = 0.2f;

    public void ToggleMenuCoroutine()
    {
        if ((GameModeManager.mode == GameMode.TimeAttack) && readyStart.ready)
        {
            if (SafeMemory.GetInt("time") == 0)
            {
                return;
            }
        }
        Debug.Log("menutoggles - "+menuToggles);
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
                if (!(gameModeManager.tutorialShown && (GameModeManager.mode == GameMode.Tutorial)))
                {
                    boardController.SetEnableBoard(open);
                    boardController.ScaleTilesUp();
                    yield return boardController.scalingSequence.WaitForCompletion();
                    gameModeManager.TimerPauseSafe(!open);
                    if (boardController.fallingSequence != null)
                    {
                        if (boardController.fallingSequence.IsActive())
                        {
                            if (!boardController.fallingSequence.IsComplete())
                            {
                                boardController.fallingSequence.Play();
                            }
                        }
                    }
                    if (boardController.scalingHiddenSequence != null)
                    {
                        if (boardController.scalingHiddenSequence.IsActive())
                        {
                            if (!boardController.scalingHiddenSequence.IsComplete())
                            {
                                boardController.scalingHiddenSequence.Play();
                            }
                        }
                    }
                    if (boardController.shakingSequence != null)
                    {
                        if (boardController.shakingSequence.IsActive())
                        {
                            if (!boardController.shakingSequence.IsComplete())
                            {
                                boardController.shakingSequence.Play();
                            }
                        }
                    }
                }
            }
            else
            {
                readyStart.SetEnableReadyPanel(open);
                readyStart.ScaleReadyUp();
                yield return readyStart.scalingTween.WaitForCompletion();
                if (readyStart.slideTween != null)
                {
                    if (readyStart.slideTween.IsActive())
                    {
                        if (!readyStart.slideTween.IsComplete())
                        {
                            readyStart.slideTween.Play();
                        }
                    }
                }
            }
            open = false;
            if (gameModeManager.tutorialShown && (GameModeManager.mode == GameMode.Tutorial))
            {
                gameModeManager.TutorialPanel.SetActive(true);
                yield return StartCoroutine(gameModeManager.ShowTutorialMessage(true));
            }
        }
        //Opens menu
        else
        {
            if (readyStart.ready)
            {
                Debug.Log("Shown - " + gameModeManager.tutorialShown);
                Debug.Log("Opens - " + gameModeManager.tutorialOpens);
                while (gameOverPanelController.isShowing)
                {
                    yield return null;
                }
                if ((gameModeManager.tutorialShown  || gameModeManager.tutorialOpens) && (GameModeManager.mode == GameMode.Tutorial))
                {
                    Debug.Log("Hide Show Tutorial Message");
                    yield return StartCoroutine(gameModeManager.ShowTutorialMessage(false));
                    gameModeManager.TutorialPanel.SetActive(false);
                }
                else
                {
                    if (boardController.fallingSequence != null)
                    {
                        if (boardController.fallingSequence.IsActive())
                        {
                            if (boardController.fallingSequence.IsPlaying())
                            {
                                boardController.fallingSequence.Pause();
                            }
                        }
                    }
                    if (boardController.scalingHiddenSequence != null)
                    {
                        if (boardController.scalingHiddenSequence.IsActive())
                        {
                            if (boardController.scalingHiddenSequence.IsPlaying())
                            {
                                boardController.scalingHiddenSequence.Pause();
                            }
                        }
                    }
                    if (boardController.shakingSequence != null)
                    {
                        if (boardController.shakingSequence.IsActive())
                        {
                            if (boardController.shakingSequence.IsPlaying())
                            {
                                boardController.shakingSequence.Pause();
                            }
                        }
                    }
                    gameModeManager.TimerPauseSafe(!open);
                    boardController.ScaleTilesDown();
                    yield return boardController.scalingSequence.WaitForCompletion();
                    boardController.SetEnableBoard(open);
                }
            }
            else
            {
                if (readyStart.slideTween != null)
                {
                    if (readyStart.slideTween.IsActive())
                    {
                        if (readyStart.slideTween.IsPlaying())
                        {
                            readyStart.slideTween.Pause();
                        }
                    }
                }
                readyStart.ScaleReadyDown();
                yield return readyStart.scalingTween.WaitForCompletion();
                readyStart.SetEnableReadyPanel(open);
            }
            menuPanel.SetActive(!open);
            yield return StartCoroutine(SlideToCenterAnimation());
            open = true;
        }
        menuToggles = false;
    }

    public IEnumerator SlideToCenterHighscore()
    {
        Tweener scoreEndSlide = scoreEnd.transform.DOMoveX(0, SLIDE_DURATION).SetEase(Ease.OutBack);
        scoreEndSlide.Play();
        yield return scoreEndSlide.WaitForCompletion();
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

    public void EndGame()
    {
        scoreEndTxt.GetComponent<TextMeshProUGUI>().text = "Score:\n" + SafeMemory.Get("score");
        Currency.ProcessEndGame();
        currencyTextController.UpdateText();
        StartCoroutine(menuBtn.GetComponent<ScalingObjectController>().ScaleOut());
        StartCoroutine(scoreTxt.GetComponent<ScalingObjectController>().ScaleOut());
        StartCoroutine(endGameBtn.GetComponent<ScalingObjectController>().ScaleOut());
        scoreEnd.SetActive(true);
        scoreEndTxt.SetActive(true);
    }
}

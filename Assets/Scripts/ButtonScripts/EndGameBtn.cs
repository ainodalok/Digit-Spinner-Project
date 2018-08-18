using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EndGameBtn : MonoBehaviour
{
    public BoardController bc;
    public Sprite burgundyBorderPref;
    public Sprite blueBorderPref;
    public TextMeshProUGUI endGameTxt;
    public Material blue;
    public Material red;
    public MenuOpener menuOpener;
    public Transform objectiveTxtTransform;
    public GameModeManager gameModeManager;
    //private Tweener widener;
    private Coroutine timer;
    private bool clickedOnce = false;
    private bool isEnding = false;

    public void EndGameBtnAction()
    {
        /*
        if (widener != null)
        {
            DOTween.Kill(widener);
        }
        */
        if (isEnding)
        {
            return;
        }

        if (!clickedOnce)
        {
            clickedOnce = true;
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            endGameTxt.text = "Sure?";
            endGameTxt.fontSharedMaterial = red;
            Tweener widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 335, 280, 0.2f).SetEase(Ease.OutQuart);
            timer = StartCoroutine(WaitFewSecondsAndReturnButton());
        }
        else
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator WaitFewSecondsAndReturnButton()
    {
        yield return new WaitForSeconds(1.0f);

        clickedOnce = false;
        gameObject.GetComponent<Image>().sprite = blueBorderPref;
        endGameTxt.text = "Give Up";
        endGameTxt.fontSharedMaterial = blue;
        Tweener widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 280, 335, 0.2f).SetEase(Ease.OutQuart);
    }

    private IEnumerator EndGame()
    {
        isEnding = true;
        gameObject.transform.DOScale(BoardController.SPAWN_SIZE, 0.5f).SetEase(Ease.InCubic).Play();
        StopCoroutine(timer);
        gameModeManager.tracker.gameOver = true;

        while (bc.isDestroying)
        {
            yield return null;
        }

        bc.ScaleTilesDown();
        menuOpener.EndGame();
        objectiveTxtTransform.DOScale(BoardController.SPAWN_SIZE, 0.5f).SetEase(Ease.InCubic).Play();
        if (!menuOpener.open)
        {
            yield return StartCoroutine(menuOpener.ToggleMenu());
        }
        else
        {
            yield return StartCoroutine(menuOpener.SlideToCenterHighscore());
        }
    }
}

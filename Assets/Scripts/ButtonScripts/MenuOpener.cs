using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuOpener : MonoBehaviour {
    [HideInInspector]
    public bool open = false;

    public BoardController boardController;
    public GameObject menuPanel;
    public GameModeManager gameModeManager;
    public GameObject menuBtn;
    public GameObject scoreTxt;
    public GameObject scoreEndTxt;
    public ReadyStart readyStart;
    public GameObject scoreEnd;

    private Coroutine toggleMenuCoroutine;

    public void ToggleMenuCoroutine()
    {
        if ((gameModeManager.mode == GameMode.TimeAttack) && readyStart.ready)
        {
            if ((gameModeManager.tracker as Timer).time == 0)
            {
                return;
            }
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
        //Closes menu
        if (open)
        {
            open = false;
            menuPanel.SetActive(open);
            if (readyStart.ready)
            {
                boardController.SetEnableBoard(!open);
                boardController.ScaleTilesUp();
                yield return boardController.scalingSequence.WaitForCompletion();
                if (gameModeManager.mode == GameMode.TimeAttack)
                {
                    if ((gameModeManager.tracker as Timer).time > 0)
                    {
                        (gameModeManager.tracker as Timer).SetEnableTimer(!open);
                    }
                }
            }
            else
            {
                readyStart.SetEnableReadyPanel(!open);
                readyStart.ScaleReadyUp();
                yield return readyStart.scalingTween.WaitForCompletion();
            }
            DOTween.PlayAll();
        }
        //Opens menu
        else
        {
            open = true;
            DOTween.PauseAll();
            if (readyStart.ready)
            {
                if (gameModeManager.mode == GameMode.TimeAttack)
                {
                    if ((gameModeManager.tracker as Timer).time > 0)
                    {
                        (gameModeManager.tracker as Timer).SetEnableTimer(!open);
                    }
                }
                boardController.ScaleTilesDown();
                yield return boardController.scalingSequence.WaitForCompletion();
                boardController.SetEnableBoard(!open);
            }
            else
            {
                readyStart.ScaleReadyDown();
                yield return readyStart.scalingTween.WaitForCompletion();
                readyStart.SetEnableReadyPanel(!open);
            }
            menuPanel.SetActive(open);
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

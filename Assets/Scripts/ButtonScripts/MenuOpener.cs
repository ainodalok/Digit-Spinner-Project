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
        if ((gameModeManager.mode == GameMode.TimeAttack) && readyStart.ready)
        {
            (gameModeManager.tracker as Timer).ToggleTimer();
        }
        //Closes menu
        if (open)
        {
            open = false;
            menuPanel.SetActive(false);
            if (readyStart.ready)
            {
                boardController.SetEnableBoard(true);
                boardController.ScaleTilesUp();
                yield return boardController.scalingSequence.WaitForCompletion();
            }
            else
            {
                readyStart.TogglePause();
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
                boardController.ScaleTilesDown();
                yield return boardController.scalingSequence.WaitForCompletion();
                boardController.SetEnableBoard(false);
            }
            else
            {
                readyStart.TogglePause();
            }
            menuPanel.SetActive(true);
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

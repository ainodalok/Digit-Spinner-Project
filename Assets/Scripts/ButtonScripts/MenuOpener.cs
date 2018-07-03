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

    public void ToggleMenu()
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
            boardController.SetEnableBoard(true);
            DOTween.PlayAll();
            readyStart.TogglePause();
        }
        //Opens menu
        else
        {
            open = true;
            boardController.SetEnableBoard(false);
            menuPanel.SetActive(true);
            DOTween.PauseAll();
            readyStart.TogglePause();
        }
    }

    public void EndGame()
    {
        menuBtn.SetActive(false);
        scoreEndTxt.GetComponent<TextMeshProUGUI>().text = scoreTxt.GetComponent<TextMeshProUGUI>().text;
        scoreTxt.SetActive(false);
        scoreEnd.SetActive(true);
        scoreEndTxt.SetActive(true);
    }
}

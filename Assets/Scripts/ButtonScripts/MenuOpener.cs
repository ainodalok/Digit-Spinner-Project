using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuOpener : MonoBehaviour {
    public bool open = false;

    private GameObject Board;
    private GameObject MenuPanel;
    private GameModeManager gameModeManager;

    void Start()
    {
        MenuPanel = transform.Find("MenuPanel").gameObject;
        Board = Util.FindRootGameObjectByName_SceneIndex("Board", SceneManager.sceneCount - 1);
        gameModeManager = Board.GetComponent<BoardController>().GetGameModeManager();
    }

    public void ToggleMenu()
    {
        if (gameModeManager.mode == GameMode.TimeAttack)
        {
            (gameModeManager.tracker as Timer).ToggleTimer();
        }
        //Closes menu
        if (open)
        {
            open = false;
            MenuPanel.SetActive(false);
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.UnPause();
            //SceneLoadManager.audioManager.pausedBGM = false;
            Board.GetComponent<BoardController>().SetEnableBoard(true);
            DOTween.PlayAll();
        }
        //Opens menu
        else
        {
            open = true;
            Board.GetComponent<BoardController>().SetEnableBoard(false);
            //SceneLoadManager.audioManager.pausedBGM = true;
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.Pause();
            MenuPanel.SetActive(true);
            DOTween.PauseAll();
        }
    }

    public void EndGame()
    {
        Transform gamePanelTransform = transform.Find("GamePanel");
        GameObject scoreTxt = gamePanelTransform.Find("Header").Find("ScoreTxt").gameObject;
        GameObject scoreEndTxt = MenuPanel.transform.Find("ScoreEndTxt").gameObject;
        gamePanelTransform.Find("Footer").Find("MenuBtn").gameObject.SetActive(false);
        scoreEndTxt.GetComponent<TextMeshProUGUI>().text = scoreTxt.GetComponent<TextMeshProUGUI>().text;
        scoreTxt.SetActive(false);
        scoreEndTxt.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpener : MonoBehaviour {
    public bool open = false;

    private GameObject Board;
    private GameObject MenuPanel;

    void Start()
    {
        MenuPanel = transform.Find("MenuPanel").gameObject;
        Board = Util.FindRootGameObjectByName_SceneIndex("Board", SceneManager.sceneCount - 1);
    }

    public void ToggleMenu()
    {
        if (open)
        {
            open = false;
            MenuPanel.SetActive(false);
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.UnPause();
            //SceneLoadManager.audioManager.pausedBGM = false;
            Board.GetComponent<BoardController>().SetEnableBoard(true);
        }
        else
        {
            open = true;
            Board.GetComponent<BoardController>().SetEnableBoard(false);
            //SceneLoadManager.audioManager.pausedBGM = true;
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.Pause();
            MenuPanel.SetActive(true);
        }
    }
}

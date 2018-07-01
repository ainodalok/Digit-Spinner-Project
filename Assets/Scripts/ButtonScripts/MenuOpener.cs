using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour {
    public bool open = false;

    private GameObject Board;
    private GameObject MenuPanel;

    void Start()
    {
        Board = Util.FindRootGameObjectByName("Board", "Game");
        MenuPanel = transform.Find("MenuPanel").gameObject;
    }

    public void ToggleMenu()
    {
        if (open)
        {
            open = false;
            MenuPanel.SetActive(false);
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.UnPause();
            //SceneLoadManager.audioManager.pausedBGM = false;
            Board.GetComponent<BoardController>().SetEnableBoardRenderers(true);
        }
        else
        {
            open = true;
            Board.GetComponent<BoardController>().SetEnableBoardRenderers(false);
            //SceneLoadManager.audioManager.pausedBGM = true;
            //SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.gameBGM[SceneLoadManager.audioManager.currentGameBGMIndex]].source.Pause();
            MenuPanel.SetActive(true);
        }
    }
}

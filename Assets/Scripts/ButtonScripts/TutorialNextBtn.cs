using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNextBtn : MonoBehaviour {
    public GameObject tutorialPanel;
    public GameModeManager gameModeManager;
    public BoardController boardController;

	public void ContinueTutorial()
    {
        gameModeManager.ShowTutorialMessage(false);
        gameModeManager.tutorialShown = false;
        boardController.SetEnableBoard(true);
        boardController.ScaleTilesUp();
    }
}

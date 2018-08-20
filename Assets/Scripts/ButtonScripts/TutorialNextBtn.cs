using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNextBtn : MonoBehaviour {
    public GameObject tutorialPanel;
    public GameModeManager gameModeManager;
    public BoardController boardController;
    public EndGameBtn endGameBtn;

    private Coroutine continueTutorial;

    public void WrapContinueTutorial()
    {
        continueTutorial = StartCoroutine(ContinueTutorial());
    }

	public IEnumerator ContinueTutorial()
    {
        
        if ((gameModeManager.tracker as SectionCounter).sectionCurrent == 8)
        {
            yield return StartCoroutine(endGameBtn.EndGame());
            gameModeManager.tutorialShown = false;
        }
        else
        {
            yield return StartCoroutine(gameModeManager.ShowTutorialMessage(false));
            gameModeManager.tutorialShown = false;
            if (!(boardController.menuOpener.open || (!boardController.menuOpener.open && boardController.menuOpener.menuToggles)))
            {
                boardController.SetEnableBoard(true);
                boardController.ScaleTilesUp();
                tutorialPanel.SetActive(false);
            }
        }
        
    }
}

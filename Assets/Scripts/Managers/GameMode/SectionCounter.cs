using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SectionCounter : ObjectiveTracker
{
    public GameModeManager gameModeManager;
    public BoardController boardController;

    [HideInInspector] [System.NonSerialized]
    public int sectionCount = 8;
    [HideInInspector]
    public int sectionCurrent = 1;


    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Section:\n1/{0}", sectionCount);
    }

    public void NextSection()
    {
        if (!gameModeManager.tracker.gameOver)
        {
            StartCoroutine(NextSectionCoroutine());
        }
    }

    private IEnumerator NextSectionCoroutine()
    {
        
        boardController.SetEnableTileColliders(false);
        yield return new WaitForSeconds(0.5f);
        sectionCurrent += 1;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Section:\n{0}/{1}", sectionCurrent, sectionCount);
        boardController.ScaleTilesDown();
        yield return boardController.scalingSequence.WaitForCompletion();
        boardController.SetEnableBoard(false);
        gameModeManager.tutorialOpens = true;
        yield return StartCoroutine(gameModeManager.ShowTutorialMessage(true));
        gameModeManager.tutorialShown = true;
        gameModeManager.tutorialOpens = false;
    }
}

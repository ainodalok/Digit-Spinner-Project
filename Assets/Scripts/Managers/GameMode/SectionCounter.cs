using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SectionCounter : ObjectiveTracker
{
    public GameModeManager gameModeManager;
    public BoardController boardController;

    [HideInInspector]
    public int sectionCount = 8;
    [HideInInspector]
    public int sectionCurrent = 1;


    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Section:\n1/{0}", sectionCount);
    }

    public void NextSection()
    {
        StartCoroutine(NextSectionCoroutine());
    }

    private IEnumerator NextSectionCoroutine()
    {
        sectionCurrent += 1;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Section:\n1/{0}", sectionCount);
        boardController.ScaleTilesDown();
        yield return boardController.scalingSequence.WaitForCompletion();
        boardController.SetEnableBoard(false);
        gameModeManager.ShowTutorialMessage(true);
        gameModeManager.tutorialShown = true;
    }
}

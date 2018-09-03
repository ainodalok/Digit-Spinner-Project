﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class Regenerate : MonoBehaviour {
    public TextMeshProUGUI RegenLeftTxt;
    public BoardController boardController;
    public GameModeManager gameModeManager;

    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        SafeMemory.SetInt("regenLeft", 1);
        RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
    }

    public void RegenerateBoard()
    {
        if (SafeMemory.GetInt("regenLeft") > 0)
        {
            boardController.boardLogic.GenerateActiveTiles();
            while (MatchFinder.IsGameOver(boardController.boardLogic.activeTiles))
            {
                boardController.boardLogic.GenerateActiveTiles();
            }
            boardController.UpdateDigitsBasic();
            SafeMemory.SetInt("regenLeft", SafeMemory.GetInt("regenLeft") - 1);
            RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
        }
        if (GameModeManager.mode == GameMode.Tutorial)
        {
            (gameModeManager.tracker as SectionCounter).NextSection();
        }
        else
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "PowerUpRegenerate", 1, "Use", "PowerUpUse");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class Regenerate : MonoBehaviour {
    public TextMeshProUGUI RegenLeftTxt;
    public BoardController boardController;
    public GameModeManager gameModeManager;

    public static bool used = false;

    void Start()
    {
        used = false;
        //CHANGE TO A REAL VALUE LATER
        //SafeMemory.SetInt("regenLeft", 1);
        if (GameModeManager.mode != GameMode.Tutorial)
        {
            PowerUps.GetPowerUpLeft("regenLeft");
        }
        else
        {
            SafeMemory.SetInt("regenLeft", 0);
        }
        Debug.Log(SafeMemory.GetInt("regenLeft"));
        RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
        Debug.Log(RegenLeftTxt.text);
    }

    public void RegenerateBoard()
    {
        if (!used)
        {
            if (SafeMemory.GetInt("regenLeft") > 0)
            {
                boardController.boardLogic.GenerateActiveTiles();
                while (MatchFinder.IsGameOver(boardController.boardLogic.activeTiles))
                {
                    boardController.boardLogic.GenerateActiveTiles();
                }
                boardController.UpdateDigitsBasic();
                used = true;
                if (GameModeManager.mode != GameMode.Tutorial)
                {
                    PowerUps.ChangePowerUpLeft("regenLeft", PowerUps.GetPowerUpLeft("regenLeft") - 1);
                }
                else
                {
                    SafeMemory.SetInt("regenLeft", 0);
                }
                RegenLeftTxt.SetText(SafeMemory.GetInt("regenLeft").ToString());
            }
            if (GameModeManager.mode == GameMode.Tutorial)
            {
                (gameModeManager.tracker as SectionCounter).NextSection();
            }
            else
            {
                GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "regenLeft", 1, "Use", "PowerUpUse");
            }
        }
    }
}

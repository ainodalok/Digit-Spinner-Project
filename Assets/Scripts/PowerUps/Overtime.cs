using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class Overtime : MonoBehaviour {
    public GameModeManager gameModeManager;
    public TextMeshProUGUI OTLeftTxt;
    private const int ADDITIONAL_TURNS = 2;
    private const int ADDITIONAL_TIME = 300;
    
    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        //SafeMemory.SetInt("overtimeLeft", 1);
        if (GameModeManager.mode != GameMode.Tutorial)
        {
            PowerUps.GetPowerUpLeft("overtimeLeft");
        }
        OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
    }

    public void AddAdditional()
    {
        if (SafeMemory.GetInt("overtimeLeft") > 0)
        {
            switch (GameModeManager.mode)
            {
                case GameMode.LimitedTurns:
                    SafeMemory.SetInt("turns", SafeMemory.GetInt("turns") + ADDITIONAL_TURNS);
                    (gameModeManager.tracker as TurnCounter).gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", SafeMemory.Get("turns"));
                    break;
                case GameMode.TimeAttack:
                    SafeMemory.SetInt("time", SafeMemory.GetInt("time") + ADDITIONAL_TIME);
                    (gameModeManager.tracker as Timer).gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", SafeMemory.GetInt("time") / 10, SafeMemory.GetInt("time") % 10);
                    break;
                case GameMode.Tutorial:
                    (gameModeManager.tracker as SectionCounter).NextSection();
                    break;

            }
            SafeMemory.SetInt("overtimeLeft", SafeMemory.GetInt("overtimeLeft") - 1);
            OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "PowerUpOvertime", 1, "Use", "PowerUpUse");
        }
    }
}

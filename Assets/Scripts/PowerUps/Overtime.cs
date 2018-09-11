using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class Overtime : MonoBehaviour {
    public GameModeManager gameModeManager;
    public TextMeshProUGUI OTLeftTxt;

    public static bool used = false;

    private const int ADDITIONAL_TURNS = 2;
    private const int ADDITIONAL_TIME = 300;
    
    void Start()
    {
        used = false;
        //CHANGE TO A REAL VALUE LATER
        //SafeMemory.SetInt("overtimeLeft", 1);
        if (GameModeManager.mode != GameMode.Tutorial)
        {
            PowerUps.GetPowerUpLeft("overtimeLeft");
        }
        else
        {
            SafeMemory.SetInt("overtimeLeft", 0);
        }
        OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
    }

    public void AddAdditional()
    {
        if (!used)
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
                used = true;
                if (GameModeManager.mode != GameMode.Tutorial)
                {
                    PowerUps.ChangePowerUpLeft("overtimeLeft", PowerUps.GetPowerUpLeft("overtimeLeft") - 1);
                }
                else
                {
                    SafeMemory.SetInt("overtimeLeft", 0);
                }
                OTLeftTxt.SetText(SafeMemory.GetInt("overtimeLeft").ToString());
                GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "overtimeLeft", 1, "Use", "PowerUpUse");
                GameAnalytics.NewDesignEvent("PowerUp:OverTime:Used");
            }
        }
    }
}

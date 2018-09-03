using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WrongMove : MonoBehaviour {
    public TextMeshProUGUI WMLeftTxt;

    public static bool active = false;
    public static bool used = false;

    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        //SafeMemory.SetInt("wrongMoveLeft", 1);
        if (GameModeManager.mode != GameMode.Tutorial)
        {
            PowerUps.GetPowerUpLeft("wrongMoveLeft");
        }
        WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
    }

    public void Activate()
    {
        if (!used)
        {
            if (active)
            {
                active = false;
                //PowerUps.ChangePowerUpLeft("wrongMoveLeft", SafeMemory.GetInt("wrongLoveLeft") + 1);
                //WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
                Util.SwapButtonColors(transform.GetComponent<Button>());
            }
            else
            {
                if (SafeMemory.GetInt("wrongMoveLeft") > 0)
                {
                    active = true;
                    //PowerUps.ChangePowerUpLeft("wrongMoveLeft", SafeMemory.GetInt("wrongLoveLeft") - 1);
                    //WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
                    Util.SwapButtonColors(transform.GetComponent<Button>());
                }
            }
        }
    }
}

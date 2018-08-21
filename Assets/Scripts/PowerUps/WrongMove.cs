﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WrongMove : MonoBehaviour {
    public TextMeshProUGUI WMLeftTxt;

    public static bool active = false;

    void Awake()
    {
        //CHANGE TO A REAL VALUE LATER
        SafeMemory.SetInt("wrongMoveLeft", 1);
        WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
    }

    public void Activate()
    {
        if (active)
        {
            active = false;
            SafeMemory.SetInt("wrongMoveLeft", SafeMemory.GetInt("wrongMoveLeft") + 1);
            WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
        }
        else
        {
            if (SafeMemory.GetInt("wrongMoveLeft") > 0)
            {
                active = true;
                SafeMemory.SetInt("wrongMoveLeft", SafeMemory.GetInt("wrongMoveLeft") - 1);
                WMLeftTxt.SetText(SafeMemory.GetInt("wrongMoveLeft").ToString());
            }
        }
    }
}
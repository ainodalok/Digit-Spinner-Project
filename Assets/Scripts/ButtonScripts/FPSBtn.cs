using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class FPSBtn : MonoBehaviour {
    public TextMeshProUGUI FPSText;

	void Start () {
        FPSText.text = Application.targetFrameRate.ToString();
    }
    
    public void FPSBtnAction()
    {
        if (Application.targetFrameRate == 30)
        {
            Application.targetFrameRate = 60;
            FPSText.text = "60";
            GameAnalytics.NewDesignEvent("Button:FPS:60");
        }
        else
        {
            Application.targetFrameRate = 30;
            FPSText.text = "30";
            GameAnalytics.NewDesignEvent("Button:FPS:30");
        }
    }
}

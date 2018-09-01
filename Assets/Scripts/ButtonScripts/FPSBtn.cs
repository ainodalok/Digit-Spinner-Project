using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        }
        else
        {
            Application.targetFrameRate = 30;
            FPSText.text = "30";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnCounter : ObjectiveTracker {
    //private int turnsLeft = 20;
    private const int TURNS_ON_START = 20;

	// Use this for initialization
	void Start() {
        SafeMemory.SetInt("turns", TURNS_ON_START);
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", SafeMemory.Get("turns"));
	}

    public void UpdateTurns()
    {
        SafeMemory.SetInt("turns", SafeMemory.GetInt("turns") - 1);
        //turnsLeft--;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", SafeMemory.Get("turns"));

        if (SafeMemory.GetInt("turns") <= 0)
        {
            StartCoroutine(EndGame());
        }
        if (SafeMemory.GetInt("turns") == 3)
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontMaterial = red;
        }
    }
}

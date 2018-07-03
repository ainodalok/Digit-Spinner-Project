using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnCounter : ObjectiveTracker {
    private int turnsLeft = 20;

	// Use this for initialization
	void Start() {
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", turnsLeft);
	}

    public void UpdateTurns()
    {
        turnsLeft--;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", turnsLeft);

        if (turnsLeft <= 0)
        {
            StartCoroutine(EndGame());
        }
    }
}

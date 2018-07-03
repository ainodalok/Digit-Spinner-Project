using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnCounter : ObjectiveTracker {
    private int turnsLeft;

	// Use this for initialization
	void Start () {
        StartCounter();
	}

    private void StartCounter()
    {
        turnsLeft = 20;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", turnsLeft);
        bc = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardController>();
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

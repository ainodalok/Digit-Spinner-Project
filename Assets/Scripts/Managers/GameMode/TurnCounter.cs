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
        turnsLeft = 2;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", turnsLeft);
        bc = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardController>();
    }

    public void SubtractOne()
    {
        turnsLeft--;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Turns left:\n{0}", turnsLeft);

        if (turnsLeft <= 0)
        {
            StartCoroutine(EndGame());
        }
    }

    public override void UpdateTurns()
    {
        SubtractOne();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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
        if (turnsLeft == 3)
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontMaterial = red;
        }
        if (turnsLeft == 5)
        {
            StartShakeTween();
        }
    }

    private void StartShakeTween()
    {
        if (turnsLeft > 0)
        {
            transform.GetComponent<RectTransform>().DOShakeAnchorPos(0.1f, 5, 50, 90, false, false).OnComplete(StartShakeTween);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyTextController : MonoBehaviour {
    public TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        UpdateText();
	}
	
    public void UpdateText()
    {
        text.text = string.Format("Coins:\n{0}", Currency.GetBalance()); 
    }
}

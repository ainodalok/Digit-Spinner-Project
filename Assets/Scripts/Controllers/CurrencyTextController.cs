using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyTextController : MonoBehaviour {
    public TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateText", 0.0f, 0.2f);
	}
	
    public void UpdateText()
    {
        text.text = string.Format("Credits:\n{0}", Currency.GetBalance()); 
    }
}

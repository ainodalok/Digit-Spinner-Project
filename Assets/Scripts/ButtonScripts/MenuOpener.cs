using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour {
    public bool open = false;
  
    public void ToggleMenu()
    {
        if (open)
        {
            open = false;
            transform.Find("MenuPanel").gameObject.SetActive(false);
            Util.FindGameObjectByName("Board").SetActive(true);
        }
        else
        {
            open = true;
            Util.FindGameObjectByName("Board").SetActive(false);
            transform.Find("MenuPanel").gameObject.SetActive(true);
        }
    }
}

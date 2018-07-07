using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreOpener : MonoBehaviour {
    public GameObject mainMenuPanel;
    public GameObject storePanel;
    public GameObject IAPTab;
    public GameObject coinTab;
    public Image IAPBtn;
    public Image coinBtn;
    public Sprite burgundyBorderPref;
    public Sprite blueBorderPref;


	public void OpenStore()
    {
        storePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseStore()
    {
        mainMenuPanel.SetActive(true);
        storePanel.SetActive(false);
    }

    public void OpenIAP()
    {
        coinTab.SetActive(false);
        IAPBtn.sprite = burgundyBorderPref;
        coinBtn.sprite = blueBorderPref;
        IAPTab.SetActive(true);

    }

    public void OpenCoin()
    {
        IAPTab.SetActive(false);
        IAPBtn.sprite = blueBorderPref;
        coinBtn.sprite = burgundyBorderPref;
        coinTab.SetActive(true);
    }
}

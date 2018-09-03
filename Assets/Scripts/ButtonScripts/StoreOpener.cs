using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;

public class StoreOpener : MonoBehaviour {
    public ScalingObjectController mainMenuPanel;
    public ScalingObjectController storePanel;
    public GameObject IAPTab;
    public GameObject coinTab;
    public Image IAPBtn;
    public Image coinBtn;
    public Sprite burgundyBorderPref;
    public Sprite blueBorderPref;

	public void OpenStore()
    {
        StartCoroutine(SwitchPanels(mainMenuPanel, storePanel));
        GameAnalytics.NewDesignEvent("Button:Store:Open");
    }

    public void CloseStore()
    {
        StartCoroutine(SwitchPanels(storePanel, mainMenuPanel));
    }

    private IEnumerator SwitchPanels(ScalingObjectController panelToHide, ScalingObjectController panelToShow)
    {
        yield return StartCoroutine(panelToHide.ScaleOut());
        panelToHide.gameObject.SetActive(false);
        panelToShow.gameObject.SetActive(true);
        StartCoroutine(panelToShow.ScaleIn());
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

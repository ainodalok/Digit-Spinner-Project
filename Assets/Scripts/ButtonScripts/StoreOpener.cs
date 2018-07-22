using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        StartCoroutine(SwitchPanels(mainMenuPanel, storePanel));
    }

    public void CloseStore()
    {
        StartCoroutine(SwitchPanels(storePanel, mainMenuPanel));
    }

    private IEnumerator SwitchPanels(GameObject panelToHide, GameObject panelToShow)
    {
        yield return panelToHide.transform.DOScale(BoardController.SPAWN_SIZE, MainMenuPanelController.fadeDuration)
            .SetEase(Ease.InCubic)
            .WaitForCompletion();

        panelToHide.SetActive(false);
        panelToShow.SetActive(true);
        panelToShow.transform.DOScale(BoardController.ACTIVE_SIZE, MainMenuPanelController.fadeDuration)
            .SetEase(Ease.OutCubic);
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

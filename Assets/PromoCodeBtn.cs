using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromoCodeBtn : MonoBehaviour {
    public ScalingObjectController storePanel;
    public ScalingObjectController inputTab;
    public TextMeshProUGUI inputTabTxt;
    public GameObject inputField;
    public GameObject specialPromoCloseBtn;
    public GameObject usualInfoTabBtn;

    public void PromoCodeBtnAction()
    {
        StartCoroutine(AnimatePromocodeInput());
    }
    
    private IEnumerator AnimatePromocodeInput()
    {
        usualInfoTabBtn.SetActive(false);
        specialPromoCloseBtn.SetActive(true);
        yield return storePanel.ScaleOut();
        inputField.SetActive(true);
        inputTabTxt.text = "Please Enter Promocode";
        inputTab.gameObject.SetActive(true);
        yield return inputTab.ScaleIn();
        storePanel.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour {
    public CurrencyTextController CurrencyTxt;
    public ScalingObjectController StorePanel;
    public ScalingObjectController InfoTab;
    public TextMeshProUGUI InfoTabTxt;

    public const int POWER_UP_PRICE = 70;
    public const int POWER_UP_SET_PRICE = 240;
    public const int ADBLOCK_PRICE = 500;

    public void PurchaseRegenerate()
    {
        if (Currency.ProcessPowerUpPurchase())
        {
            PowerUps.ChangePowerUpLeft("regenLeft", PowerUps.GetPowerUpLeft("regenLeft") + 1);
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    public void PurchaseOverTime()
    {
        if (Currency.ProcessPowerUpPurchase())
        {
            PowerUps.ChangePowerUpLeft("overtimeLeft", PowerUps.GetPowerUpLeft("overtimeLeft") + 1);
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    public void PurchaseBomb()
    {
        if (Currency.ProcessPowerUpPurchase())
        {
            PowerUps.ChangePowerUpLeft("bombLeft", PowerUps.GetPowerUpLeft("bombLeft") + 1);
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    public void PurchaseWrongMove()
    {
        if (Currency.ProcessPowerUpPurchase())
        {
            PowerUps.ChangePowerUpLeft("wrongMoveLeft", PowerUps.GetPowerUpLeft("wrongMoveLeft") + 1);
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    public void PurchasePowerUpSet()
    {
        if (Currency.ProcessPowerUpSetPurchase())
        {
            PowerUps.ChangePowerUpLeft("regenLeft", PowerUps.GetPowerUpLeft("regenLeft") + 1);
            PowerUps.ChangePowerUpLeft("overtimeLeft", PowerUps.GetPowerUpLeft("overtimeLeft") + 1);
            PowerUps.ChangePowerUpLeft("bombLeft", PowerUps.GetPowerUpLeft("bombLeft") + 1);
            PowerUps.ChangePowerUpLeft("wrongMoveLeft", PowerUps.GetPowerUpLeft("wrongMoveLeft") + 1);
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    public void PurchaseBlockAds()
    {
        if (Currency.ProcessBlockAdsPurchase())
        {
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    private void ShowSuccess()
    {
        InfoTabTxt.text = "Purchase successful";
        StartCoroutine(FadeToInfoTab());
    }

    private void ShowFailure()
    {
        InfoTabTxt.text = "Not enough credits";
        StartCoroutine(FadeToInfoTab());
    }

    private IEnumerator FadeToInfoTab()
    {
        yield return StartCoroutine(StorePanel.ScaleOut());
        InfoTab.gameObject.SetActive(true);
        StartCoroutine(InfoTab.ScaleIn());
    }
}

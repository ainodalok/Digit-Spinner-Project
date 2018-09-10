using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameAnalyticsSDK;

public class StoreManager : MonoBehaviour {
    public CurrencyTextController CurrencyTxt;
    public ScalingObjectController StorePanel;
    public ScalingObjectController InfoTab;
    public TextMeshProUGUI InfoTabTxt;
    public GameObject AdRemoveBtnObj;

    public const int POWER_UP_PRICE = 15;
    public const int POWER_UP_SET_PRICE = 50;
    public const int ADBLOCK_PRICE = 750;

    public void PurchaseRegenerate()
    {
        if (Currency.ProcessPowerUpPurchase())
        {
            PowerUps.ChangePowerUpLeft("regenLeft", PowerUps.GetPowerUpLeft("regenLeft") + 1);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "regenLeft", 1, "purchase", "powerUp");
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
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "overtimeLeft", 1, "purchase", "powerUp");
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
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "bombLeft", 1, "purchase", "powerUp");
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
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "wrongMoveLeft", 1, "purchase", "powerUp");
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
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "regenLeft", 1, "purchase", "powerUp");
            PowerUps.ChangePowerUpLeft("overtimeLeft", PowerUps.GetPowerUpLeft("overtimeLeft") + 1);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "overtimeLeft", 1, "purchase", "powerUp");
            PowerUps.ChangePowerUpLeft("bombLeft", PowerUps.GetPowerUpLeft("bombLeft") + 1);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "bombLeft", 1, "purchase", "powerUp");
            PowerUps.ChangePowerUpLeft("wrongMoveLeft", PowerUps.GetPowerUpLeft("wrongMoveLeft") + 1);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "wrongMoveLeft", 1, "purchase", "powerUp");
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
            AdRemove.Enable();
            GameAnalytics.NewDesignEvent("Ad:Block:On");
            ShowSuccess();
            AdRemoveBtnObj.SetActive(false);
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

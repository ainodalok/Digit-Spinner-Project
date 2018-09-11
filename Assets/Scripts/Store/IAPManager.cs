using System;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using System.Collections;
using GameAnalyticsSDK;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public CurrencyTextController currencyTxt;
    public ScalingObjectController StorePanel;
    public ScalingObjectController InfoTab;
    public TextMeshProUGUI InfoTabTxt;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    //list all product ids and then add them to config in InitializePurchasing()
    //also, provide handlers in ProcessPurchase()
    private const string CREDITS_100 = "100credits";
    private const string CREDITS_200 = "200credits";
    private const string CREDITS_500 = "500credits";
    private const string CREDITS_1000 = "1000credits";
    private const string CREDITS_5000 = "5000credits";
    private const string CREDITS_10000 = "10000credits";

    //below you may see commented implementation for it in case of different ids in apple and google stores for the same product
    //private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
    //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(CREDITS_100, ProductType.Consumable);
        builder.AddProduct(CREDITS_200, ProductType.Consumable);
        builder.AddProduct(CREDITS_500, ProductType.Consumable);
        builder.AddProduct(CREDITS_1000, ProductType.Consumable);
        builder.AddProduct(CREDITS_5000, ProductType.Consumable);
        builder.AddProduct(CREDITS_10000, ProductType.Consumable);
        /*
        builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
                { kProductNameAppleSubscription, AppleAppStore.Name },
                { kProductNameGooglePlaySubscription, GooglePlay.Name },
            });
        */
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void Buy100Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:100:Inititalize");
        BuyProductByID(CREDITS_100);
    }

    public void Buy200Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:200:Inititalize");
        BuyProductByID(CREDITS_200);
    }

    public void Buy500Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:500:Inititalize");
        BuyProductByID(CREDITS_500);
    }

    public void Buy1000Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:1000:Inititalize");
        BuyProductByID(CREDITS_1000);
    }

    public void Buy5000Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:5000:Inititalize");
        BuyProductByID(CREDITS_5000);
    }

    public void Buy10000Credits()
    {
        GameAnalytics.NewDesignEvent("IAP:10000:Inititalize");
        BuyProductByID(CREDITS_10000);
    }

    void BuyProductByID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                ShowFailure();
            }
        }
        else
        {
            ShowFailure();
        }
    }

    //works on apple only, google does it automatically
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            ShowFailure();
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            ShowFailure();
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, CREDITS_100, StringComparison.Ordinal))
        {
            Currency.ProcessPurchase(100);
            GameAnalytics.NewDesignEvent("IAP:100:Complete");
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, CREDITS_200, StringComparison.Ordinal))
        {
            GameAnalytics.NewDesignEvent("IAP:200:Complete");
            Currency.ProcessPurchase(200);
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, CREDITS_500, StringComparison.Ordinal))
        {
            GameAnalytics.NewDesignEvent("IAP:500:Complete");
            Currency.ProcessPurchase(500);
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, CREDITS_1000, StringComparison.Ordinal))
        {
            GameAnalytics.NewDesignEvent("IAP:1000:Complete");
            Currency.ProcessPurchase(1000);
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, CREDITS_5000, StringComparison.Ordinal))
        {
            GameAnalytics.NewDesignEvent("IAP:5000:Complete");
            Currency.ProcessPurchase(5000);
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, CREDITS_10000, StringComparison.Ordinal))
        {
            GameAnalytics.NewDesignEvent("IAP:10000:Complete");
            Currency.ProcessPurchase(10000);
            currencyTxt.UpdateText();
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }

        //PlayServicesManager.Init();
        //PlayServicesManager.SaveData();

        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        string reason = "";

        switch (failureReason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:
                reason = "Store unavailable.";
                break;
            case PurchaseFailureReason.ExistingPurchasePending:
                reason = "Previous purchase not finalized yet.";
                break;
            case PurchaseFailureReason.ProductUnavailable:
                reason = "Product is unavailable at the moment.";
                break;
            case PurchaseFailureReason.SignatureInvalid:
                reason = "Invalid signature.";
                break;
            case PurchaseFailureReason.UserCancelled:
                reason = "Cancelled by user.";
                break;
            case PurchaseFailureReason.PaymentDeclined:
                reason = "Payment declined.";
                break;
            case PurchaseFailureReason.DuplicateTransaction:
                reason = "Duplicate Transaction.";
                break;
        }

        ShowFailure(reason);
    }

    private void ShowFailure(string reason = "")
    {
        InfoTabTxt.text = "Purchase failed!";
        if (reason != "")
        {
            InfoTabTxt.text = InfoTabTxt.text + "\n Reason: " + reason;
        }
        else
        {
            InfoTabTxt.text = InfoTabTxt.text + "\n Reason: Unknown.";
        }
        StartCoroutine(FadeToInfoTab());
    }

    private void ShowSuccess()
    {
        InfoTabTxt.text = "Purchase successful!";
        StartCoroutine(FadeToInfoTab());
    }

    private IEnumerator FadeToInfoTab()
    {
        yield return StartCoroutine(StorePanel.ScaleOut());
        InfoTab.gameObject.SetActive(true);
        StartCoroutine(InfoTab.ScaleIn());
    }
}
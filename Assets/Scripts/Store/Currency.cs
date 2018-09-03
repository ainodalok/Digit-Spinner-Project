using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class Currency
{
    private static bool initialized = false;
    public const int DEFAULT_BALANCE = 100;

    //Use this at the beginning of each function that has to do with balance
    private static void Init()
    {
        if (!initialized)
        {
            SafeMemory.SetInt("balance", Storage.GetSafeInt("balance", DEFAULT_BALANCE));
            initialized = true;
        }
    }

    public static int GetBalance()
    {
        Init();
        return SafeMemory.GetInt("balance");
    }

    public static void ProcessEndGame()
    {
        Init();
        ChangeBalance(SafeMemory.GetInt("score") / 1000);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "credits", SafeMemory.GetInt("score") / 1000, "reward", "game");
    }

    public static void ProcessPurchase(int amount)
    {
        if (amount > 0)
        {
            Init();
            ChangeBalance(amount);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "credits", amount, "purchase", amount.ToString() + "credits");
        }
    }

    public static void ProcessRewardedVideo()
    {
        Init();
        ChangeBalance(RewardedVideo.REWARD_AMOUNT);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "credits", RewardedVideo.REWARD_AMOUNT, "reward", "rewardedVideoAd");
    }

    private static void ChangeBalance(int change)
    {
        SafeMemory.SetInt("balance", SafeMemory.GetInt("balance") + change);
        Storage.SetSafeInt("balance", SafeMemory.GetInt("balance"));
        Storage.Commit();
    }
}

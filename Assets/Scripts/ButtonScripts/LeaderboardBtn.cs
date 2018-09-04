using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GameAnalyticsSDK;

public class LeaderboardBtn : MonoBehaviour {

    public void OpenLeaderboard()
    {
        GameAnalytics.NewDesignEvent("Button:Leaderboard:Open");
        StartCoroutine(OpenUI());
    }

    private IEnumerator OpenUI()
    {
        while (PlayServicesManager.isSigningIn)
        {
            yield return null;
        }

        if (PlayServicesManager.authenicated)
        {
            Social.ShowLeaderboardUI();
        }
    }
}

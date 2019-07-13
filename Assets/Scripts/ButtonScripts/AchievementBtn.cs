using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GameAnalyticsSDK;

public class AchievementBtn : MonoBehaviour {
    public void OpenAchievements()
    {
        GameAnalytics.NewDesignEvent("Button:Achievement:Open");
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
            Social.ShowAchievementsUI();
        }
    }
}

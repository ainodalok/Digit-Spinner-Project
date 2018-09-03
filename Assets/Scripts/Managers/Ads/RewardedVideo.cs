using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedVideo
{
    public const int REWARD_AMOUNT = 35;

    private RewardBasedVideoAd video;
    private bool loaded = false;
    private bool shown = false;
    private bool loadNeed = true;

    public void Request()
    {
        video = RewardBasedVideoAd.Instance;
        video.OnAdLoaded += HandleOnAdLoaded;
        video.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        video.OnAdOpening += HandleOnAdOpening;
        video.OnAdStarted += HandleOnAdStarted;
        video.OnAdRewarded += HandleOnAdShouldBeRewarded;
        video.OnAdClosed += HandleOnAdClosed;
        video.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        LoadNew();
    }

    public void LoadNew()
    {
        loadNeed = false;

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        //NASHE
        //string adUnitId = "ca-app-pub-6252312720775415/7624766435";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        AdRequest request = new AdRequest.Builder().Build();
        video.LoadAd(request, adUnitId);
        loaded = false;
        shown = false;
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        loaded = true;
    }

    private void HandleOnAdOpening(object sender, EventArgs args)
    {
        loadNeed = true;
    }

    private void HandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        loadNeed = true;
    }

    private void HandleOnAdStarted(object sender, EventArgs args)
    {

    }

    private void HandleOnAdShouldBeRewarded(object sender, Reward reward)
    {
        Debug.Log(reward);
        Currency.ProcessRewardedVideo();
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        LoadNew();
    }

    private void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        LoadNew();
    }

    public bool IsLoaded()
    {
        return loaded;
    }

    public bool IsShown()
    {
        return shown;
    }

    public void Show()
    {
        video.Show();
        shown = true;
    }

    public bool IsLoadNeed()
    {
        return loadNeed;
    }
}

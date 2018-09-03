using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Interstitial {
    private InterstitialAd interstitial;
    private bool shown = false;
    private bool loadNeed = true;
    private bool loaded = false;

    public void Request()
    {
        loadNeed = false;
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        //NASHE
        string adUnitId = "ca-app-pub-6252312720775415/9911609303";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        Destroy();

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        LoadNew();
    }

    public void LoadNew()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        loaded = false;
    }

    public void Destroy()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            loaded = false;
        }
    }

    public void Show()
    {
        shown = true;
        interstitial.Show();
    }

    public bool IsShown()
    {
        return shown;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        shown = false;
        LoadNew();
    }

    public void HandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        loadNeed = true;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        loaded = true;
    }

    public bool IsLoaded()
    {
        return loaded;
    }

    public bool IsLoadNeed()
    {
        return loadNeed;
    }
}

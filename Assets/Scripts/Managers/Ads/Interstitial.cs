using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Interstitial {
    private InterstitialAd interstitial;
    private bool shown = false;

    public void RequestAndLoad()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        //NASHE
        //string adUnitId = "ca-app-pub-6252312720775415/9911609303";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        Destroy();

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        Debug.Log("About to build request");
        AdRequest request = new AdRequest.Builder().Build();
        Debug.Log("Request built");
        if (request == null)
        {
            Debug.Log("HaHa");
        }
        if (interstitial == null)
        {
            Debug.Log("HoHo");
        }
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        Debug.Log("Ad loaded");
    }

    public void Destroy()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
    }

    public void Show()
    {
        shown = true;
        interstitial.Show();
    }

    public bool IsLoaded()
    {
        if (interstitial != null)
        {
            return interstitial.IsLoaded();
        }
        else
        {
            return false;
        }
    }

    public bool IsShown()
    {
        return shown;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        shown = false;
        RequestAndLoad();
    }
}

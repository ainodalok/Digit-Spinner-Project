using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Banner {
    private BannerView bannerView;
    private bool loaded = false;
    private bool shown = false;

    public void Request()
    {
        Destroy();

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        //NASHE
        //string adUnitId = "ca-app-pub-6252312720775415/6436179941";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        bannerView.OnAdOpening += HandleOnAdOpening;
        LoadNew();
    }

    public void LoadNew()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerView.Hide();
        loaded = false;
        shown = false;
    }

    public void Destroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            loaded = false;
            shown = false;
        }
    }

    public float GetHeight()
    {
        return bannerView.GetHeightInPixels();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        loaded = true;
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
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
        bannerView.Show();
        shown = true;
    }
}

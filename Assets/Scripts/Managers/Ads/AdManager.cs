using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {
    public Banner banner = new Banner();
    public Interstitial interstitial = new Interstitial();

    void Awake ()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713";
        //NASHE
        //string appId = "ca-app-pub-6252312720775415~6167805410";
#elif UNITY_IPHONE
        string appId = "unexpected_platform";
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        //Ad start
        banner.Request();
        interstitial.RequestAndLoad();
    }
}

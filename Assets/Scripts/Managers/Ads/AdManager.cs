using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {
    public SceneLoadManager sceneLoadManager;
    public Banner banner;
    public Interstitial interstitial = new Interstitial();
    public RewardedVideo rewardedVideo = new RewardedVideo();

    const float RELOAD_AD_REST_TIME = 5.0f;

    public void InitAds ()
    {
        banner = new Banner(sceneLoadManager);
#if UNITY_ANDROID
        //string appId = "ca-app-pub-3940256099942544~3347511713";
        //NASHE
        string appId = "ca-app-pub-6252312720775415~6167805410";
#elif UNITY_IPHONE
        string appId = "unexpected_platform";
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        RequestAndLoadAds();
        //StartCoroutine(AdReload());
        InvokeRepeating("AdReload", 0.01f, RELOAD_AD_REST_TIME);
    }

    public void RequestAndLoadAds()
    {
        if (!AdRemove.Get())
        {
            banner.Request();
            interstitial.Request();
        }
        rewardedVideo.Request();
    }

    public void/*IEnumerator*/ AdReload()
    {
        //while (true)
        //{
            if(!AdRemove.Get())
            {
                if (banner != null)
                {
                    if (banner.IsLoadNeed())
                    {
                        banner.LoadNew();
                    }
                    else if (banner.IsLoaded() && !banner.IsShown())
                    {
                        banner.Show();
                    }
                }
                if (!interstitial.IsLoaded() && interstitial.IsLoadNeed())
                {
                    interstitial.LoadNew();
                }
            }
            if (!rewardedVideo.IsLoaded() && rewardedVideo.IsLoadNeed())
            {
                rewardedVideo.LoadNew();
            }
            //yield return new WaitForSeconds(RELOAD_AD_REST_TIME);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchRewardedVideoBtn : MonoBehaviour {
    public CurrencyTextController currencyTxt;
    private AdManager adManager;

	void Awake () {
        adManager = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().adManager;
        GetComponent<Button>().onClick.AddListener(() => WatchRewardedVideoAction());
    }

    private void WatchRewardedVideoAction()
    {
        if (adManager.rewardedVideo.IsLoaded())
        {
            adManager.rewardedVideo.Show();
        }
    }
}

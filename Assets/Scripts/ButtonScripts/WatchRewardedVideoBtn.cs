using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WatchRewardedVideoBtn : MonoBehaviour {
    public CurrencyTextController currencyTxt;
    public ScalingObjectController infoPanel;
    public TextMeshPro infoText;
    private AdManager adManager;

	void Awake () {
        adManager = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().adManager;
    }

    public void WatchRewardedVideoAction()
    {
        if (adManager.rewardedVideo.IsLoaded())
        {
            adManager.rewardedVideo.Show();
        }
    }
}

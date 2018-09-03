using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using GameAnalyticsSDK;

public class SoundBtn : MonoBehaviour
{
    public Sprite burgundyBorderPref;
    public Sprite blueBorderPref;
    public TextMeshProUGUI soundTxt;
    public Material blue;
    public Material red;
    public Tweener widener;

    private AudioManager AudioManagerScript;

    void Awake()
    {
        AudioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (AudioManagerScript.muted)
        {
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            soundTxt.text = "OFF";
            soundTxt.fontSharedMaterial = red;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            soundTxt.text = "ON";
            soundTxt.fontSharedMaterial = blue;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
        }
    }

    public void SoundBtnAction()
    {
        if (widener != null)
        {
            DOTween.Kill(widener);
        }
        if (AudioManagerScript.muted)
        {
            AudioManagerScript.MuteSounds(false);
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            soundTxt.text = "ON";
            soundTxt.fontSharedMaterial = blue;
            widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 250, 200, 0.2f).SetEase(Ease.OutQuart);
            GameAnalytics.NewDesignEvent("Button:Sound:Unmute");
        }
        else
        {
            AudioManagerScript.MuteSounds(true);
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            soundTxt.text = "OFF";
            soundTxt.fontSharedMaterial = red;
            widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 200, 250, 0.2f).SetEase(Ease.OutQuart);
            GameAnalytics.NewDesignEvent("Button:Sound:Mute");
        }
    }
}

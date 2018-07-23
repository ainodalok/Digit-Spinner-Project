using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MuteBtn : MonoBehaviour {
    public Sprite burgundyBorderPref;
    public Sprite blueBorderPref;
    public TextMeshProUGUI muteTxt;
    public Material blue;
    public Material red;
    public Tweener widener;

    private AudioManager AudioManagerScript;

    void Awake()
    {
        AudioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        GetComponent<Button>().onClick.AddListener(() => MuteBtnAction());
    }

    void Start()
    {
        if (AudioManagerScript.muted)
        {
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            muteTxt.text = "Unmute";
            muteTxt.fontSharedMaterial = red;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 335);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            muteTxt.text = "Mute";
            muteTxt.fontSharedMaterial = blue;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
        }
    }

    public void MuteBtnAction()
    {
        if (widener != null)
        {
            DOTween.Kill(widener);
        }
        if (AudioManagerScript.muted)
        {
            AudioManagerScript.MuteSounds(false);
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            muteTxt.text = "Mute";
            muteTxt.fontSharedMaterial = blue;
            widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 335, 250, 0.2f).SetEase(Ease.OutQuart);
        }
        else
        {
            AudioManagerScript.MuteSounds(true);
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            muteTxt.text = "Unmute";
            muteTxt.fontSharedMaterial = red;
            widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 250, 335, 0.2f).SetEase(Ease.OutQuart);
        }
    }
}

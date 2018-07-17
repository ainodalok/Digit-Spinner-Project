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

    private AudioManager AudioManagerScript;
    private bool toggling = false;

    void Awake()
    {
        AudioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (AudioManagerScript.muted)
        {
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            muteTxt.text = "Unmute";
            muteTxt.fontMaterial = red;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 315);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            muteTxt.text = "Mute";
            muteTxt.fontMaterial = blue;
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
        }
    }

    public void MuteBtnWrapper()
    {
        if (toggling)
        {
            return;
        }
        StartCoroutine(AnimateMuteBtn());
    }

    private IEnumerator AnimateMuteBtn()
    {
        toggling = true;
        if (AudioManagerScript.muted)
        {
            gameObject.GetComponent<Image>().sprite = blueBorderPref;
            muteTxt.text = "Mute";
            muteTxt.fontMaterial = blue;
            Tweener widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 315, 250, 0.2f).SetEase(Ease.OutQuart);
            yield return widener.WaitForCompletion();
            AudioManagerScript.MuteSounds();
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = burgundyBorderPref;
            muteTxt.text = "Unmute";
            muteTxt.fontMaterial = red;
            Tweener widener = DOTween.To(x => transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 250, 315, 0.2f).SetEase(Ease.OutQuart);
            yield return widener.WaitForCompletion();
            AudioManagerScript.MuteSounds();
        }
        toggling = false;
    }
}

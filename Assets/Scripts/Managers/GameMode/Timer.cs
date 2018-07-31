using System.Collections;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Timer : ObjectiveTracker {
    //public int time = 1200;
    [HideInInspector]
    public bool playing = false;

    private Coroutine counter;
    private const int INITIAL_TIME = 1200;

    void Start()
    {
        SafeMemory.SetInt("time", INITIAL_TIME);
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", INITIAL_TIME / 10, INITIAL_TIME % 10);
    }

    public void StartTimer()
    {
        if (playing)
        {
            return;
        }
        playing = true;
        counter = StartCoroutine(MsCounter());
    }

    public void StopTimer()
    {
        if (!playing)
        {
            return;
        }
        playing = false;
        StopCoroutine(counter);
    }

    public void SetEnableTimer(bool enabled)
    {
        if (enabled)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }

    private IEnumerator MsCounter()
    {
        while (SafeMemory.GetInt("time") > 0)
        {
            yield return new WaitForSeconds(0.1f);
            SafeMemory.SetInt("time", SafeMemory.GetInt("time") - 1);
            //time--;
            gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", SafeMemory.GetInt("time") / 10, SafeMemory.GetInt("time") % 10);
            if (SafeMemory.GetInt("time") == 200)
            {
                gameObject.GetComponent<TextMeshProUGUI>().fontMaterial = red;
            }
            if (SafeMemory.GetInt("time") == 100)
            {
                StartShakeTween();
            }
        }

        StartCoroutine(EndGame());
    }

    private void StartShakeTween()
    {
        if (SafeMemory.GetInt("time") > 0)
        { 
            transform.GetComponent<RectTransform>().DOShakeAnchorPos(0.1f, 5, 50, 90, false, false).OnComplete(StartShakeTween);
        }
    }
}

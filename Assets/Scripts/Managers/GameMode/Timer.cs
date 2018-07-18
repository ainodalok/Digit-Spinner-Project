using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : ObjectiveTracker {
    [HideInInspector]
    public int time = 1200;
    [HideInInspector]
    public bool playing = false;

    private Coroutine counter;

    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", time / 10, time % 10);
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
        while (time > 0)
        {
            yield return new WaitForSeconds(0.1f);
            time--;
            gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", time / 10, time % 10);            
        }

        StartCoroutine(EndGame());
    }
}

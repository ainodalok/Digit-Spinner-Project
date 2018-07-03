using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : ObjectiveTracker {
    private int time;
    private bool paused = false;
    private Coroutine counter;

    void Start()
    {
        StartTimer();
        bc = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardController>();
    }

    private void StartTimer()
    {
        time = 1200;
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", time / 10, time % 10);
        counter = StartCoroutine(MsCounter());
    }

    private void StopTimer()
    {
        StopCoroutine(counter);
    }

    public void PauseTimer()
    {
        if (paused)
        {
            paused = false;
            counter = StartCoroutine(MsCounter());
        }
        else
        {
            paused = true;
            StopTimer();
        }
    }

    private IEnumerator MsCounter()
    {
        while (time >= 0)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("Time left:\n{0}.{1}", time / 10, time % 10);
            time--;
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(EndGame());
    }
}

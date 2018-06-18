using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    private int time;
    private bool paused = false;
    private Coroutine counter;
    private bool finished = false;

    void Update()
    {
        if (time <= 0 && !finished)
        {
            MenuOpener menuOpener = transform.GetComponentInParent(typeof(MenuOpener)) as MenuOpener;
            if (menuOpener.open == false)
            {
                finished = true;
                menuOpener.ToggleMenu();
                menuOpener.transform.Find("GamePanel").Find("Footer").Find("MenuBtn").gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        time = 600;
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
    }
}

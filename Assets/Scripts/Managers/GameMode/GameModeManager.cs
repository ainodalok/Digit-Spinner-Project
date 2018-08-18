using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode
{
    None,
    TimeAttack,
    LimitedTurns,
    Tutorial
};

public class GameModeManager : MonoBehaviour {
    public static GameMode mode = GameMode.None;
    public ObjectiveTracker tracker;

    //Tutorial
    public GameObject TutorialPanel;
    public GameObject TutorialTxt;
    [HideInInspector] [System.NonSerialized]
    public bool tutorialShown = false;

    public static int[][] tutorialActive1 =
    {
        new int[] {9,9,9,9,9,1,9},
        new int[] {9,9,9,9,9,2,9},
        new int[] {9,9,9,9,9,5,3},
        new int[] {9,9,9,9,9,6,4},
        new int[] {9,9,9,9,9,9,7},
        new int[] {9,9,9,9,9,9,8},
        new int[] {9,9,9,9,9,9,9}
    };

    // Use this for initialization
    void Awake () {
        mode = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().currentGameMode;
        UpdateSceneForMode();
	}

    private void UpdateSceneForMode()
    {
        if (mode == GameMode.TimeAttack)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<Timer>();
        }
        else if (mode == GameMode.LimitedTurns)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<TurnCounter>();
        }
        else if (mode == GameMode.Tutorial)
        {
            tracker = gameObject.transform.GetChild(1).gameObject.GetComponent<SectionCounter>();
        }

        tracker.enabled = true;
    }

    public void TimerPauseSafe(bool enabled)
    {
        if (mode == GameMode.TimeAttack)
        {
            if (SafeMemory.GetInt("time") > 0)
            {
                (tracker as Timer).SetEnableTimer(!enabled);
            }
        }
    }

    public void ShowTutorialMessage(bool enable)
    {
        if (mode == GameMode.Tutorial)
        {
            if (enable && !tutorialShown)
            {
                switch ((tracker as SectionCounter).sectionCurrent)
                {
                    case 1:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 1";
                        break;
                    case 2:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 2";
                        break;
                    case 3:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 3";
                        break;
                    case 4:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 4";
                        break;
                    case 5:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 5";
                        break;
                    case 6:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 6";
                        break;
                    case 7:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 7";
                        break;
                    case 8:
                        TutorialTxt.GetComponent<TextMeshProUGUI>().text = "I am Section 8";
                        break;
                }
            }
            TutorialPanel.SetActive(enable);
        }
    }
}

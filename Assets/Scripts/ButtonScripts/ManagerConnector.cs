using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManagerConnector : MonoBehaviour
{
    private SceneLoadManager LoaderScript;
    private Button button;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        button = GetComponent<Button>();
        if (button.name == "TimeAttackBtn")
        {
            button.onClick.AddListener(() => gameObject.GetComponent<TimeAttackBtn>().TimeAttackBtnWrapper());
        }
        else if (button.name == "LimitedTurnsBtn")
        {
            button.onClick.AddListener(() => gameObject.GetComponent<LimitedTurnsBtn>().LimitedTurnsBtnWrapper());
        }
        else if (button.name == "RestartBtn")
        {
            button.onClick.AddListener(() => gameObject.GetComponent<RestartBtn>().RestartBtnWrapper());
        }
        else if (button.name == "MainMenuBtn")
        {
            button.onClick.AddListener(() => gameObject.GetComponent<MainMenuBtn>().MainMenuBtnWrapper());
        }
        else if (button.name == "MuteBtn")
        {
            button.onClick.AddListener(() => gameObject.GetComponent<MuteBtn>().MuteBtnAction());
        }
    }
}

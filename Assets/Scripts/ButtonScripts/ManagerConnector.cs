using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManagerConnector : MonoBehaviour
{
    void Awake()
    {
        SceneLoadManager LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        AudioManager AudioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        Button button = GetComponent<Button>();
        if (button.name == "TimeAttackBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game", GameMode.TimeAttack));
        }
        if (button.name == "LimitedTurnsBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game", GameMode.LimitedTurns));
        }
        if (button.name == "RestartBtn")
        {
            button.onClick.AddListener(() => LoaderScript.ReloadScene());
        }
        if (button.name == "MainMenuBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Menu"));
        }
        if (button.name == "MuteBtn")
        {
            button.onClick.AddListener(() => AudioManagerScript.MuteSounds());
        }
    }
}

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
        else if (button.name == "LimitedTurnsBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game", GameMode.LimitedTurns));
        }
        else if (button.name == "RestartBtn")
        {
            button.onClick.AddListener(() => LoaderScript.ReloadScene());
        }
        else if (button.name == "MainMenuBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Menu"));
        }
        else if (button.name == "MuteBtn")
        {
            button.onClick.AddListener(() => AudioManagerScript.MuteSounds());
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManagerConnector : MonoBehaviour
{
    public MenuOpener menuOpener;

    private SceneLoadManager LoaderScript;
    private AudioManager AudioManagerScript;
    private Button button;

    void Awake()
    {
        LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        AudioManagerScript = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        button = GetComponent<Button>();
        if (button.name == "TimeAttackBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game", GameMode.TimeAttack));
        }
        else if (button.name == "LimitedTurnsBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game", GameMode.LimitedTurns));
        }
        else if (button.name == "RestartBtn")
        {
            button.onClick.AddListener(() => RestartBtnWrapper());
        }
        else if (button.name == "MainMenuBtn")
        {
            button.onClick.AddListener(() => MainMenuBtnWrapper());
        }
        else if (button.name == "MuteBtn")
        {
            button.onClick.AddListener(() => AudioManagerScript.MuteSounds());
        }
    }

    private void RestartBtnWrapper()
    {
        StartCoroutine(RestartBtn());
    }

    private IEnumerator RestartBtn()
    {
        yield return StartCoroutine(menuOpener.ToggleMenu());
        LoaderScript.ReloadScene();
    }

    private void MainMenuBtnWrapper()
    {
        StartCoroutine(RestartBtn());
    }

    private IEnumerator MainMenuBtn()
    {
        yield return StartCoroutine(menuOpener.ToggleMenu());
        LoaderScript.WrapLoadCoroutine("Menu");
    }

    private void MuteBtnWrapper()
    {
        AudioManagerScript.MuteSounds();
    }
}
 */

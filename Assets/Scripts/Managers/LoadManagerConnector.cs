using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadManagerConnector : MonoBehaviour {

    void Awake()
    {
        SceneLoadManager LoaderScript = GameObject.FindWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();
        Button button = GetComponent<Button>();
        if (button.name == "TimeAttackBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Game"));
        }
        if (button.name == "RestartBtn")
        {
            button.onClick.AddListener(() => LoaderScript.ReloadScene());
        }
        if (button.name == "MainMenuBtn")
        {
            button.onClick.AddListener(() => LoaderScript.WrapLoadCoroutine("Menu"));
        }
    }
}

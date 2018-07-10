﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using DG.Tweening;

public class SceneLoadManager : MonoBehaviour
{
    public AudioManager audioManager;
    public AdManager adManager;

    [HideInInspector]
    public string currentScene = "";
    [HideInInspector]
    public GameMode currentGameMode = GameMode.None;

    private bool loading = false;
    private bool viewportBanner = false;


    void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        Input.multiTouchEnabled = false;
        if (SceneManager.sceneCount < 2)
        {
            StartCoroutine(LoadScene("Menu"));
        }
        else
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    void Update()
    {
        if (!adManager.banner.IsShown() && adManager.banner.IsLoaded())
        {
            adManager.banner.Show();
        }
        CheckViewport();
    }

    public void WrapLoadCoroutine(string sceneName, GameMode gameMode = GameMode.None)
    {
        currentGameMode = gameMode;

        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(sceneName, gameMode));
        }
    }

    // Load a scene with a specified string name
    IEnumerator LoadScene(string sceneName, GameMode gameMode = GameMode.None)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        audioManager.pausedBGM = false;
        if (currentScene != "")
        {
            DeactivateCamerasInCurrentScene();
            StopAudioInCurrentScene();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        if (currentScene != "")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = sceneName;
        viewportBanner = false;
        CheckViewport();
        ActivateCamerasInLastScene();
#if !UNITY_EDITOR
        if (adManager.interstitial.IsLoaded() && currentScene != "")
        {
            GL.Clear(false, true, new Color (0.0f, 0.0f, 0.0f, 1.0f));
            adManager.interstitial.Show();
            while (adManager.interstitial.IsShown())
            {
                yield return null;
            }
        }
#endif
        loading = false;
    }

    private void StopAudioInCurrentScene()
    {
        if (currentScene == "Menu")
        {
            if (audioManager.IsPlayingBGM() || audioManager.pausedBGM)
            {
                audioManager.sounds[audioManager.menuBGM[audioManager.currentMenuBGMIndex]].source.Stop();
            }
        }
        else if (currentScene == "Game")
        {
            if (audioManager.IsPlayingBGM() || audioManager.pausedBGM)
            {
                audioManager.sounds[audioManager.gameBGM[audioManager.currentGameBGMIndex]].source.Stop();
            }
        }
    }

    private void DeactivateCamerasInCurrentScene()
    {
        GameObject[] camerasOld = Util.FindRootGameObjectsByName("Camera", currentScene);
        foreach (GameObject camera in camerasOld)
        {
            camera.SetActive(false);
        }
    }

    private void ActivateCamerasInLastScene()
    {
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.SetActive(true);
        }
    }

    private void ChangeViewportFitBanner()
    {
        GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 1.0f));
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height - adManager.banner.GetHeight());
        }
        viewportBanner = true;
    }

    private void ChangeViewportDefault()
    {
        GameObject[] camerasNew = Util.FindRootGameObjectsByName_SceneIndex("Camera", SceneManager.sceneCount - 1);
        foreach (GameObject camera in camerasNew)
        {
            camera.GetComponent<Camera>().pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
        }
        viewportBanner = false;
    }

    private void CheckViewport()
    {
        if (!viewportBanner)
        {
            if (adManager.banner.IsShown())
            {
                ChangeViewportFitBanner();
            }
            else if (adManager.banner.IsLoaded())
            {
                ChangeViewportFitBanner();
                adManager.banner.Show();
            }
        }
        else if (!(adManager.banner.IsLoaded() || adManager.banner.IsShown()))
        {
            ChangeViewportDefault();
        }
    }

    // Reload the current scene
    public void ReloadScene()
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}
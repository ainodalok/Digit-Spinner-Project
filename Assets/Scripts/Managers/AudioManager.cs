using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;
    
    [HideInInspector]
    public int[] gameBGM = { 0, 1, 2, 3, 4, 5 };
    [HideInInspector]
    public int[] menuBGM = { 6, 7, 8 };
    [HideInInspector]
    public int currentMenuBGMIndex = -1;
    [HideInInspector]
    public int currentGameBGMIndex = -1;
    [HideInInspector]
    public bool pausedBGM = false;
    [HideInInspector]
    public bool muted = false;

    void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = mixerGroup;
		}
        if (PlayerPrefs.GetInt("muted", 0) == 1)
        {
            muted = true;
        }
        StartCoroutine(CheckIfPlaying());
	}

    IEnumerator CheckIfPlaying()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if ((!IsPlayingBGM()) && (!pausedBGM) && (!muted))
            {
                PlayBGM();
            }
        }
    }

    private void PlayBGM()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if ((currentMenuBGMIndex < 0) || (currentMenuBGMIndex >= menuBGM.Length - 1))
            {
                Util.ShuffleArray(menuBGM);
                currentMenuBGMIndex = -1;
            }
            currentMenuBGMIndex++;
            sounds[menuBGM[currentMenuBGMIndex]].source.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if ((currentGameBGMIndex < 0) || (currentGameBGMIndex >= gameBGM.Length - 1))
            {
                Util.ShuffleArray(gameBGM);
                currentGameBGMIndex = -1;
            }

            currentGameBGMIndex++;
            sounds[gameBGM[currentGameBGMIndex]].source.Play();
        }
    }

    public bool IsPlayingBGM()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (((currentScene == "Menu") && ((currentMenuBGMIndex > -1) && (currentMenuBGMIndex < menuBGM.Length))) || 
            ((currentScene == "Game") && ((currentGameBGMIndex > -1) && (currentGameBGMIndex < gameBGM.Length))))
        {
            Sound s = null;
            if (currentScene == "Menu")
            {
                s = sounds[menuBGM[currentMenuBGMIndex]];
            }
            else if (currentScene == "Game")
            {
                s = sounds[gameBGM[currentGameBGMIndex]];
            }
            if (s.source.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    //For external calls by name
    public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		s.source.Play();
	}

    public void MuteSounds(bool mute)
    {
        if (mute)
        {
            PlayerPrefs.SetInt("muted", 1);
            string currentScene = SceneManager.GetActiveScene().name;
            Sound s = null;
            if (currentScene == "Menu")
            {
                s = sounds[menuBGM[currentMenuBGMIndex]];
            }
            else if (currentScene == "Game")
            {
                s = sounds[gameBGM[currentGameBGMIndex]];
            }
            s.source.Stop();
        }
        else
        {
            PlayerPrefs.SetInt("muted", 0);
        }
        muted = mute;
        PlayerPrefs.Save();
    }
}

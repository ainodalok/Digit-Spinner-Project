using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;
    
    [HideInInspector]
    public int[] gameBGM = { 0, 1, 2, 3, 4, 5 };
    public int[] menuBGM = { 6, 7, 8 };
    public int currentMenuBGMIndex = -1;
    public int currentGameBGMIndex = -1;
    public bool pausedBGM = false;

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
	}

    void Update()
    {
        if (!IsPlayingBGM() && !pausedBGM)
        {
            PlayBGM();
        }
    }

    private void PlayBGM()
    {
        if (!IsPlayingBGM())
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
            Debug.Log("Scene - " + SceneManager.GetActiveScene().name + ", currentGameBGMIndex - " + currentGameBGMIndex + ", currentMenuBGMIndex - " + currentMenuBGMIndex);
        }
    }

    private bool IsPlayingBGM()
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
}

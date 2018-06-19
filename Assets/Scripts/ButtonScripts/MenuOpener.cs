using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour {
    public bool open = false;
  
    public void ToggleMenu()
    {
        if (open)
        {
            open = false;
            transform.Find("MenuPanel").gameObject.SetActive(false);
            SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.currentGameBGMIndex].source.UnPause();
            SceneLoadManager.audioManager.pausedBGM = false;
            Util.FindRootGameObjectByName("Board").SetActive(true);
        }
        else
        {
            open = true;
            Util.FindRootGameObjectByName("Board").SetActive(false);
            SceneLoadManager.audioManager.pausedBGM = true;
            SceneLoadManager.audioManager.sounds[SceneLoadManager.audioManager.currentGameBGMIndex].source.Pause();
            transform.Find("MenuPanel").gameObject.SetActive(true);
        }
    }
}

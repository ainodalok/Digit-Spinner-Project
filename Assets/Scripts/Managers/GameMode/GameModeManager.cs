using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode
{
    None,
    TimeAttack,
    LimitedTurns
};

public class GameModeManager : MonoBehaviour {
    public GameMode mode = GameMode.None;
    public ObjectiveTracker tracker;

	// Use this for initialization
	void Start () {
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

        tracker.enabled = true;
    }
}

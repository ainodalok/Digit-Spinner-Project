using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScalingObjectController : MonoBehaviour {
    public const float fadeDuration = 0.5f;

    void Awake()
    {
        switch(gameObject.name)
        {
            case "MenuBtn":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[0];
                break;
            case "MuteBtn":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[1];
                break;
            case "ScoreTxt":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[2];
                break;
            case "ObjectiveTxt":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[3];
                break;
            case "EndGameBtn":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[4];
                break;
            case "CurrencyTxt":
                transform.localScale = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>().gamePanelScales[5];
                break;
        }
        
    }

    // Use this for initialization
    void Start () {
        if (transform.localScale != BoardController.ACTIVE_SIZE)
        {
            StartCoroutine(ScaleIn());
        }
	}

    public IEnumerator ScaleIn()
    {
        yield return gameObject.transform.DOScale(BoardController.ACTIVE_SIZE, fadeDuration)
            .SetEase(Ease.OutCubic)
            .WaitForCompletion();
    }

    public IEnumerator ScaleOut()
    {
        yield return gameObject.transform.DOScale(BoardController.SPAWN_SIZE, fadeDuration)
            .SetEase(Ease.InCubic)
            .WaitForCompletion();
    }
}

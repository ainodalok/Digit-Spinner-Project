using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ReadyStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ReadyAnimation());
	}

    private IEnumerator ReadyAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        Tween slideToCenter = transform.DOMoveX(0, 0.5f);
        slideToCenter.SetEase(Ease.OutBack);
        slideToCenter.Play();
        yield return slideToCenter.WaitForCompletion();

        yield return new WaitForSeconds(0.7f);

        Tween slideToRight = transform.DOLocalMoveX((transform.parent.GetComponent<RectTransform>().rect.width + transform.GetComponent<RectTransform>().rect.width) / 2.0f, 0.5f);
        slideToRight.SetEase(Ease.InBack);
        slideToRight.Play();
        yield return slideToRight.WaitForCompletion();

        yield return new WaitForSeconds(0.5f);

        Util.FindRootGameObjectByName_SceneIndex("Board", SceneManager.sceneCount - 1).SetActive(true);
        GameModeManager gameModeManager = GameObject.FindWithTag("Header").GetComponent<GameModeManager>();
        if (gameModeManager.mode == GameMode.TimeAttack)
        {
            (gameModeManager.tracker as Timer).StartTimer();
        }

        transform.gameObject.SetActive(false);
    }
}

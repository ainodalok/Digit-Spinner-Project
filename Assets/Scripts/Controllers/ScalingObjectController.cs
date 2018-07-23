using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScalingObjectController : MonoBehaviour {
    public const float fadeDuration = 0.5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(ScaleIn());
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

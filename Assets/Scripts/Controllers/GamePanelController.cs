using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GamePanelController : MonoBehaviour {
    // MenuBtn, MuteBtn, ScoreTxt, ObjectiveTxt, CurrencyText
    public Transform[] scalingObjects;
    public const float fadeDuration = 0.5f;

    public IEnumerator Minimize()
    {
        Sequence minimization = DOTween.Sequence();
        for (int i = 0; i < scalingObjects.Length; i++)
        {
            minimization.Join(scalingObjects[i].DOScale(BoardController.SPAWN_SIZE, fadeDuration).SetEase(Ease.OutCubic));
        }

        minimization.Play();
        yield return minimization.WaitForCompletion();
    }

    public Vector3[] GetObjectScales()
    {
        Vector3[] scales = new Vector3[scalingObjects.Length];
        for (int i = 0; i < scalingObjects.Length; i++)
        {
            scales[i] = scalingObjects[i].localScale;
        }
        return scales;
    }
}

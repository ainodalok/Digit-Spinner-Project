using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundMover : MonoBehaviour {
    public float target;
    private RectTransform rect;

    private void Awake()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }

    public IEnumerator MoveBackground()
    {
        yield return DOTween.To(
                () => rect.offsetMax.x, // this is actually the RectTransform's Right value
                (val) => rect.offsetMax = new Vector2(val, rect.offsetMax.y),
                target,
                1.0f //make sure this plus menu scaling duration is equal to rain fading times (called either fade time or duration in rain controller variables)
            )
            .SetEase(Ease.InOutCubic)
            .WaitForCompletion();
    }
}

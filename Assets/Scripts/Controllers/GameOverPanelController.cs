using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverPanelController : MonoBehaviour {

    public Tween scalingTween;
    public Coroutine animationCoroutine;
    [HideInInspector]
    public bool isShowing = false;

    public void StopAnimation()
    {
        if (scalingTween != null)
        {
            if (scalingTween.IsActive())
            {
                if (scalingTween.IsPlaying())
                {
                    scalingTween.Pause();
                }
            }
        }
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
    }

    public IEnumerator Animate()
    {
        gameObject.SetActive(true);
        ScaleUp();
        yield return scalingTween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        ScaleDown();
        yield return scalingTween.WaitForCompletion();
        gameObject.SetActive(false);
    }

    public void ScaleUp()
    {
        scalingTween = gameObject.transform.DOScale(BoardController.ACTIVE_SIZE, 0.5f).SetEase(Ease.OutCubic).Play();
    }

    public void ScaleDown()
    {
        scalingTween = gameObject.transform.DOScale(BoardController.SPAWN_SIZE, 0.5f).SetEase(Ease.InCubic).Play();
    }
}

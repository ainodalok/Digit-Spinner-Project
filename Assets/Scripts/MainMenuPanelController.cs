using UnityEngine;
using DG.Tweening;

public class MainMenuPanelController : MonoBehaviour {
    public const float fadeDuration = 0.5f;

	// Use this for initialization
	void Start () {
        gameObject.transform.DOScale(BoardController.ACTIVE_SIZE, fadeDuration)
            .SetEase(Ease.OutCubic);
	}
}

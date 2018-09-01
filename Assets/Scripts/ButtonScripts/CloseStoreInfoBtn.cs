using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CloseStoreInfoBtn : MonoBehaviour {
    public ScalingObjectController StorePanel;
    public ScalingObjectController InfoTab;

	void Awake () {
        GetComponent<Button>().onClick.AddListener(() => ReturnToStore());
    }

    private void ReturnToStore()
    {
        StartCoroutine(FadeToStore());
    }

    private IEnumerator FadeToStore()
    {
        yield return StartCoroutine(InfoTab.ScaleOut());
        yield return StartCoroutine(StorePanel.ScaleIn());
        InfoTab.gameObject.SetActive(false);
    }
}

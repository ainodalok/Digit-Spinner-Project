using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CloseStoreInfoBtn : MonoBehaviour {
    public ScalingObjectController StorePanel;
    public ScalingObjectController InfoTab;

    public void ReturnToStore()
    {
        StartCoroutine(FadeToStore());
    }

    private IEnumerator FadeToStore()
    {
        yield return StartCoroutine(InfoTab.ScaleOut());
        StorePanel.gameObject.SetActive(true);
        yield return StartCoroutine(StorePanel.ScaleIn());
        InfoTab.gameObject.SetActive(false);
    }
}

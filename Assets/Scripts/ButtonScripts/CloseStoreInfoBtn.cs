using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseStoreInfoBtn : MonoBehaviour {
    public GameObject StorePanel;
    public GameObject InfoTab;

	void Awake () {
        GetComponent<Button>().onClick.AddListener(() => ReturnToStore());
    }

    private void ReturnToStore()
    {
        InfoTab.SetActive(false);
        StorePanel.SetActive(true);
    }
}

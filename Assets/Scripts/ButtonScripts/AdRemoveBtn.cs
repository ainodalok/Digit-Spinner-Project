using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdRemoveBtn : MonoBehaviour {
    
    void Awake()
    {
        if (AdRemove.Get())
        {
            gameObject.SetActive(false);
        }
    }
}

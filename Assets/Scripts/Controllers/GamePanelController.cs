using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour {
    public ScalingObjectController[] scalingObjects;

    public void Minimize()
    {
        for (int i = 0; i < scalingObjects.Length; i++)
        {
            StartCoroutine(scalingObjects[i].ScaleOut());
        }
    }
}

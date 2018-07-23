using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour {
    public ScalingObjectController[] scalingObjects;

    public IEnumerator Minimize()
    {
        for (int i = 0; i < scalingObjects.Length - 1; i++)
        {
            StartCoroutine(scalingObjects[i].ScaleOut());
        }

        yield return StartCoroutine(scalingObjects[scalingObjects.Length - 1].ScaleOut());
    }
}

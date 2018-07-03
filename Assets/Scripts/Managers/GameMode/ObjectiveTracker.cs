using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectiveTracker : MonoBehaviour {
    public BoardController bc;

    protected IEnumerator EndGame()
    {
        while (bc.isDestroying)
        {
            yield return null;
        }

        MenuOpener menuOpener = transform.GetComponentInParent<MenuOpener>();
        menuOpener.ToggleMenu();
        menuOpener.EndGame();
    }
}

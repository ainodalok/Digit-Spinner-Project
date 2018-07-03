using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ObjectiveTracker : MonoBehaviour {
    public BoardController bc;

    protected IEnumerator EndGame()
    {
        Debug.Log(bc);
        while (bc.isDestroying)
        {
            yield return null;
        }

        MenuOpener menuOpener = transform.GetComponentInParent<MenuOpener>();
        menuOpener.ToggleMenu();
        menuOpener.EndGame();
    }
}

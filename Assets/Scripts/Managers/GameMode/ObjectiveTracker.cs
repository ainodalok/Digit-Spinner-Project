using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ObjectiveTracker : MonoBehaviour {
    public BoardController bc;
    public Material red;
    public bool gameOver = false;

    protected IEnumerator EndGame()
    {
        gameOver = true;
        while (bc.isDestroying)
        {
            yield return null;
        }

        MenuOpener menuOpener = transform.GetComponentInParent<MenuOpener>();
        menuOpener.EndGame();
        yield return StartCoroutine(menuOpener.ToggleMenu());
    }
}

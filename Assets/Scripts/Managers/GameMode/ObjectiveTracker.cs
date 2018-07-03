using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ObjectiveTracker : MonoBehaviour {
    public BoardController bc;

    protected void Start()
    {
        bc = Util.FindRootGameObjectByName_SceneIndex("Board", SceneManager.sceneCount - 1).GetComponent<BoardController>();
    }

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

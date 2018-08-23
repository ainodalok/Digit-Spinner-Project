using System.Collections;
using UnityEngine;

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
        if (GameModeManager.mode == GameMode.LimitedTurns)
        {
            Social.ReportScore(SafeMemory.GetInt("score"), "CgkI-MWprNwaEAIQAg", (bool success) =>
            {

            });
        }
        else if (GameModeManager.mode == GameMode.TimeAttack)
        {
            Social.ReportScore(SafeMemory.GetInt("score"), "CgkI-MWprNwaEAIQAw", (bool success) =>
            {

            });
        }
        MenuOpener menuOpener = transform.GetComponentInParent<MenuOpener>();
        menuOpener.EndGame();
        yield return StartCoroutine(menuOpener.ToggleMenu());
    }
}

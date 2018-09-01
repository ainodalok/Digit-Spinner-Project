using System.Collections;
using UnityEngine;

public class InGameSettingsBackBtn : MonoBehaviour
{
    public MenuOpener menuOpener;
    public ScalingObjectController settingsPanel;
    private bool isClosing = false;

    public void InGameSettingsBackBtnAction()
    {
        if (isClosing)
        {
            return;
        }

        StartCoroutine(FadeToMenu());
    }

    private IEnumerator FadeToMenu()
    {
        isClosing = true;
        yield return settingsPanel.ScaleOut();
        menuOpener.menuPanel.SetActive(true);
        yield return menuOpener.SlideToCenterAnimation();
        isClosing = false;
        settingsPanel.gameObject.SetActive(false);
    }
}

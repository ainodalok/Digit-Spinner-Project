using System.Collections;
using UnityEngine;

public class InGameSettingsBtn : MonoBehaviour
{
    public MenuOpener menuOpener;
    public ScalingObjectController settingsPanel;
    private bool isOpening = false;

    public void InGameSettingsBtnAction()
    {
        if (isOpening)
        {
            return;
        }

        StartCoroutine(FadeToSettings());
    }

    private IEnumerator FadeToSettings()
    {
        isOpening = true;
        yield return menuOpener.SlideOffScreenAnimation();
        settingsPanel.gameObject.SetActive(true);
        yield return settingsPanel.ScaleIn();
        isOpening = false;
        menuOpener.menuPanel.SetActive(false);
    }
}

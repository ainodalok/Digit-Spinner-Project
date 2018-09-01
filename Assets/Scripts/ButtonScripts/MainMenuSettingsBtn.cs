using System.Collections;
using UnityEngine;

public class MainMenuSettingsBtn : MonoBehaviour {
    public ScalingObjectController mainMenuPanel;
    public ScalingObjectController settingsPanel;
    private bool isOpening = false;

    public void MainMenuSettingsBtnAction()
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
        yield return mainMenuPanel.ScaleOut();
        settingsPanel.gameObject.SetActive(true);
        yield return settingsPanel.ScaleIn();
        mainMenuPanel.gameObject.SetActive(false);
        isOpening = false;
    }
}

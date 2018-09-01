using System.Collections;
using UnityEngine;

public class SettingsBackBtn : MonoBehaviour
{
    public ScalingObjectController mainMenuPanel;
    public ScalingObjectController settingsPanel;
    private bool isClosing = false;

    public void SettingsBackBtnAction()
    {
        if (isClosing)
        {
            return;
        }

        StartCoroutine(FadeToSettings());
    }

    private IEnumerator FadeToSettings()
    {
        isClosing = true;
        yield return settingsPanel.ScaleOut();
        mainMenuPanel.gameObject.SetActive(true);
        yield return mainMenuPanel.ScaleIn();
        settingsPanel.gameObject.SetActive(false);
        isClosing = false;
    }
}

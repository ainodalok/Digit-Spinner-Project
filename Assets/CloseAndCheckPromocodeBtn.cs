using UnityEngine;
using TMPro;
using System.Collections;

public class CloseAndCheckPromocodeBtn : MonoBehaviour
{
    public ScalingObjectController infoTab;
    public ScalingObjectController storeTab;
    public TextMeshProUGUI infoText;
    public TMP_InputField inputField;
    public GameObject usualCloseBtn;

    private const string PROMOCODE_1 = "GZ4Rne5e";
    private const string PROMOCODE_2 = "5ALrNS8c";

    public void CloseAndCheckPromocodeBtnAction()
    {
         ShowResponse();
    }

    private void ShowResponse()
    {
        if (string.CompareOrdinal(inputField.text.Trim(), PROMOCODE_1) == 0)
        {
            AdRemove.Enable();
            Currency.AddBalance(2000);
            infoText.text = "Promocode accepted. \n Thank you!";
        }
        else if (string.CompareOrdinal(inputField.text.Trim(), PROMOCODE_2) == 0)
        {
            AdRemove.Enable();
            infoText.text = "Promocode accepted. \n Thank you!";
        }
        else
        {
            infoText.text = "Invalid Promocode.";
        }

        inputField.text = "";
        inputField.gameObject.SetActive(false);
        usualCloseBtn.SetActive(true);
        gameObject.SetActive(false);
    }
}

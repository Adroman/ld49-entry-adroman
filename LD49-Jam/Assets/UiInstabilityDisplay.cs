using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInstabilityDisplay : MonoBehaviour
{
    public IntVariable MaxInstability;
    public IntVariable CurrentInstability;
    public TMP_Text TextToUpdate;
    public string Format;
    public Image FillingImage;

    public void UpdateData()
    {
        TextToUpdate.text = string.Format(Format, CurrentInstability.Value, MaxInstability.Value);
        FillingImage.fillAmount = (float)CurrentInstability.Value / MaxInstability.Value;
    }
}
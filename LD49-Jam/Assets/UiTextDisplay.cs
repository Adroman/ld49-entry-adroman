using TMPro;
using UnityEngine;

public class UiTextDisplay : MonoBehaviour
{
    public IntVariable Variable;
    public TMP_Text Text;
    public string Prefix;
    public string Postfix;

    public void UpdateText()
    {
        Text.text = $"{Prefix}{Variable.Value}{Postfix}";
    }
}
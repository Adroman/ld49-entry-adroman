using TMPro;
using UnityEngine;

public class SurvivedWavesText : MonoBehaviour
{
    public IntVariable WaveNumber;

    public TMP_Text TextToDisplay;

    public void Start()
    {
        TextToDisplay.text = $"You survived {WaveNumber.Value - 1} waves.";
    }
}
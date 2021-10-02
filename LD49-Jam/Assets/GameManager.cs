using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntVariable GoldAmount;
    public IntVariable WaveNumber;
    
    private void OnEnable()
    {
        GoldAmount.Value = 100;
        WaveNumber.Value = 0;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DifficultyConfigurator DifficultyConfigurator;
    public IntVariable GoldAmount;
    public IntVariable WaveNumber;

    public int InitialGoldAmount = 100;
    
    private void OnEnable()
    {
        ImportFromDifficulty();
        GoldAmount.Value = InitialGoldAmount;
        WaveNumber.Value = 0;
    }

    public void Greed()
    {
        GoldAmount.Value /= 2;
    }

    private void ImportFromDifficulty()
    {
        if (DifficultyConfigurator != null && DifficultyConfigurator.SelectedDifficulty != null)
        {
            var difficulty = DifficultyConfigurator.SelectedDifficulty;
            InitialGoldAmount = difficulty.InitialGold;
        }
    }
}

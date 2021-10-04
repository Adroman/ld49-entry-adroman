using System;
using UnityEngine;

[RequireComponent(typeof(WaveManager))]
public class WaveManagerDisasters : MonoBehaviour
{
    private WaveManager _waveManager;
    public DifficultyConfigurator SelectedDifficulty;

    public float HitpointIncrease;
    public float ArmorIncrease;
    public float SpeedIncrease;
    public int AmountIncrease;
    
    private void Awake()
    {
        _waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        ImportFromDifficulty();
    }

    private void ImportFromDifficulty()
    {
        if (SelectedDifficulty != null && SelectedDifficulty.SelectedDifficulty != null)
        {
            var difficulty = SelectedDifficulty.SelectedDifficulty;

            HitpointIncrease = difficulty.HitpointIncrease;
            ArmorIncrease = difficulty.ArmorIncrease;
            SpeedIncrease = difficulty.SpeedIncrease;
            AmountIncrease = difficulty.AmountIncrease;
        }
    }

    public void IncreaseHitPointsDifficulty()
    {
        _waveManager.BaseHitPointsIncrease += HitpointIncrease;
    }

    public void IncreaseArmorDifficulty()
    {
        _waveManager.BaseArmorIncrease += ArmorIncrease;
    }

    public void IncreaseSpeed()
    {
        _waveManager.FlatSpeedIncrease += SpeedIncrease;
    }

    public void IncreaseAmount()
    {
        _waveManager.FlatAmountIncrease += AmountIncrease;
    }
}
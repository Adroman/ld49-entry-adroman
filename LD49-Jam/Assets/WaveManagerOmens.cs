using System;
using UnityEngine;

[RequireComponent(typeof(WaveManager))]
public class WaveManagerOmens : MonoBehaviour
{
    private WaveManager _waveManager;
    
    private void Awake()
    {
        _waveManager = GetComponent<WaveManager>();
    }

    public void IncreaseHitPointsDifficulty(float amount)
    {
        _waveManager.BaseHitPointsIncrease += amount;
    }

    public void IncreaseArmorDifficulty(float amount)
    {
        _waveManager.BaseArmorIncrease += amount;
    }

    public void IncreaseSpeed(float amount)
    {
        _waveManager.FlatSpeedIncrease += amount;
    }

    public void IncreaseAmount(int amount)
    {
        _waveManager.FlatAmountIncrease += amount;
    }
}
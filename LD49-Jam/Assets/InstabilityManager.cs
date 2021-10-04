using System.Collections.Generic;
using UnityEngine;

public class InstabilityManager : MonoBehaviour
{
    public DifficultyConfigurator DifficultyConfigurator;
    public IntVariable MaxInstability;
    public IntVariable CurrentInstability;
    public IntVariable GoldAmount;

    public List<GameEvent> Disasters;
    public GameEvent DisasterOccured;

    public int InitialMaxInstability;
    public int MaxInstabilityIncrease;
    
    // constants
    public int TowerInstability = 10;
    public int TowerInstabilityIncrease = 5;

    public int TowerUpgradeInstability = 10;
    public int TowerUpgradeInstabilityIncrease = 5;

    public int FlowerPickupInstability = 5;
    public int FlowerLostInstability = 50;

    public int MoneyHoardingInstabilityMinimum = 200;
    public int MoneyHoardingInstabilityIncreasePerGold = 1;

    public int PauseInstability = 10;

    private void Start()
    {
        ImportFromDifficulty();
        MaxInstability.Value = InitialMaxInstability;
        CurrentInstability.Value = 0;
    }

    private void ImportFromDifficulty()
    {
        if (DifficultyConfigurator != null && DifficultyConfigurator.SelectedDifficulty != null)
        {
            var difficulty = DifficultyConfigurator.SelectedDifficulty;
            InitialMaxInstability = difficulty.InitialMaxInstability;
            MaxInstabilityIncrease = difficulty.MaxInstabilityIncrease;
            TowerInstability = difficulty.TowerInstability;
            TowerInstabilityIncrease = difficulty.TowerInstabilityIncrease;
            TowerUpgradeInstability = difficulty.TowerUpgradeInstability;
            TowerUpgradeInstabilityIncrease = difficulty.TowerUpgradeInstabilityIncrease;
            FlowerPickupInstability = difficulty.FlowerPickupInstability;
            FlowerLostInstability = difficulty.FlowerLostInstability;
            MoneyHoardingInstabilityMinimum = difficulty.MoneyHoardingInstabilityMinimum;
            MoneyHoardingInstabilityIncreasePerGold = difficulty.MoneyHoardingInstabilityIncreasePerGold;
            PauseInstability = difficulty.PauseInstability;

            foreach (var disaster in difficulty.DisastersToExclude)
            {
                Disasters.Remove(disaster);
            }

            foreach (var disaster in difficulty.DisastersToInclude)
            {
                Disasters.Add(disaster);
            }
        }
    }
    
    public void IncreaseInstability(int amount)
    {
        CurrentInstability.Value += amount;
        while (CurrentInstability.Value >= MaxInstability.Value)
        {
            CurrentInstability.Value -= MaxInstability.Value;
            MaxInstability.Value += MaxInstabilityIncrease;
            TriggerDisaster();
        }
    }

    public void TowerBuilt()
    {
        IncreaseInstability(TowerInstability);
        TowerInstability += TowerInstabilityIncrease;
    }

    public void TowerUpgraded()
    {
        IncreaseInstability(TowerUpgradeInstability);
        TowerUpgradeInstability += TowerUpgradeInstabilityIncrease;
    }

    public void FlowerPickedUp()
    {
        IncreaseInstability(FlowerPickupInstability);
    }

    public void FlowerLost()
    {
        IncreaseInstability(FlowerLostInstability);
    }

    public void MoneyHoarding()
    {
        if (GoldAmount.Value <= MoneyHoardingInstabilityMinimum) return;
        
        IncreaseInstability((GoldAmount.Value - MoneyHoardingInstabilityMinimum) * MoneyHoardingInstabilityIncreasePerGold);
        MoneyHoardingInstabilityMinimum = GoldAmount.Value;
    }

    public void GamePaused()
    {
        IncreaseInstability(PauseInstability);
    }

    private void TriggerDisaster()
    {
        if (Disasters.Count == 0) return;
        int i = Random.Range(0, Disasters.Count);
        Disasters[i].RaiseEvent();
        DisasterOccured.RaiseEvent();
    }
}

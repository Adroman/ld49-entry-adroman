using System.Collections.Generic;
using UnityEngine;

public class InstabilityManager : MonoBehaviour
{
    public IntVariable MaxInstability;
    public IntVariable CurrentInstability;
    public IntVariable GoldAmount;

    public List<GameEvent> Disasters;
    public GameEvent DisasterOccured;
    
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

    private void Start()
    {
        MaxInstability.Value = 100;
        CurrentInstability.Value = 0;
    }

    public void IncreaseInstability(int amount)
    {
        CurrentInstability.Value += amount;
        if (CurrentInstability.Value >= MaxInstability.Value)
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

    private void TriggerDisaster()
    {
        int i = Random.Range(0, Disasters.Count);
        Disasters[i].RaiseEvent();
        DisasterOccured.RaiseEvent();
    }
}

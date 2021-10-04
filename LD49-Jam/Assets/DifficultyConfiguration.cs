using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultyConfiguration : ScriptableObject
{
    // GameManager
    public int InitialGold;
    
    // WaveManager
    public float BaseHitPoints;
    public float BaseArmor;
    public int TotalLoot;
    public float BaseHitPointsIncrease;
    public float BaseArmorIncrease;
    public int FlatLootIncrease ;
    public float FlatSpeedIncrease;
    public int FlatAmountIncrease;
    
    // WaveManagerDisaster
    public float HitpointIncrease;
    public float ArmorIncrease;
    public float SpeedIncrease;
    public int AmountIncrease;
    
    // InstabilityManager
    public int TowerInstability;
    public int TowerInstabilityIncrease;
    public int TowerUpgradeInstability;
    public int TowerUpgradeInstabilityIncrease;
    public int FlowerPickupInstability;
    public int FlowerLostInstability;
    public int MoneyHoardingInstabilityMinimum ;
    public int MoneyHoardingInstabilityIncreasePerGold;
    public int PauseInstability;

    public List<GameEvent> DisastersToExclude;
    public List<GameEvent> DisastersToInclude;
    public int InitialMaxInstability;
    public int MaxInstabilityIncrease;
}
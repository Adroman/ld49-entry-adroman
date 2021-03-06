using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public DifficultyConfigurator DifficultyConfigurator;
    public WaveTemplate[] TemplatesToUse;
    public Waypoint SpawnPoint;
    public Transform EnemiesParent;
    public IntVariable WaveNumber;
    public GameEvent WaveStarted;
    public Image NextWaveMonstersUiImage;
    public TMP_Text NextWaveMonstersAmount;
    public NextWaveManager NextWaveManager;
    public Button NextWaveButton;

    public float BaseHitPoints;
    public float BaseArmor;
    public int TotalLoot;

    public float BaseHitPointsIncrease = 1;
    public float BaseArmorIncrease = 1;
    public int FlatLootIncrease = 10;
    public float FlatSpeedIncrease = 0;
    public int FlatAmountIncrease = 0;

    private Wave _nextWave;
    private bool _spawning;
    private int _idCounter = 0;

    public void Start()
    {
        ImportFromDifficulty();
        _nextWave = GenerateWave(1);
    }

    public void StartWave()
    {
        WaveStarted.RaiseEvent();
        WaveNumber.Value++;
        NextWaveButton.gameObject.SetActive(false);
        StartCoroutine(SpawnMonsters(_nextWave));
        
        BaseHitPoints *= BaseHitPointsIncrease;
        BaseArmor *= BaseArmorIncrease;
        TotalLoot += FlatLootIncrease;
        
        _nextWave = GenerateWave(WaveNumber.Value + 1);
    }

    private void ImportFromDifficulty()
    {
        if (DifficultyConfigurator != null && DifficultyConfigurator.SelectedDifficulty != null)
        {
            var difficulty = DifficultyConfigurator.SelectedDifficulty;
            BaseHitPoints = difficulty.BaseHitPoints;
            BaseArmor = difficulty.BaseArmor;
            TotalLoot = difficulty.TotalLoot;

            BaseHitPointsIncrease = difficulty.BaseHitPointsIncrease;
            BaseArmorIncrease = difficulty.BaseArmorIncrease;
            FlatLootIncrease = difficulty.FlatLootIncrease;
            FlatSpeedIncrease = difficulty.FlatSpeedIncrease;
            FlatAmountIncrease = difficulty.FlatAmountIncrease;
        }
    }

    public void EnemiesCleared()
    {
        if (!_spawning)
        {
            NextWaveButton.gameObject.SetActive(true);
        }
    }
    
    private Wave GenerateWave(int waveNumber)
    {
        var templatesToUse = TemplatesToUse.Where(t => t.MinimumWave <= waveNumber).ToArray();
        var selectedTemplate = templatesToUse[Random.Range(0, templatesToUse.Length)];
        var wave = new Wave
        {
            Amount = selectedTemplate.Amount + FlatAmountIncrease,
            EnemyToUse = selectedTemplate.PrefabToUse,
            Interval = selectedTemplate.Interval,
            HitPoints = BaseHitPoints * selectedTemplate.HitPointsModifier,
            Armor = BaseArmor * selectedTemplate.ArmorModifier,
            Speed = selectedTemplate.Speed + FlatSpeedIncrease,
            Loot = TotalLoot / (selectedTemplate.Amount + FlatAmountIncrease)
        };

        NextWaveMonstersUiImage.sprite = wave.EnemyToUse.PreviewImage.sprite;
        NextWaveMonstersUiImage.color = wave.EnemyToUse.PreviewImage.color;
        NextWaveMonstersAmount.text = $"{wave.Amount} x";
        NextWaveManager.NextWave = wave;
        return wave;
    }

    private IEnumerator SpawnMonsters(Wave wave)
    {
        _spawning = true;
        int amountLeft = wave.Amount;
        while (amountLeft > 0)
        {
            SpawnEnemy(wave);
            if (--amountLeft == 0)
            {
                _spawning = false;
                yield break;
            }
            
            yield return new WaitForSeconds(wave.Interval);
        }
        _spawning = false;
    }

    private Enemy SpawnEnemy(Wave wave)
    {
        var enemy = Instantiate(wave.EnemyToUse, SpawnPoint.transform.position, Quaternion.identity, EnemiesParent);
        enemy.Id = _idCounter++;
        
        if (SpawnPoint.Flowers.Count > 0)
        {
            var targetFlower = enemy.FindFlower(SpawnPoint);
            enemy.NextWaypoint = SpawnPoint;
            Debug.Log($"Enemy {enemy.Id} found a flower {targetFlower.Id} at waypoint {SpawnPoint.Index}.");
        }
        else
        {
            enemy.NextWaypoint = SpawnPoint.GetNextWaypoint();
            Debug.Log($"Enemy {enemy.Id} next waypoint is {enemy.NextWaypoint.Index}.");
        }
        
        enemy.MaximumHealth = wave.HitPoints;
        enemy.Health = wave.HitPoints;
        enemy.Armor = wave.Armor;
        enemy.Speed = wave.Speed;
        enemy.Loot = wave.Loot;

        return enemy;
    }
}

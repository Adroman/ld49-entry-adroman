using UnityEngine;

[CreateAssetMenu]
public class WaveTemplate : ScriptableObject
{
    public int MinimumWave = 1;
    public Enemy PrefabToUse;
    public float Interval = 1;
    public int Amount = 10;
    
    public float HitPointsModifier = 1;
    public float ArmorModifier = 1;
    public float Speed = 3;
}
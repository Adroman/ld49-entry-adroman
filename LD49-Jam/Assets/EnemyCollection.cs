using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyCollection : ScriptableObject
{
    public List<Enemy> Enemies = new List<Enemy>();

    public GameEvent CollectionEmpty;

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        if (Enemies.Count == 0 && CollectionEmpty != null)
        {
            CollectionEmpty.RaiseEvent();
        }
    }
}
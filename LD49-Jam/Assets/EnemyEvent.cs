using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyEvent : ScriptableObject
{
    public List<EnemyEventListener> Listeners = new List<EnemyEventListener>();

    public void AddListener(EnemyEventListener listener) => Listeners.Add(listener);

    public bool RemoveListener(EnemyEventListener listener) => Listeners.Remove(listener);

    public void RaiseEvent(Enemy enemy)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].Respond(enemy);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> Listeners = new List<GameEventListener>();

    public void AddListener(GameEventListener listener) => Listeners.Add(listener);

    public bool RemoveListener(GameEventListener listener) => Listeners.Remove(listener);

    public void RaiseEvent()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].Respond();
        }
    }
}
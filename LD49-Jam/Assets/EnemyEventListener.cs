using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyEventListener : MonoBehaviour
{
    public EnemyEvent Event;
    public EnemyUnityEvent Response;

    public void OnEnable() => Event.AddListener(this);

    public void OnDisable() => Event.RemoveListener(this);

    public void Respond(Enemy enemy) => Response.Invoke(enemy);
}

[Serializable]
public class EnemyUnityEvent : UnityEvent<Enemy>
{
}
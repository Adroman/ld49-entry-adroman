using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    public void OnEnable() => Event.AddListener(this);

    public void OnDisable() => Event.RemoveListener(this);

    public void Respond() => Response.Invoke();
}
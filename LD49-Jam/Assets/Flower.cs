using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flower : MonoBehaviour
{
    public GameEvent FlowerPickedUp;
    public FlowerCollection FlowerCollection;
    public int Id;
    
    private SpriteRenderer _spriteRenderer;

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        FlowerCollection.AddFlower(this);
    }

    public void OnDisable()
    {
        FlowerCollection.RemoveFlower(this);
    }

    public void OnPickup()
    {
        _spriteRenderer.sortingLayerName = SortingLayers.EnemiesCarryingObjects;
        FlowerPickedUp.RaiseEvent();
    }

    public void OnDrop()
    {
        _spriteRenderer.sortingLayerName = SortingLayers.Objects;
    }
}
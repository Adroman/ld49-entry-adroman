using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FlowerCollection : ScriptableObject
{
    public List<Flower> Flowers = new List<Flower>();

    public GameEvent CollectionEmpty;

    public void AddFlower(Flower flower)
    {
        Flowers.Add(flower);
    }

    public void RemoveFlower(Flower flower)
    {
        Flowers.Remove(flower);
        if (Flowers.Count == 0 && CollectionEmpty != null)
        {
            CollectionEmpty.RaiseEvent();
        }
    }
}
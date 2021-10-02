using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileCollection : ScriptableObject
{
    public List<Tile> Tiles = new List<Tile>();

    public GameEvent CollectionEmpty;

    public void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }

    public void RemoveTile(Tile tile)
    {
        Tiles.Remove(tile);
        if (Tiles.Count == 0 && CollectionEmpty != null)
        {
            CollectionEmpty.RaiseEvent();
        }
    }
}
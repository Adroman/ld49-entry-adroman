using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeManager : MonoBehaviour
{
    public TileCollection Tiles;
    public GameObject DamagedTilePrefab;

    public int TilesToDamage = 5;
    public int TilesDamageIncrease = 1;
    
    public void EarthquakeTriggered()
    {
        var tiles = Tiles.Tiles;
        if (tiles.Count <= TilesToDamage)
        {
            while (tiles.Count > 0)
            {
                DestroyTile(tiles[0]);
            }
        }
        else
        {
            int tilesToDestroy = TilesToDamage;
            TilesToDamage += TilesDamageIncrease;
            while (tilesToDestroy > 0)
            {
                DestroyTile(tiles[Random.Range(0, tiles.Count)]);
                tilesToDestroy--;
            }
        }
    }

    private void DestroyTile(Tile tile)
    {
        var t = tile.transform;
        Instantiate(DamagedTilePrefab, t.position, t.rotation, t.parent);
        Destroy(tile.gameObject);
    }
}

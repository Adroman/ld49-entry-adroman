using System.Collections;
using System.Linq;
using UnityEngine;

public class FlowersManager : MonoBehaviour
{
    public GameEvent FlowerRitual;
    public InstabilityManager InstabilityManager;
    public FlowerCollection Flowers;
    public WaypointManager WaypointManager;

    public void PerformFlowerRitual()
    {
        var waypointsWithFlowers = WaypointManager.Waypoints.Where(w => w.Flowers.Count > 0).ToList();
        
        if (waypointsWithFlowers.Count == 0)
        {
            waypointsWithFlowers = WaypointManager.gameObject.GetComponentsInChildren<Waypoint>().ToList();
        }
        
        Debug.Log($"Waypoints with flower: {waypointsWithFlowers.Count}");
        var flowersToPickFrom = waypointsWithFlowers.SelectMany(w => w.Flowers).ToList();
        Debug.Log($"Total flowers: {flowersToPickFrom.Count}");

        if (flowersToPickFrom.Count == 0)
        {
            Debug.Log($"This is not supposed to happen");
            return;
        }
        var flowerToSacrifice = flowersToPickFrom[Random.Range(0, flowersToPickFrom.Count)];

        Debug.Log($"Sacrificing flower: {flowerToSacrifice.Id}");
        
        foreach (var waypoint in waypointsWithFlowers)
        {
            waypoint.Flowers.Remove(flowerToSacrifice);
        }

        if (Flowers.Flowers.Count <= 2)
        {
            // last flower will be taken by monsters
            InstabilityManager.Disasters.Remove(FlowerRitual);
        }

        StartCoroutine(TakeFlower(flowerToSacrifice));
    }

    private IEnumerator TakeFlower(Flower flower)
    {
        var tr = flower.transform;
        
        while (tr.localScale.x > 0)
        {
            var localScale = tr.localScale;
            tr.localScale = new Vector3(localScale.x - Time.deltaTime, localScale.y - Time.deltaTime, localScale.z - Time.deltaTime);
            yield return null;
        }
        Destroy(flower.gameObject);
    }
}
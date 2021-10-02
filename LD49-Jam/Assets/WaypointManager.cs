using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Waypoint> Waypoints;

    public void OnEnable()
    {
        IndexWaves();
    }

    private void IndexWaves()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            var waypoint = Waypoints[i];
            waypoint.WaypointManager = this;
            waypoint.Index = i;
        }
    }
}

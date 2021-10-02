using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int Index;

    public WaypointManager WaypointManager;

    public Waypoint GetNextWaypoint()
    {
        return 
            Index < WaypointManager.Waypoints.Count - 1 
                ? WaypointManager.Waypoints[Index + 1]    // in bounds
                : null;                                   // last waypoint
    }

    public Waypoint GetPreviousWaypoint()
    {
        return
            Index > 0
                ? WaypointManager.Waypoints[Index - 1]    // in bounds
                : null;                                   // first waypoints
    }

    public List<Flower> Flowers;
}
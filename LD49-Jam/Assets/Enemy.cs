using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public float MaximumHealth;
    public float Health;
    public float Armor;
    public float Speed;

    public int Loot;

    public Transform Sprite;
    public float AngleOffset;
    
    public WaypointManager WaypointManager;
    public Waypoint NextWaypoint;
    public TravelMethod Direction;
    public Sprite PreviewImage;

    public EnemyCollection EnemyCollection;
    public IntVariable GoldVariable;
    public EnemyEvent EnemyDied;
    public EnemyHealth EnemyHealth;
    public GameEvent OnFlowerLost;
    
    private Flower _targetFlower;
    private Flower _carryingFlower;

    public void OnEnable()
    {
        EnemyCollection.AddEnemy(this);
    }

    public void OnDisable()
    {
        EnemyCollection.RemoveEnemy(this);
    }

    public void Start()
    {
        WaypointManager = NextWaypoint.WaypointManager;
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        var target = GetTarget();
        var direction = target.position - transform.position;

        RotateSprite(direction);

        float sqrDistanceLeft = direction.sqrMagnitude;
        float travelDistance = Speed * Time.deltaTime;
        bool targetReached = sqrDistanceLeft < travelDistance * travelDistance;

        transform.Translate(direction.normalized * travelDistance, Space.World);
        if (_carryingFlower != null)
        {
            _carryingFlower.transform.position = transform.position;
        }

        if (targetReached)
        {
            TargetReached();
        }
    }

    private void RotateSprite(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        float rotatingAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Sprite.rotation = Quaternion.AngleAxis(rotatingAngle + AngleOffset, Vector3.forward);
    }

    private Transform GetTarget()
    {
        return _targetFlower != null ? _targetFlower.transform : NextWaypoint.transform;
    }

    private void TargetReached()
    {
        // flower reached
        if (_targetFlower != null)
        {
            _carryingFlower = _targetFlower;
            _carryingFlower.OnPickup();
            _targetFlower = null;
            Direction = TravelMethod.Entrance;
            
            // NextWaypoint should be the next one
        }
        else
        {
            // waypoint reached
            if (Direction == TravelMethod.Goal)
            {
                // travelling towards goal
                if (_carryingFlower == null && NextWaypoint.Flowers.Count > 0)
                {
                    FindFlower(NextWaypoint);
                }
                else
                {
                    // no flower found or already carrying flower, go to next waypoint
                    var nextWaypoint = NextWaypoint.GetNextWaypoint();
                    if (nextWaypoint == null)
                    {
                        // no next waypoint, turn around
                        Direction = TravelMethod.Entrance;
                        nextWaypoint = NextWaypoint.GetPreviousWaypoint();
                    }

                    NextWaypoint = nextWaypoint;
                }
                
            }
            else
            {
                // travelling towards entrance
                {
                    var nextWaypoint = NextWaypoint.GetPreviousWaypoint();
                    if (nextWaypoint == null)
                    {
                        // we reached the entrance
                        Direction = TravelMethod.Goal;
                        NextWaypoint = NextWaypoint.GetNextWaypoint();

                        if (_carryingFlower != null)
                        {
                            FlowerLost();
                        }
                    }
                    else
                    {
                        NextWaypoint = nextWaypoint;
                        
                        // there is a next waypoint
                        if (_carryingFlower == null && nextWaypoint.Flowers.Count > 0)
                        {
                            // it has a flower
                            FindFlower(nextWaypoint);
                        }
                    }
                }
            }
        }
    }

    private void FindFlower(Waypoint waypointToUse)
    {
        var nearestFlower = waypointToUse.Flowers
            .OrderBy(f => (f.transform.position - transform.position).sqrMagnitude).First();

        waypointToUse.Flowers.Remove(nearestFlower);
        _targetFlower = nearestFlower;
    }

    private void FlowerLost()
    {
        Destroy(_carryingFlower.gameObject);
        OnFlowerLost.RaiseEvent();
    }

    public void Die()
    {
        GoldVariable.Value += Loot;
        EnemyDied.RaiseEvent(this);
        if (_carryingFlower != null)
        {
            _carryingFlower.OnDrop();
            NextWaypoint.Flowers.Add(_carryingFlower);
        }

        if (_targetFlower != null)
        {
            var waypoint = Direction == TravelMethod.Entrance 
                ? NextWaypoint 
                : NextWaypoint.GetPreviousWaypoint();
            waypoint.Flowers.Add(_targetFlower);
        }
        Destroy(gameObject);
    }
}
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
    public SpriteRenderer PreviewImage;

    public EnemyCollection EnemyCollection;
    public IntVariable GoldVariable;
    public EnemyEvent EnemyDied;
    public EnemyHealth EnemyHealth;
    public GameEvent OnFlowerLost;
    public AudioClip[] Cries;

    public int Id;
    
    private Flower _targetFlower;
    private Flower _carryingFlower;
    public bool CarriesFlower => _carryingFlower != null;

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
        Debug.Log($"Enemy {Id} reached its target.");
        // flower reached
        if (_targetFlower != null)
        {
            Debug.Log($"Enemy {Id} picked the flower {_targetFlower.Id}.");
            _carryingFlower = _targetFlower;
            _carryingFlower.OnPickup();
            _targetFlower = null;

            if (Direction == TravelMethod.Goal)
            {
                Direction = TravelMethod.Entrance;
            }
            Debug.Log($"Enemy {Id} next target is {NextWaypoint.Index}.");
        }
        else
        {
            Debug.Log($"Enemy {Id} reached its waypoint {NextWaypoint.Index}.");
            // waypoint reached
            if (Direction == TravelMethod.Goal)
            {
                // travelling towards goal
                if (_carryingFlower == null && NextWaypoint.Flowers.Count > 0)
                {
                    FindFlower(NextWaypoint);
                    Debug.Log($"Enemy {Id} found a flower {_targetFlower.Id} at waypoint {NextWaypoint.Index}.");
                }
                else
                {
                    Debug.Log($"Enemy {Id} found  no flower at waypoint {NextWaypoint.Index}.");
                    // no flower found or already carrying flower, go to next waypoint
                    var nextWaypoint = NextWaypoint.GetNextWaypoint();
                    if (nextWaypoint == null)
                    {
                        // no next waypoint, turn around
                        Direction = TravelMethod.Entrance;
                        nextWaypoint = NextWaypoint.GetPreviousWaypoint();
                    }

                    NextWaypoint = nextWaypoint;
                    Debug.Log($"Enemy {Id} next waypoint is {NextWaypoint.Index}.");
                }
                
            }
            else
            {
                // travelling towards entrance
                {
                    var nextWaypoint = NextWaypoint.GetPreviousWaypoint();
                    if (nextWaypoint == null)
                    {
                        Debug.Log($"Enemy {Id} reached the entrance.");
                        // we reached the entrance
                        Direction = TravelMethod.Goal;
                        NextWaypoint = NextWaypoint.GetNextWaypoint();

                        if (_carryingFlower != null)
                        {
                            Debug.Log($"Enemy {Id} secured the flower {_carryingFlower.Id}.");
                            FlowerLost();
                        }
                    }
                    else
                    {
                        NextWaypoint = nextWaypoint;
                        Debug.Log($"Enemy {Id} next waypoint is {NextWaypoint.Index}.");
                        
                        // there is a next waypoint
                        if (_carryingFlower == null && nextWaypoint.Flowers.Count > 0)
                        {
                            // it has a flower
                            FindFlower(nextWaypoint);
                            Debug.Log($"Enemy {Id} found a flower {_targetFlower.Id} at waypoint {NextWaypoint.Index}.");
                        }
                    }
                }
            }
        }
    }

    public Flower FindFlower(Waypoint waypointToUse)
    {
        waypointToUse.Flowers.RemoveAll(f => f == null);
        
        var nearestFlower = waypointToUse.Flowers
            .OrderBy(f => (f.transform.position - transform.position).sqrMagnitude).First();

        waypointToUse.Flowers.Remove(nearestFlower);
        _targetFlower = nearestFlower;
        return _targetFlower;
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
        Debug.Log($"Enemy {Id} dies.");
        if (_carryingFlower != null)
        {
            _carryingFlower.OnDrop();
            NextWaypoint.Flowers.Add(_carryingFlower);
            Debug.Log($"Enemy {Id} drops flower {_carryingFlower.Id} at waypoint {NextWaypoint.Index}.");
        }

        if (_targetFlower != null)
        {
            // var waypoint = Direction == TravelMethod.Entrance
            //     ? NextWaypoint 
            //     : NextWaypoint.GetPreviousWaypoint();
            var waypoint = NextWaypoint;
            waypoint.Flowers.Add(_targetFlower);
            Debug.Log($"Enemy {Id} returns flower {_targetFlower.Id} to waypoint {waypoint.Index}.");
        }
        Destroy(gameObject);
    }
}
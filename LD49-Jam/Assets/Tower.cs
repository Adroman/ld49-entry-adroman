using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour
{
    public float MinimumDamage;
    public float MaximumDamage;
    public float FireRate;
    
    public float Range
    {
        get => _collider.radius;
        set => _collider.radius = value;
    }

    public Transform RotatingPart;
    public Transform ShootingPoint;
    public float RotatingAngleOffset;
    public Projectile ProjectileToFire;

    private CircleCollider2D _collider;
    private float _reloadTimeRemaining;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    
    public void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_reloadTimeRemaining > 0)
        {
            // reloading
            _reloadTimeRemaining -= Time.deltaTime;
            return;
        }

        if (EnemiesInRange.Count == 0)
        {
            // no enemy to shoot
            return;
        }

        // pick the enemy closest to goal
        var enemy = EnemiesInRange
            .OrderByDescending(e => e.NextWaypoint.Index)
            .ThenBy(e => (e.transform.position - e.NextWaypoint.transform.position).sqrMagnitude)
            .First();

        var direction = enemy.transform.position - transform.position;
        RotateTurret(direction);
        
        Fire(enemy);
    }

    private void Fire(Enemy enemy)
    {
        var point = ShootingPoint != null
            ? ShootingPoint.transform
            : transform;

        var projectile = Instantiate(ProjectileToFire, point.position, point.rotation);
        projectile.Damage = UnityEngine.Random.Range(MinimumDamage, MaximumDamage);
        projectile.Target = enemy;

        _reloadTimeRemaining = 1f / FireRate;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Add(enemy);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Remove(enemy);
        }
    }
    
    private void RotateTurret(Vector3 direction)
    {
        if (RotatingPart == null) return;
        if (direction == Vector3.zero) return;
        float rotatingAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        RotatingPart.rotation = Quaternion.AngleAxis(rotatingAngle + RotatingAngleOffset, Vector3.forward);
    }

    public void EnemyDied(Enemy enemy)
    {
        EnemiesInRange.Remove(enemy);
    }
}

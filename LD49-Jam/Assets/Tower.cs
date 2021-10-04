using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour
{
    public string Name;
    public float MinimumDamage;
    public float MaximumDamage;
    public float FireRate;
    public bool ShootAllTargets;
    public int UpgradePrice;
    public Tower UpgradedTower;

    private AudioSource _audioSource;
    
    public float Range
    {
        get => _collider.radius;
        set => _collider.radius = value;
    }

    public Transform RotatingPart;
    public Transform ShootingPoint;
    public float RotatingAngleOffset;
    public Projectile ProjectileToFire;
    public InstabilityManager InstabilityManager;

    public AudioClip[] SoundsToPlay;
    
    private CircleCollider2D _collider;
    private float _reloadTimeRemaining;
    private readonly List<Enemy> _enemiesInRange = new List<Enemy>();
    private List<SpecialComponent> _specialComponents = new List<SpecialComponent>();
    
    public void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _specialComponents = new List<SpecialComponent>(GetComponents<SpecialComponent>());
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

        if (_enemiesInRange.Count == 0)
        {
            // no enemy to shoot
            return;
        }


        if (ShootAllTargets)
        {
            foreach (var enemy in _enemiesInRange)
            {
                Fire(enemy);
            }
        }
        else
        {
            // pick the enemy closest to goal
            var enemy = _enemiesInRange
                .OrderBy(e => e.CarriesFlower ? 0 : 1)
                .ThenByDescending(e => e.Direction == TravelMethod.Goal ? e.NextWaypoint.Index : 6 - e.NextWaypoint.Index)
                .ThenBy(e => (e.transform.position - e.NextWaypoint.transform.position).sqrMagnitude)
                .First();

            var direction = enemy.transform.position - transform.position;
            RotateTurret(direction);
        
            Fire(enemy);
        }
        
        PlaySound();
    }

    private void Fire(Enemy enemy)
    {
        var point = ShootingPoint != null
            ? ShootingPoint.transform
            : transform;
        
        var projectile = Instantiate(ProjectileToFire, point.position, point.rotation);
        projectile.Damage = UnityEngine.Random.Range(MinimumDamage, MaximumDamage);
        projectile.Target = enemy;
        projectile.InstabilityManager = InstabilityManager;
        foreach (var specialComponent in _specialComponents)
        {
            specialComponent.CloneComponent(projectile.gameObject);
        }

        _reloadTimeRemaining = 1f / FireRate;
    }

    private void PlaySound()
    {
        if (_audioSource == null) return;

        _audioSource.clip = SoundsToPlay[Random.Range(0, SoundsToPlay.Length)];
        _audioSource.Play();
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _enemiesInRange.Add(enemy);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _enemiesInRange.Remove(enemy);
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
        _enemiesInRange.Remove(enemy);
    }
}

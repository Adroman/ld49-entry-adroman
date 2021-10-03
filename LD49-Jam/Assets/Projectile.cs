using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public Enemy Target;
    public float AngleOffset;
    public InstabilityManager InstabilityManager;
    
    public void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            InstabilityManager.IncreaseInstability(Mathf.CeilToInt(Damage));
            return;
        }
        
        var direction = Target.transform.position - transform.position;

        RotateSprite(direction);

        float sqrDistanceLeft = direction.sqrMagnitude;
        float travelDistance = Speed * Time.deltaTime;
        bool targetReached = sqrDistanceLeft < travelDistance * travelDistance;

        transform.Translate(direction.normalized * travelDistance, Space.World);

        if (targetReached)
        {
            HitEnemy(Target);
            Destroy(gameObject);
        }
    }

    private void HitEnemy(Enemy enemy)
    {
        enemy.Health -= Math.Max(1, Damage - enemy.Armor);
        enemy.EnemyHealth.AdjustHealth(enemy);
        if (enemy.Health < 0)
        {
            enemy.Die();
        }
    }
    
    public virtual void AdditionalEffect(Enemy enemy)
    {
        
    }

    private void RotateSprite(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        float rotatingAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotatingAngle + AngleOffset, Vector3.forward);
    }
}
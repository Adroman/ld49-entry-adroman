using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EnemyHealth : MonoBehaviour
{
    public RectTransform HealthBar;

    public void AdjustHealth(Enemy enemy)
    {
        float ratio = enemy.Health / enemy.MaximumHealth;
        HealthBar.localScale = new Vector3(ratio, 1, 1);
    }
}

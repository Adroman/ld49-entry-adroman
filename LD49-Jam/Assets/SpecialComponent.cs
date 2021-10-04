using UnityEngine;
using UnityEngine.Analytics;

public abstract class SpecialComponent : MonoBehaviour
{
    public abstract void PerformSpecialEffect(Enemy e);

    public abstract SpecialComponent CloneComponent(GameObject target);

    public abstract string Description { get; }
}
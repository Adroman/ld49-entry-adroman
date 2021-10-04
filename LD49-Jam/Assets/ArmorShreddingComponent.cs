using UnityEngine;

public class ArmorShreddingComponent : SpecialComponent
{
    public float Amount;
    
    public override void PerformSpecialEffect(Enemy e)
    {
        e.Armor = Mathf.Max(0, e.Armor - Amount);
    }

    public override SpecialComponent CloneComponent(GameObject target)
    {
        var component = target.AddComponent<ArmorShreddingComponent>();
        component.Amount = Amount;

        return component;
    }

    public override string Description => $"Reduces armor by {Amount}";
}
using UnityEngine;

namespace DefaultNamespace
{
    public class DrainerComponent : SpecialComponent
    {
        public float DrainPercentage;
        
        public override void PerformSpecialEffect(Enemy e)
        {
            e.Health *= 1f - DrainPercentage / 100f;
            e.Armor *= 1f - DrainPercentage / 100f;
        }

        public override SpecialComponent CloneComponent(GameObject target)
        {
            var component = target.AddComponent<DrainerComponent>();
            component.DrainPercentage = DrainPercentage;
            
            return component;
        }

        public override string Description => $"Drains {DrainPercentage:F2}% of current health";
    }
}
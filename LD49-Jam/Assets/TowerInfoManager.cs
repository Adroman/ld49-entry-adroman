using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerInfoManager : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text DamageText;
    public TMP_Text FiringRateText;
    public TMP_Text RangeText;
    public TMP_Text SpecialText;
    public TMP_Text PriceText;

    public void UpdateData(Tower tower)
    {
        NameText.text = tower.Name;
        DamageText.text = $"Damage: {tower.MinimumDamage}-{tower.MaximumDamage}";
        FiringRateText.text = $"Fire rate: {tower.FireRate}";
        RangeText.text = $"Range: {tower.GetComponent<CircleCollider2D>().radius}";
        var special = tower.GetComponent<SpecialComponent>();
        SpecialText.text = 
            special == null 
                ? "" 
                : special.Description;
        
        PriceText.text = tower.UpgradedTower != null 
            ? $"Upgrade cost: {tower.UpgradePrice}"
            : "Fully upgraded";
    }

    public void UpdateData(UiTowerSelection selection)
    {
        UpdateData(selection.Tower);

        PriceText.text = $"Build cost: {selection.Price}";
    }
}

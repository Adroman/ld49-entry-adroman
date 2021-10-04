using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTowerSelector : MonoBehaviour
{
    public UiTowerSelection SelectedTower;
    public Image TowerBaseImage;
    public Image TurretImage;
    public TMP_Text Label;

    public void SelectTower(UiTowerSelection tower)
    {
        SelectedTower = tower;
        TowerBaseImage.sprite = tower.TowerBaseImage.sprite;
        TowerBaseImage.color = Color.white;
        TurretImage.sprite = tower.TurretImage.sprite;
        TurretImage.color = Color.white;
        Label.color = Color.white;
    }

    public void DeselectTower()
    {
        SelectedTower = null;
        TowerBaseImage.color = new Color(0, 0, 0, 0);
        TurretImage.color = new Color(0, 0, 0, 0);
        Label.color = new Color(0, 0, 0, 0);
    }
}

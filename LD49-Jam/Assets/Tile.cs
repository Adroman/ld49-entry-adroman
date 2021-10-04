using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public UiTowerSelector TowerSelector;
    public IntVariable GoldAmount;
    public Transform TowersParent;
    public TileCollection Tiles;
    public GameEvent TowerBuilt;
    public GameEvent TowerUpgraded;
    public InstabilityManager InstabilityManager;
    public TowerInfoManager TowerInfoManager;
    
    public Color DeselectedColor = new Color(0, 0, 0, 0);
    public Color SelectedColor = new Color(0, 1, 1, 0.5f);

    private SpriteRenderer _renderer;
    private bool _damaged;
    public Tower BuiltTower;

    public void OnDisable()
    {
        Tiles.RemoveTile(this);
        if (BuiltTower != null)
        {
            Destroy(BuiltTower.gameObject);
        }
    }

    public void OnEnable()
    {
        Tiles.AddTile(this);
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = DeselectedColor;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnMouseExit();
        }
    }


    public void OnMouseEnter()
    {
        if (BuiltTower == null && TowerSelector.SelectedTower != null)
        {
            _renderer.color = SelectedColor;
        }

        if (BuiltTower != null)
        {
            TowerInfoManager.gameObject.SetActive(true);
            TowerInfoManager.UpdateData(BuiltTower);
        }
    }

    public void OnMouseExit()
    {
        _renderer.color = DeselectedColor;
        if (BuiltTower != null)
        {
            TowerInfoManager.gameObject.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
        if (BuiltTower == null && TowerSelector.SelectedTower != null)
        {
            BuildTower();
        }
        else if (BuiltTower != null && BuiltTower.UpgradedTower != null)
        {
            UpgradeTower();
            TowerInfoManager.UpdateData(BuiltTower);
        }
    }

    private void UpgradeTower()
    {
        var selectedTower = BuiltTower.UpgradedTower;
        if (GoldAmount.Value < BuiltTower.UpgradePrice)
        {
            Debug.Log("Not enough money");
            return;
        }

        GoldAmount.Value -= BuiltTower.UpgradePrice;

        var oldTower = BuiltTower;
        
        var t = transform;
        BuiltTower = Instantiate(selectedTower, t.position, t.rotation, TowersParent);
        BuiltTower.InstabilityManager = InstabilityManager;
        TowerUpgraded.RaiseEvent();
        Destroy(oldTower.gameObject);
    }
    
    private void BuildTower()
    {
        var selectedTower = TowerSelector.SelectedTower;
        if (GoldAmount.Value < selectedTower.Price)
        {
            Debug.Log("Not enough money");
            return;
        }

        GoldAmount.Value -= selectedTower.Price;
        
        var t = transform;
        BuiltTower = Instantiate(selectedTower.Tower, t.position, t.rotation, TowersParent);
        BuiltTower.InstabilityManager = InstabilityManager;
        TowerBuilt.RaiseEvent();
        OnMouseExit();
        
        TowerInfoManager.gameObject.SetActive(true);
        TowerInfoManager.UpdateData(BuiltTower);
    }
}
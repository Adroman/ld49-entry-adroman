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

    public void OnMouseEnter()
    {
        if (BuiltTower == null && TowerSelector.SelectedTower != null)
        {
            _renderer.color = SelectedColor;
        }
    }

    public void OnMouseExit()
    {
        _renderer.color = DeselectedColor;
    }

    public void OnMouseDown()
    {
        if (BuiltTower == null && TowerSelector.SelectedTower != null)
        {
            BuildTower();
        }
        else
        {
            // select tower
        }
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
        TowerBuilt.RaiseEvent();
        OnMouseExit();
    }
}
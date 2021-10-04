using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UiTowerSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tower Tower;
    public Image TowerBaseImage;
    public Image TurretImage;
    public int Price;

    public TowerInfoManager TowerInfoManager;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TowerInfoManager.gameObject.SetActive(true);
        TowerInfoManager.UpdateData(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TowerInfoManager.gameObject.SetActive(false);
    }
}

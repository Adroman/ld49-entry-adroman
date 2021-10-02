using UnityEngine;
using UnityEngine.EventSystems;

public class SpecificDisasterPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject AdditionalUi;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        AdditionalUi.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AdditionalUi.SetActive(false);
    }
}
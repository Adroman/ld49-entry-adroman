using System.Collections.Generic;
using UnityEngine;

public class UiDisastersPanel : MonoBehaviour
{
    public SpecificDisasterPanel CurrentPanel;

    public void TogglePanel(SpecificDisasterPanel newPanel)
    {
        if (CurrentPanel == newPanel) return;

        if (CurrentPanel != null)
        {
            CurrentPanel.gameObject.SetActive(false);
        }
        
        newPanel.gameObject.SetActive(true);
        CurrentPanel = newPanel;
    }
}

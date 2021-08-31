using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelButton : MonoBehaviour
{
    public GameObject targetPanel;
    // Start is called before the first frame update
    public void MoveToPanel()
    {
        GameManager.instance.eventManager.eventPanel.ShowPanel(targetPanel);
        GameManager.instance.eventManager.eventPanel.ChangeCurrentPanel(targetPanel);
        for(int i = 0; i < targetPanel.GetComponentsInChildren<SaveSlot>().Length; i++)
        {
            targetPanel.GetComponentsInChildren<SaveSlot>()[i].CheckSlotInfo();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public int mySlot;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<SaveSlot>() != null) mySlot = GetComponent<SaveSlot>().slot;
    }

    public bool CheckIfSlotHasData()
    {
        if(PlayerPrefs.GetInt("Saved_Slot_"+mySlot,-1) != -1) return true;
        else return false;
    }

    public void Save()
    {
        if(CheckIfSlotHasData())
        {
            if(!GameManager.instance.eventManager.eventPanel.OverwritePanel.activeInHierarchy)
            {
                GameManager.instance.eventManager.eventPanel.ShowPanel(GameManager.instance.eventManager.eventPanel.OverwritePanel);
                GameManager.instance.eventManager.eventPanel.ChangeCurrentPanel(GameManager.instance.eventManager.eventPanel.OverwritePanel);
                GameManager.instance.eventManager.eventPanel.OverwritePanel.GetComponentInChildren<SaveButton>().mySlot = mySlot;
            }
            else
            {
                LoadSaveController.instance.SaveGame(mySlot);
                GameManager.instance.eventManager.eventPanel.ShowPanel(GameManager.instance.eventManager.eventPanel.SaveSuccessfullPanel);
                GameManager.instance.eventManager.eventPanel.ChangeCurrentPanel(GameManager.instance.eventManager.eventPanel.SaveSuccessfullPanel);
            }
        }
        else
        {
            LoadSaveController.instance.SaveGame(mySlot);
            GameManager.instance.eventManager.eventPanel.ShowPanel(GameManager.instance.eventManager.eventPanel.SaveSuccessfullPanel);
            GameManager.instance.eventManager.eventPanel.ChangeCurrentPanel(GameManager.instance.eventManager.eventPanel.SaveSuccessfullPanel);
        }
    }
}

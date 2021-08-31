using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public UIEventPanel eventPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        eventPanel = GameObject.FindObjectOfType<UIEventPanel>();
    }

    public void ActivateEvent()
    {
        Time.timeScale = 0f;
    }

    public void DeactivateEvent()
    {
        Time.timeScale = 1f;
    }

    public void ShowEventPanel()
    {
        eventPanel.ShowEventPanel();
    }

    public void GrabKeyEvent(string keycardName, Door myDoor)
    {
        ActivateEvent();
        ShowEventPanel();
        eventPanel.ShowPanel(eventPanel.keycardEventPanel);
        int remainingKeys = myDoor.doorKeys.keycards.Length;
        for(int i = 0; i < myDoor.doorKeys.keycards.Length; i++)
        {
            if(myDoor.doorKeys.keycards[i].hasBeenGrabbed)
            {
                remainingKeys--;
            }
        }
        string message = "You have found the "+ keycardName + "!\n"+ remainingKeys + " keys remaining.";
        eventPanel.ChangeTextOnPanel(eventPanel.keycardEventText, message);
    }
}

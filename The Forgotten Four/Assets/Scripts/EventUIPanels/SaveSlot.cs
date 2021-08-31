using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public int slot;
    public Text currentScene;
    public Image[] activeChars;
    public Text[] activeCharsLevel;
    
    // Start is called before the first frame update

    public void CheckSlotInfo()
    {
        if(PlayerPrefs.GetInt("Saved_Slot_"+slot,0)!=0)
        {
            currentScene.text = PlayerPrefs.GetString("Saved_Slot_"+slot+"_Current_Scene");
            for(int i = 0; i<PlayerController.instance.partyStats.Length;i++)
            {
                if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_"+PlayerController.instance.partyStats[i].charName+"_Active") == "yes")
                {
                    activeChars[i].gameObject.SetActive(true);
                    activeCharsLevel[i].gameObject.SetActive(true);
                    activeCharsLevel[i].text = "LV "+PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Level");
                }
                else
                {
                    activeChars[i].gameObject.SetActive(false);
                    activeCharsLevel[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for(int i = 0; i<activeChars.Length;i++)
            {
                activeChars[i].gameObject.SetActive(false);
                activeCharsLevel[i].gameObject.SetActive(false);
            }
            currentScene.text = "No saved data";
        }
    }

    public void CheckSlotInfoInMainMenu()
    {
        if(PlayerPrefs.GetInt("Saved_Slot_"+slot,0)!=0)
        {
            currentScene.text = PlayerPrefs.GetString("Saved_Slot_"+slot+"_Current_Scene");
            for(int i = 0; i<FindObjectOfType<ScreenTest>().charNames.Length;i++)
            {
                if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_"+FindObjectOfType<ScreenTest>().charNames[i]+"_Active") == "yes")
                {
                    activeChars[i].gameObject.SetActive(true);
                    activeCharsLevel[i].gameObject.SetActive(true);
                    activeCharsLevel[i].text = "LV "+PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+FindObjectOfType<ScreenTest>().charNames[i]+"_Level");
                }
                else
                {
                    activeChars[i].gameObject.SetActive(false);
                    activeCharsLevel[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for(int i = 0; i<activeChars.Length;i++)
            {
                activeChars[i].gameObject.SetActive(false);
                activeCharsLevel[i].gameObject.SetActive(false);
            }
            currentScene.text = "No saved data";
        }
    }
}

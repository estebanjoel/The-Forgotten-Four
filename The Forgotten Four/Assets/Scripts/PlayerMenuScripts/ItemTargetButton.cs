using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTargetButton : MonoBehaviour
{
    public string targetName;
    public Item itemToUse;
    public GameObject targetPanel;
    public GameObject itemMessagePanel;
    private string messageModifier;
    
    // Start is called before the first frame update

    public void Press()
    {
        for (int i = 0; i < PlayerController.instance.partyStats.Length; i++)
        {
            if(PlayerController.instance.partyStats[i].charName == targetName)
            {
                itemToUse.Use(i);
                targetPanel.SetActive(false);
                itemMessagePanel.SetActive(true);
                messageModifier = ModifyMessage();
                itemMessagePanel.GetComponentInChildren<Text>().text = messageModifier;
                break;
            }
        }
    }

    public string ModifyMessage()
    {
        messageModifier = "";
        Consumable consumableItem = (Consumable) itemToUse;
        EquipItem equipItem = (EquipItem) itemToUse;
        if(consumableItem != null)
        {
            if (consumableItem.affectHP)
            {
                messageModifier = targetName + " has recovered " + itemToUse.amountToChange + "HP!";
            }
            if (consumableItem.affectMP)
            {
                messageModifier = targetName + " has recovered " + itemToUse.amountToChange + "HP!";
            }
            if (consumableItem.revivePlayer)
            {
                messageModifier = targetName + " has been revived!";
            }
        }

        if(equipItem != null)
        {            
            if(equipItem.isArmour || equipItem.isWeapon)
            {
                messageModifier = targetName + " has equipped the " + itemToUse.itemName + "!";
            }
        }

        return messageModifier;

    }

}

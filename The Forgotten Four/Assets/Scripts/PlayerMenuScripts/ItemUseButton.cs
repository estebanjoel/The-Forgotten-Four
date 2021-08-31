using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseButton : MonoBehaviour
{
    public Item itemToUse;
    public ItemTargetMenu targetMenu;

    public void Press()
    {
        targetMenu.gameObject.SetActive(true);
        for(int i = 0; i < targetMenu.charButtons.Length; i++)
        {
            if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                targetMenu.charButtons[i].gameObject.SetActive(true);
                targetMenu.charButtons[i].GetComponentInChildren<Text>().text = PlayerController.instance.partyStats[i].charName;
                targetMenu.charButtons[i].targetName = PlayerController.instance.partyStats[i].charName;
                targetMenu.charButtons[i].itemToUse = itemToUse;
            }

            else
            {
                targetMenu.charButtons[i].gameObject.SetActive(false);
            }
        }
        transform.parent.gameObject.SetActive(false);
    }
}

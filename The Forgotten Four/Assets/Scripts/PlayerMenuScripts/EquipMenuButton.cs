using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class EquipMenuButton : MonoBehaviour
{
    public Image itemSprite;
    public Text itemNameText;
    public Text itemQuantityText;
    public Item itemReference;
    int statModifier;
    public bool isWeapon, isArmor;
    bool isAvailable;
    public EquipPanel myPanel;

    public void SetButtonValues()
    {
        itemSprite.sprite = itemReference.itemSprite;
        itemNameText.text = itemReference.itemName;
        for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
        {
            if(GameManager.instance.itemsHeld[i] == itemReference.itemName) itemQuantityText.text = "x"+GameManager.instance.numberOfItems[i];
        }
    }
    public void CheckIfButtonIsAvailable()
    {
        isAvailable = false;
        switch(itemReference.itemCharTarget)
        {
            case equipItemCharTarget.Anyone:
                isAvailable = true;
                break;
            case equipItemCharTarget.None:
                isAvailable = false;
                break;
            case equipItemCharTarget.Heavy:
                if(myPanel.CharIndex == 0 || myPanel.CharIndex == 3) isAvailable = true;
                else isAvailable = false;
                break;
            case equipItemCharTarget.Soft:
                if(myPanel.CharIndex == 1 || myPanel.CharIndex == 2) isAvailable = true;
                else isAvailable = false;
                break;
            case equipItemCharTarget.Sanji:
                if(myPanel.CharIndex == 0) isAvailable = true;
                else isAvailable = false;
                break;
            case equipItemCharTarget.Kenya:
                if(myPanel.CharIndex == 1) isAvailable = true;
                else isAvailable = false;
                break;
            case equipItemCharTarget.Yakov:
                if(myPanel.CharIndex == 2) isAvailable = true;
                else isAvailable = false;
                break;
            case equipItemCharTarget.RX10:
                if(myPanel.CharIndex == 3) isAvailable = true;
                else isAvailable = false;
                break;
        }

        if (!isAvailable)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = new Color(255, 0, 0, 0.58f);
            GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0.19f);
        }
        else
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            GetComponentInChildren<Text>().color = new Color(255, 255, 255, 1f);
        }
    }

    public void ShowStatModifierText()
    {
        if(isAvailable)
        {
            if(isArmor)
            {
                statModifier = itemReference.armorStrength - myPanel.equippedArmor.equippedItemReference.armorStrength;
                myPanel.armorPowerText.transform.GetChild(0).gameObject.SetActive(true);
                Text modifierText = myPanel.armorPowerText.transform.GetChild(0).GetComponent<Text>();
                if(statModifier<0)
                {
                    modifierText.text = statModifier.ToString();
                    modifierText.color = Color.red;
                }
                else
                {
                    modifierText.text = "+"+statModifier.ToString();
                    modifierText.color = Color.green;
                }
            }
            if(isWeapon)
            {
                statModifier = itemReference.weaponStrength - myPanel.equippedWeapon.equippedItemReference.weaponStrength;
                myPanel.weaponPowerText.transform.GetChild(0).gameObject.SetActive(true);
                Text modifierText = myPanel.weaponPowerText.transform.GetChild(0).GetComponent<Text>();
                if(statModifier<0)
                {
                    modifierText.text = statModifier.ToString();
                    modifierText.color = Color.red;
                }
                else
                {
                    modifierText.text = "+"+statModifier.ToString();
                    modifierText.color = Color.green;
                }
            }
        }
    }

    public void HideStatModifierText()
    {
        if(isArmor) myPanel.armorPowerText.transform.GetChild(0).gameObject.SetActive(false);        
        if(isWeapon) myPanel.weaponPowerText.transform.GetChild(0).gameObject.SetActive(false);        
    }

    public void Press()
    {
        CharStats myChar = PlayerController.instance.partyStats[myPanel.CharIndex];
        if(isArmor)
        {
            int armrPwr = myChar.GetCharStats().GetArmourPower();
            armrPwr-= myPanel.equippedArmor.equippedItemReference.armorStrength;
            for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
            {
                if(GameManager.instance.itemsHeld[i] == myPanel.equippedArmor.equippedItemReference.itemName)
                {
                    GameManager.instance.numberOfItems[i]++;
                    for(int j = 0; j < myPanel.panelButtons.buttons.Length; j++)
                    {
                        if(myPanel.panelButtons.buttons[j].itemReference.itemName ==myPanel.equippedArmor.equippedItemReference.itemName)
                        {
                            myPanel.panelButtons.buttons[j].SetButtonValues();
                            break;
                        }
                    }
                    break;
                }
            }
            armrPwr+= itemReference.armorStrength;
            myChar.GetCharStats().SetArmourPower(armrPwr);
            myChar.equippedArmr = itemReference.itemName;
            for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
            {
                if(GameManager.instance.itemsHeld[i] == itemReference.itemName)
                {
                    GameManager.instance.numberOfItems[i]--;
                    if(GameManager.instance.numberOfItems[i] <= 0)
                    {
                        GameManager.instance.numberOfItems[i] = 0;
                        this.gameObject.SetActive(false);
                    }
                }
            }
            myPanel.armorPowerText.text = "A.P.: "+myChar.GetCharStats().GetArmourPower();
            myPanel.equippedArmor.equippedItemReference = itemReference;
            myPanel.equippedArmor.equippedItemSprite.sprite = myPanel.equippedArmor.equippedItemReference.itemSprite;
            myPanel.equippedArmor.equippedItemText.text = myPanel.equippedArmor.equippedItemReference.itemName;
        }
        if(isWeapon)
        {
            int wpnPwr = myChar.GetCharStats().GetWeaponPower();
            wpnPwr-= myPanel.equippedWeapon.equippedItemReference.weaponStrength;
            for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
            {
                if(GameManager.instance.itemsHeld[i] == myPanel.equippedWeapon.equippedItemReference.itemName)
                {
                    GameManager.instance.numberOfItems[i]++;
                    for(int j = 0; j < myPanel.panelButtons.buttons.Length; j++)
                    {
                        if(myPanel.panelButtons.buttons[j].itemReference.itemName ==myPanel.equippedWeapon.equippedItemReference.itemName)
                        {
                            myPanel.panelButtons.buttons[j].SetButtonValues();
                            break;
                        }
                    }
                    break;
                }
            }
            wpnPwr+= itemReference.weaponStrength;
            myChar.GetCharStats().SetWeaponPower(wpnPwr);
            myChar.equippedWpn = itemReference.itemName;
            for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
            {
                if(GameManager.instance.itemsHeld[i] == itemReference.itemName)
                {
                    GameManager.instance.numberOfItems[i]--;
                    if(GameManager.instance.numberOfItems[i] <= 0)
                    {
                        GameManager.instance.numberOfItems[i] = 0;
                        this.gameObject.SetActive(false);
                    }
                }
            }
            myPanel.weaponPowerText.text = "W.P.: "+myChar.GetCharStats().GetWeaponPower();
            myPanel.equippedWeapon.equippedItemReference = itemReference;
            myPanel.equippedWeapon.equippedItemSprite.sprite = myPanel.equippedWeapon.equippedItemReference.itemSprite;
            myPanel.equippedWeapon.equippedItemText.text = myPanel.equippedWeapon.equippedItemReference.itemName;
        }

        transform.GetComponentInParent<PlayerMenu>().ShowEquipButtons(myPanel.CharIndex);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Create Item/Equip Item", order = 0)]
public class EquipItem : Item
{
    [Header("Equip Item Target")]
    public equipItemCharTarget itemCharTarget;
    [Header("Weapon Details")]
    public bool isWeapon;
    public int weaponStrength;
    [Header("Armor Details")]
    public bool isArmour;
    public int armourStrength;

    public override void UseItem(CharStats selectedCharacter, Item[] myItems, int[] myItemsQuantity)
    {
        if (isWeapon)
        {
            int newWeaponPower = 0;
            if (selectedCharacter.equippedWpn == "")
            {
                selectedCharacter.equippedWpn = itemName;
                newWeaponPower = selectedCharacter.GetCharStats().GetWeaponPower() + weaponStrength;
            }

            else
            {
                newWeaponPower = SetEquippedItemValue(selectedCharacter.GetCharStats().GetWeaponPower(), weaponStrength, isWeapon, isArmour, selectedCharacter, myItems, myItemsQuantity);
            }
            
            selectedCharacter.GetCharStats().SetWeaponPower(newWeaponPower);
        }

        if (isArmour)
        {
            int newArmourPower = 0;
            if (selectedCharacter.equippedArmr == "")
            {
                selectedCharacter.equippedArmr = itemName;
                newArmourPower = selectedCharacter.GetCharStats().GetArmourPower() + armourStrength;
            }

            else
            {
                newArmourPower = SetEquippedItemValue(selectedCharacter.GetCharStats().GetArmourPower(), armourStrength, isWeapon, isArmour, selectedCharacter, myItems, myItemsQuantity);
            }

            selectedCharacter.GetCharStats().SetArmourPower(newArmourPower);
        }
    }

    int SetEquippedItemValue(int value, int itemStrength, bool isWeapon, bool isArmor, CharStats selectedCharacter, Item[] items, int[] itemsQuantity)
    {
        int newValue = value;
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == selectedCharacter.equippedWpn)
            {
                EquipItem previousItem = (EquipItem) items[i];
                itemsQuantity[i]++;
                if(isWeapon) newValue = value - previousItem.weaponStrength;
                if(isArmor) newValue = value - previousItem.armourStrength;
                newValue += itemStrength;
                
            }
            if(items[i].itemName == itemName)
            {
                itemsQuantity[i]--;
            }
        }
        return newValue;
    }
}

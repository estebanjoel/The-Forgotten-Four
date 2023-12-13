using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    // [Header("Item Type")]
    // public bool isItem;
    // public bool isWeapon;
    // public bool isArmor;
    // public bool affectHP, affectMP, revivePlayer;
    // [Header("Equip Item Target")]
    // public equipItemCharTarget itemCharTarget;
    // [Header("Weapon Details")]
    // public int weaponStrength;
    // [Header("Armor Details")]
    // public int armorStrength;

    // public void Use(int character)
    // {
    //     CharStats selectedCharacter = PlayerController.instance.partyStats[character];
    //     Item[] myItems = GameManager.instance.referenceItems;
    //     int[] myItemsQuantity = GameManager.instance.numberOfItems;
    //     if (isItem)
    //     {
    //         UseItem(selectedCharacter, myItems, myItemsQuantity);
    //     }

    //     /*if (isWeapon)
    //     {
    //         if (selectedCharacter.equippedWpn == "")
    //         {
    //             selectedCharacter.equippedWpn = itemName;
    //             selectedCharacter.wpnPwr += weaponStrength;
    //         }

    //         else
    //         {
    //             for (int i = 0; i < myItems.Length; i++)
    //             {
    //                 if (myItems[i].isWeapon)
    //                 {
    //                     if (myItems[i].itemName == selectedCharacter.equippedWpn)
    //                     {
    //                         myItemsQuantity[i]++;
    //                         selectedCharacter.wpnPwr -= myItems[i].weaponStrength;
    //                         selectedCharacter.wpnPwr += weaponStrength;
    //                     }

    //                     if (myItems[i].itemName == itemName)
    //                     {
    //                         myItemsQuantity[i]--;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     if (isArmor)
    //     {
    //         if (selectedCharacter.equippedArmr == "")
    //         {
    //             selectedCharacter.equippedArmr = itemName;
    //             selectedCharacter.armrPwr += armorStrength;
    //         }

    //         else
    //         {
    //             for (int i = 0; i < myItems.Length; i++)
    //             {
    //                 if (myItems[i].isWeapon)
    //                 {
    //                     if (myItems[i].itemName == selectedCharacter.equippedArmr)
    //                     {
    //                         myItemsQuantity[i]++;
    //                         selectedCharacter.armrPwr -= myItems[i].armorStrength;
    //                         selectedCharacter.armrPwr += armorStrength;
    //                     }

    //                     if (myItems[i].itemName == itemName)
    //                     {
    //                         myItemsQuantity[i]--;
    //                     }
    //                 }
    //             }
    //         }
    //     }*/
    // }

    // public void UseItem(CharStats selectedCharacter, Item[] myItems, int[] myItemsQuantity)
    // {
    //     for (int i = 0; i < myItems.Length; i++)
    //     {
    //         if (myItems[i].itemName == itemName)
    //         {
    //             myItemsQuantity[i]--;
    //             ApplyItemEffect(selectedCharacter);
    //         }
    //     }
    // }

    // public void ApplyItemEffect(CharStats selectedCharacter)
    // {
    //     if (affectHP)
    //     {
    //         int currentHP = selectedCharacter.GetCharStats().GetCurrentHP();
    //         currentHP += amountToChange;

    //         if (currentHP > selectedCharacter.GetCharStats().GetMaxHP()) selectedCharacter.GetCharStats().SetCurrentHP(selectedCharacter.GetCharStats().GetMaxHP());
    //         else selectedCharacter.GetCharStats().SetCurrentHP(currentHP);
    //     }

    //     if (affectMP)
    //     {
    //         int currentMP = selectedCharacter.GetCharStats().GetCurrentMP();
    //         currentMP += amountToChange;

    //         if (currentMP > selectedCharacter.GetCharStats().GetMaxMP()) selectedCharacter.GetCharStats().SetCurrentMP(selectedCharacter.GetCharStats().GetMaxMP());
    //         else selectedCharacter.GetCharStats().SetCurrentMP(currentMP);
    //     }

    //     if (revivePlayer)
    //     {
    //         if (selectedCharacter.GetCharStats().GetCurrentHP() == 0)
    //         {
    //             int currentHP = selectedCharacter.GetCharStats().GetCurrentHP();
    //             currentHP += Mathf.FloorToInt((selectedCharacter.GetCharStats().GetMaxHP() * amountToChange) / selectedCharacter.GetCharStats().GetMaxHP());
    //             selectedCharacter.GetCharStats().SetCurrentHP(currentHP);
    //         }
    //     }
    // }



}

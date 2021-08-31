using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Create Item/Consumable Item", order = 0)]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;
    public AudioClip itemAudio;
    public int amountToChange;

    public void Use(int character)
    {
        CharStats selectedCharacter = PlayerController.instance.partyStats[character];
        Item[] myItems = GameManager.instance.referenceItems;
        int[] myItemsQuantity = GameManager.instance.numberOfItems;
        UseItem(selectedCharacter, myItems, myItemsQuantity);
    }
    
    public abstract void UseItem(CharStats selectedCharacter, Item[] myItems, int[] myItemsQuantity);
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

    // public void UseItemInBattle(BattleChar activeCharacter, Item[] myItems, int[] myItemsQuantity)
    // {
    //     for (int i = 0; i < myItems.Length; i++)
    //     {
    //         if (myItems[i].itemName == itemName)
    //         {
    //             myItemsQuantity[i]--;
    //             ApplyItemEffectInBattle(activeCharacter);
    //         }
    //     }
    // }

    // public void ApplyItemEffectInBattle(BattleChar selectedCharacter)
    // {
    //     if (affectHP)
    //     {
    //         ItemVFXInBattle(selectedCharacter, 0, Color.green, false,amountToChange);
    //         selectedCharacter.currentHp += amountToChange;

    //         if (selectedCharacter.currentHp > selectedCharacter.maxHP)
    //         {
    //             selectedCharacter.currentHp = selectedCharacter.maxHP;
    //         }
    //     }

    //     if (affectMP)
    //     {
    //         ItemVFXInBattle(selectedCharacter, 1, Color.green, true,amountToChange);
    //         selectedCharacter.currentMP += amountToChange;

    //         if (selectedCharacter.currentMP > selectedCharacter.maxMP)
    //         {
    //             selectedCharacter.currentMP = selectedCharacter.maxMP;
    //         }
    //     }

    //     if (revivePlayer)
    //     {
    //         if (selectedCharacter.currentHp == 0)
    //         {
    //             selectedCharacter.currentHp += Mathf.FloorToInt((selectedCharacter.maxHP * amountToChange) / selectedCharacter.maxHP);
    //             ItemVFXInBattle(selectedCharacter, 2, Color.green, false,amountToChange);
    //         }

    //         else
    //         {
    //             selectedCharacter.currentHp += 0;
    //             ItemVFXInBattle(selectedCharacter, 2, Color.green, false, 0);
    //             selectedCharacter.SetAnimatorTrigger("reviveTrigger");
    //         }
    //     }
    // }

    // public void ItemVFXInBattle(BattleChar selectedCharacter, int effectIndex, Color numberColor, bool isMP, int numberOnScreen)
    // {
    //     GameObject effect = Instantiate(BattleManager.instance.battleItems.itemEffects[effectIndex]);
    //     PlayItemAudioInBattle();
    //     effect.transform.SetParent(selectedCharacter.transform);
    //     effect.transform.position = new Vector3(selectedCharacter.transform.position.x + 0.1f,selectedCharacter.transform.position.y+0.5f,selectedCharacter.transform.position.z);
    //     Instantiate(BattleManager.instance.damageNumber, selectedCharacter.transform.position, selectedCharacter.transform.rotation).SetDamage(numberOnScreen);
    //     GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = numberColor;
    //     if (isMP)
    //     {
    //         GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().text += " MP";
    //     }
    // }

    // public void PlayItemAudioInBattle()
    // {
    //     AudioManager.instance.BattleSFX[1].clip = itemAudio;
    //     if (!AudioManager.instance.BattleSFX[1].isPlaying)
    //     {
    //         AudioManager.instance.BattleSFX[1].Play();
    //     }
    // }

}

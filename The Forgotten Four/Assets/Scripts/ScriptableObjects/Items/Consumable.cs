using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class Consumable : Item
{
    bool affectHP;
    bool affectMP;
    bool revivePlayer;
    public override void UseItem(CharStats selectedCharacter, Item[] myItems, int[] myItemsQuantity)
    {
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i].itemName == itemName)
            {
                myItemsQuantity[i]--;
                ApplyItemEffect(selectedCharacter);
            }
        }
    }

    public void ApplyItemEffect(CharStats selectedCharacter)
    {
        if (affectHP)
        {
            int currentHP = selectedCharacter.GetCharStats().GetCurrentHP();
            currentHP += amountToChange;

            if (currentHP > selectedCharacter.GetCharStats().GetMaxHP()) selectedCharacter.GetCharStats().SetCurrentHP(selectedCharacter.GetCharStats().GetMaxHP());
            else selectedCharacter.GetCharStats().SetCurrentHP(currentHP);
        }

        if (affectMP)
        {
            int currentMP = selectedCharacter.GetCharStats().GetCurrentMP();
            currentMP += amountToChange;

            if (currentMP > selectedCharacter.GetCharStats().GetMaxMP()) selectedCharacter.GetCharStats().SetCurrentMP(selectedCharacter.GetCharStats().GetMaxMP());
            else selectedCharacter.GetCharStats().SetCurrentMP(currentMP);
        }

        if (revivePlayer)
        {
            if (selectedCharacter.GetCharStats().GetCurrentHP() == 0)
            {
                int currentHP = selectedCharacter.GetCharStats().GetCurrentHP();
                currentHP += Mathf.FloorToInt((selectedCharacter.GetCharStats().GetMaxHP() * amountToChange) / selectedCharacter.GetCharStats().GetMaxHP());
                
                if (currentHP > selectedCharacter.GetCharStats().GetMaxHP()) selectedCharacter.GetCharStats().SetCurrentHP(selectedCharacter.GetCharStats().GetMaxHP());
                else selectedCharacter.GetCharStats().SetCurrentHP(currentHP);
            }
        }
    }
}

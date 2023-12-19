using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Create Item/Consumable Item", order = 0)]
public class Consumable : Item
{
    public bool affectHP;
    public bool affectMP;
    public bool revivePlayer;

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

    public void UseItemInBattle(Battler activeCharacter, Item[] myItems, int[] myItemsQuantity)
    {
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i].itemName == itemName)
            {
                myItemsQuantity[i]--;
                ApplyItemEffectInBattle(activeCharacter);
            }
        }
    }

    public void ApplyItemEffectInBattle(Battler selectedCharacter)
    {
        if (affectHP)
        {
            ItemVFXInBattle(selectedCharacter, 0, Color.green, false,amountToChange);
            selectedCharacter.chara.currentHp += amountToChange;

            if (selectedCharacter.chara.currentHp > selectedCharacter.chara.maxHP)
            {
                selectedCharacter.chara.currentHp = selectedCharacter.chara.maxHP;
            }
        }

        if (affectMP)
        {
            ItemVFXInBattle(selectedCharacter, 1, Color.green, true,amountToChange);
            selectedCharacter.chara.currentMP += amountToChange;

            if (selectedCharacter.chara.currentMP > selectedCharacter.chara.maxMP)
            {
                selectedCharacter.chara.currentMP = selectedCharacter.chara.maxMP;
            }
        }

        if (revivePlayer)
        {
            if (selectedCharacter.chara.currentHp == 0)
            {
                selectedCharacter.chara.currentHp += Mathf.FloorToInt((selectedCharacter.chara.maxHP * amountToChange) / selectedCharacter.chara.maxHP);
                ItemVFXInBattle(selectedCharacter, 2, Color.green, false,amountToChange);
            }

            else
            {
                selectedCharacter.chara.currentHp += 0;
                ItemVFXInBattle(selectedCharacter, 2, Color.green, false, 0);
                selectedCharacter.SetAnimatorTrigger("reviveTrigger");
            }
        }
    }

    public void ItemVFXInBattle(Battler selectedCharacter, int effectIndex, Color numberColor, bool isMP, int numberOnScreen)
    {
        GameObject effect = Instantiate(BattleManager.instance.battleItems.itemEffects[effectIndex]);
        PlayItemAudioInBattle();
        effect.transform.SetParent(selectedCharacter.transform);
        effect.transform.position = new Vector3(selectedCharacter.transform.position.x + 0.1f,selectedCharacter.transform.position.y+0.5f,selectedCharacter.transform.position.z);
        Instantiate(BattleManager.instance.damageNumber, selectedCharacter.transform.position, selectedCharacter.transform.rotation).SetDamage(numberOnScreen);
        GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = numberColor;
        if (isMP)
        {
            GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().text += " MP";
        }
    }

    public void PlayItemAudioInBattle()
    {
        AudioManager.instance.BattleSFX[1].clip = itemAudio;
        if (!AudioManager.instance.BattleSFX[1].isPlaying)
        {
            AudioManager.instance.BattleSFX[1].Play();
        }
    }
}

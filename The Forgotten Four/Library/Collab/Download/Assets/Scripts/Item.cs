using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    [Header("Item Main Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;
    [Header("Item Specific Details")]
    public int amountToChange;
    public bool affectHP, affectMP, revivePlayer;
    [Header("Weapon Details")]
    public int weaponStrength;
    [Header("Armor Details")]
    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int character)
    {
        CharStats selectedCharacter = PlayerController.instance.partyStats[character];
        if (isItem)
        {
            if (affectHP)
            {
                selectedCharacter.currentHP += amountToChange;

                if(selectedCharacter.currentHP > selectedCharacter.maxHP)
                {
                    selectedCharacter.currentHP = selectedCharacter.maxHP;
                }
            }

            if (affectMP)
            {
                selectedCharacter.currentMP += amountToChange;

                if (selectedCharacter.currentMP > selectedCharacter.maxMP)
                {
                    selectedCharacter.currentMP = selectedCharacter.maxMP;
                }
            }

            if (revivePlayer)
            {
                if (selectedCharacter.currentHP == 0)
                {
                    selectedCharacter.currentHP += Mathf.FloorToInt((selectedCharacter.maxHP * amountToChange) / selectedCharacter.maxHP);
                }
            }
        }

        if (isWeapon)
        {

        }

        if (isArmor)
        {

        }
    }
}

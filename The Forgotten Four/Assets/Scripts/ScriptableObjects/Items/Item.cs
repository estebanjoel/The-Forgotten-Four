using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

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

}

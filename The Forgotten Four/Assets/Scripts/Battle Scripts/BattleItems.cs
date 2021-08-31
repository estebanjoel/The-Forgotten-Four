using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItems : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public GameObject[] itemEffects;

    public void SetItems(Item[] myItems)
    {
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i].isItem)
            {
                items.Add(myItems[i]);
            }
        }
    }

    public void ClearItems()
    {
        items.Clear();
    }
}

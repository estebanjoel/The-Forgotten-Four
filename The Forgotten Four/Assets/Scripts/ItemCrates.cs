using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrates : Container
{
    public List<ItemCrate> levelCrates = new List<ItemCrate>();

    public override void SetItemsOnContainer(int index)
    {
        levelCrates.Add(GetComponentsInChildren<ItemCrate>()[index]);
    }
}

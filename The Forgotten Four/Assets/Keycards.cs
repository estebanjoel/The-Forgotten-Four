using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycards : Container
{
    public List<KeyCard> levelKeycards = new List<KeyCard>();

    public override void SetItemsOnContainer(int index)
    {
        levelKeycards.Add(GetComponentsInChildren<KeyCard>()[index]);
    }
}

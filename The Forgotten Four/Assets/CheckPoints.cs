using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : Container
{
    public List<CheckPoint> levelCheckpoints = new List<CheckPoint>();

    public override void SetItemsOnContainer(int index)
    {
        levelCheckpoints.Add(GetComponentsInChildren<CheckPoint>()[index]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePoints : Container
{
    public List<RestorePoint> restorePoints = new List<RestorePoint>();

    public override void SetItemsOnContainer(int index)
    {
        restorePoints.Add(GetComponentsInChildren<RestorePoint>()[index]);
    }
}

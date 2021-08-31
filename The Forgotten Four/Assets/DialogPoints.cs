using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPoints : Container
{
    public List<DialogPoint> levelDialogPoints = new List<DialogPoint>();

    public override void SetItemsOnContainer(int index)
    {
        levelDialogPoints.Add(GetComponentsInChildren<DialogPoint>()[index]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingPoints : Container
{
    public List<SavingPoint> savingPoints;

    public override void SetItemsOnContainer(int index)
    {
        savingPoints.Add(GetComponentsInChildren<SavingPoint>()[index]);
    }
}

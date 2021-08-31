using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SetItemsOnContainer(i);
        }
    }

    public abstract void SetItemsOnContainer(int index);
}

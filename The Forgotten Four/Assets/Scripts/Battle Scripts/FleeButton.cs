using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleeButton : MonoBehaviour
{
    public BattleManager myManager;

    public void Press()
    {
        if(!myManager.hasClickedFleeButton)
        {
            myManager.Flee();
        }
    }
}

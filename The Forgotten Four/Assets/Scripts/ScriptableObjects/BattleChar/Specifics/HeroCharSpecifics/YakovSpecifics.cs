using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakovSpecifics : HeroCharSpecifics
{
    [Header ("YakovChar Specific Variables")]
    public bool isDrunk;
    public int drunkTemp;
    
    public void VodkaMadness()
    {
        drunkTemp = 3;
        isDrunk = true;
    }

    public bool CheckIfIsDrunk()
    {
        if(isDrunk)
        {
            drunkTemp--;
            if(drunkTemp<=0)
            {
                isDrunk = false;
            }
            return true;
        }
        
        return false;
    }
}

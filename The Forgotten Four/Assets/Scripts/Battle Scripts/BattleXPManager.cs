using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleXPManager : MonoBehaviour
{
    public int expToGive;
    private int xpDivider, xpToEachPartyMember, xpGained;
    private float xpModifier;

    public void SetXPToGive(int xp)
    {
        expToGive = xp;
    }

    public void DivideXP(BattleManager bm)
    {
        xpDivider = 0;
        for (int i = 0; i < PlayerController.instance.partyStats.Length; i++)
        {
            if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                xpDivider++;
            }
        }
        xpToEachPartyMember = expToGive / xpDivider;
        xpModifier = Random.Range(1, 1.25f);
        DistributeXP(bm, xpToEachPartyMember, xpModifier);
    }

    public void DistributeXP(BattleManager bm, int xpToEachPartyMember, float xpModifier)
    {
        for (int i = 0; i < PlayerController.instance.partyStats.Length; i++)
        {
            if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                xpGained = Mathf.FloorToInt(xpToEachPartyMember * xpModifier);
                int currentLevel = PlayerController.instance.partyStats[i].playerLevel;
                PlayerController.instance.partyStats[i].currentEXP += xpGained;
                PlayerController.instance.partyStats[i].CheckExp();
                for (int j = 0; j < bm.activeBattlers.Count; j++)
                {
                    if (PlayerController.instance.partyStats[i].charName == bm.activeBattlers[j].charName)
                    {
                        DamageNumber expNumber = Instantiate(bm.damageNumber, bm.activeBattlers[j].transform.position, bm.activeBattlers[j].transform.rotation);
                        expNumber.GetComponentInChildren<Text>().text = "^" + xpGained + " XP";
                        if (PlayerController.instance.partyStats[i].playerLevel != currentLevel)
                        {
                            GameManager.instance.hasBeenAnLevelUP = true;
                            GameManager.instance.levelUPChars[i] = true;
                            // DamageNumber levelUP = Instantiate(bm.damageNumber, bm.activeBattlers[j].transform.position, bm.activeBattlers[j].transform.rotation);
                            // levelUP.GetComponentInChildren<Text>().text = "LEVEL UP!";
                        }
                    }
                }
            }
        }
        expToGive = 0;
    }
}

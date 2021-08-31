using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;
    public int activeBattlerTarget;
    public Text targetName;
    public bool isSingle;
    
    public void Press()
    {
        if(isSingle)
        {
            SingleAction();
        }

        else
        {
            MultipleAction();
        }

        BattleManager.instance.Invoke("NextTurn",3f);
    }

    public void SingleAction()
    {
        if (!BattleManager.instance.activeBattlers[activeBattlerTarget].isPlayer)
        {
            BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
            BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
        }

        else
        {
            for(int i = 0; i < BattleManager.instance.movesList.Length; i++)
            {
                if(BattleManager.instance.movesList[i].moveName == moveName)
                {
                    BattleManager.instance.CypherToPlayer(moveName, activeBattlerTarget);
                    BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
                    break;
                }
            }
                
            for(int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
            {
                if (GameManager.instance.itemsHeld[i] == moveName)
                {
                    BattleManager.instance.ItemToPlayer(moveName, activeBattlerTarget);
                    BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
                    break;
                }
            }
        }
    }
    public void MultipleAction()
    {
        if (!BattleManager.instance.activeBattlers[activeBattlerTarget].isPlayer)
        {
            for(int i = 0; i<BattleManager.instance.activeBattlers.Count;i++)
            {
                if(!BattleManager.instance.activeBattlers[i].isPlayer)
                {
                    if(BattleManager.instance.activeBattlers[i].currentHp>0) BattleManager.instance.PlayerAttack(moveName, i);
                }
            }
            BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
        }
        else
        {
            bool hasInitiativeChanged = false;
            for(int i = 0; i < BattleManager.instance.movesList.Length; i++)
            {
                if(BattleManager.instance.movesList[i].moveName == moveName)
                {
                    for(int j = 0; j < BattleManager.instance.activeBattlers.Count; j++)
                    {
                        if(BattleManager.instance.activeBattlers[j].isPlayer)
                        {
                            if(BattleManager.instance.activeBattlers[j].currentHp>0) BattleManager.instance.CypherToPlayer(moveName, j);
                            if(BattleManager.instance.activeBattlers[j].hasAnAlteratedStat)
                            {
                                for(int k=0; k<BattleManager.instance.activeBattlers[j].previousAlteratedStatsName.Count;k++)
                                {
                                    if(BattleManager.instance.activeBattlers[j].previousAlteratedStatsName[k] == GameEnums.statTarget.Initiative)
                                    {
                                        hasInitiativeChanged = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
                    break;
                }
            }
            if(hasInitiativeChanged) BattleManager.instance.SortBattlers();
            for(int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
            {
                if (GameManager.instance.itemsHeld[i] == moveName)
                {
                    for(int j = 0; j < BattleManager.instance.activeBattlers.Count; j++)
                    {
                        if(BattleManager.instance.activeBattlers[j].isPlayer)
                        {
                            if(BattleManager.instance.activeBattlers[j].currentHp>0) BattleManager.instance.ItemToPlayer(moveName, j);
                        }
                    }
                    BattleManager.instance.battleUI.playerHasSelectedAnOption = false;
                    break;
                }
            }    
        }
    }
}

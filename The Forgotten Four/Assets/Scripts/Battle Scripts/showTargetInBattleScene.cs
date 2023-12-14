using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTargetInBattleScene : MonoBehaviour
{
    public int myTarget;
    string move;
    bool isCypher;

    public void ShowTarget()
    {
        isCypher = false;
        myTarget = GetComponent<BattleTargetButton>().activeBattlerTarget;
        move = GetComponent<BattleTargetButton>().moveName;
        for(int i = 0; i< BattleManager.instance.movesList.Length;i++)
        {
            if(BattleManager.instance.movesList[i].moveName == move)
            {
                if(BattleManager.instance.movesList[i].mTarget == GameEnums.moveTarget.All)
                {
                    if(BattleManager.instance.activeBattlers[myTarget].chara.isPlayer)
                    {
                        for(int j = 0; j<BattleManager.instance.activeBattlers.Count;j++)
                        {
                            if(BattleManager.instance.activeBattlers[j].chara.isPlayer)
                            {
                                if(BattleManager.instance.activeBattlers[j].chara.currentHp>0) BattleManager.instance.indicators.SetTargetIndicator(BattleManager.instance.activeBattlers, j);
                            }
                        }
                    }

                    else
                    {
                        for(int j = 0; j<BattleManager.instance.activeBattlers.Count;j++)
                        {
                            if(!BattleManager.instance.activeBattlers[j].chara.isPlayer)
                            {
                                if(BattleManager.instance.activeBattlers[j].chara.currentHp>0) BattleManager.instance.indicators.SetTargetIndicator(BattleManager.instance.activeBattlers, j);
                            }
                        }
                    }
                }

                else
                {
                    BattleManager.instance.indicators.SetTargetIndicator(BattleManager.instance.activeBattlers, myTarget);
                }
                isCypher = true;
                break;
            }
        }
        if(!isCypher) BattleManager.instance.indicators.SetTargetIndicator(BattleManager.instance.activeBattlers, myTarget);
    }

    public void CloseTargets()
    {
        BattleManager.instance.indicators.ClearTargetIndicators();
    }
}

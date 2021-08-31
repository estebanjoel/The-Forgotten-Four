using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

public class BattleEnemyIA : MonoBehaviour
{
    public bool isHealer;
    public bool isBoss;
    public BattleChar battleReference;
    public Animator anim;

    void Start() 
    {
        CheckIfIAmAHealer();
    }

    public void CheckIfIAmAHealer()
    {
        for(int i = 0; i<BattleManager.instance.movesList.Length;i++)
        {
            for(int j = 0; j<battleReference.movesAvailable.Count;j++)
            {
                if(BattleManager.instance.movesList[i].moveName == battleReference.movesAvailable[j])
                {
                    if(BattleManager.instance.movesList[i].eType == elementType.Heal)
                    {
                        isHealer = true;
                        break;
                    }
                }
            }
        }
    }

    public int CheckForEnemiesWounded(int index)
    {
        for(int i = 0; i<BattleManager.instance.activeBattlers.Count;i++)
        {
            if(!BattleManager.instance.activeBattlers[i].isPlayer)
            {
                if(BattleManager.instance.activeBattlers[i].currentHp<=(BattleManager.instance.activeBattlers[i].maxHP/4))
                {
                    index = i;
                    return index;
                }
            }
        }
        return index;
    }

    public void SetBossAnimationTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
}

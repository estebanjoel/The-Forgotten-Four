using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIndicators : MonoBehaviour
{
    public GameObject turnIndicator, targetIndicator;
    public Transform[] indicatorPositions;

    public void SetTurnIndicator(List<BattleChar> battlers, int currentTurn)
    {
        for (int i = 0; i < indicatorPositions.Length; i++)
        {
            if (indicatorPositions[i].parent != battlers[currentTurn].gameObject.transform.parent)
            {
                if (i >= battlers.Count)
                {
                    i = 0;
                }
            }
            else
            {
                if (indicatorPositions[i].childCount == 0)
                {
                    InstantiateIndicator(i);
                }
                break;
            }
        }
    }

    public void InstantiateIndicator(int position)
    {
        Instantiate(turnIndicator, indicatorPositions[position]);
    }

    public void ClearTurnIndicators()
    {
        for (int i = 0; i < indicatorPositions.Length; i++)
        {
            if (indicatorPositions[i].childCount > 0)
            {
                for(int j = 0; j < indicatorPositions[i].childCount; j++)
                {
                    Destroy(indicatorPositions[i].GetChild(j).gameObject);
                }                
            }
        }
    }

    public void SetTargetIndicator(List<BattleChar> battlers, int target)
    {
        for (int i = 0; i < indicatorPositions.Length; i++)
        {
            if (indicatorPositions[i].parent != battlers[target].gameObject.transform.parent)
            {
                if (i >= battlers.Count)
                {
                    i = 0;
                }
            }
            else
            {
                InstantiateTargetIndicator(i);
                break;
            }
        }
    }

    public void InstantiateTargetIndicator(int position)
    {
        Instantiate(targetIndicator, indicatorPositions[position]);
    }

    public void ClearTargetIndicators()
    {
        for (int i = 0; i < indicatorPositions.Length; i++)
        {
            if (indicatorPositions[i].Find("TargetIndicator(Clone)") != null)
            {
                Destroy(indicatorPositions[i].Find("TargetIndicator(Clone)").gameObject);
            }
        }
    }
}

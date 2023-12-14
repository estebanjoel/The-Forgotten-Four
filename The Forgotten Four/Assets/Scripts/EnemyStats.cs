using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public BattleEnemyChar enemyStats;
    public elementType elementalType;
    public elementType isWeakToElement;
    public int minHPModifier, maxHPModifier, minMPModifier, maxMPModifier, xpToGive;
    void Start()
    {
        enemyStats.maxHP = Mathf.FloorToInt(enemyStats.vitality * Random.Range(1, 5) + Random.Range(minHPModifier, maxHPModifier));
        enemyStats.currentHp = enemyStats.maxHP;
        enemyStats.maxMP = Mathf.FloorToInt(enemyStats.element * Random.Range(1, 5) + Random.Range(minMPModifier, maxMPModifier));
        enemyStats.currentMP = enemyStats.maxMP;
    }

    public void EnemyFade()
    {
        enemyStats.shouldFade = true;
    }


}

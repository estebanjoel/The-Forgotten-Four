using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class BattleDamageCalculator : MonoBehaviour
{
    public BattleUIManager battleUI;
    public float critRate;
    private float atkPwr, defPwr, critProbability, damageCalc;

    public void DealDamage(int target, int movePower, moveType m, elementType e, moveModifier mM)
    {
        atkPwr = 0;
        defPwr = 0;
        Battler charAttacker = BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn];
        Battler charTarget = BattleManager.instance.activeBattlers[target];
        switch (m)
        {
            case moveType.Attack:
                atkPwr = charAttacker.chara.strength + charAttacker.chara.wpnPower;
                defPwr = charTarget.chara.defence + charTarget.chara.armrPower;
                break;
            case moveType.Cypher:
                switch(mM)
                {
                    case moveModifier.Physical:
                        atkPwr = charAttacker.chara.strength + charAttacker.chara.wpnPower;
                        break;
                    case moveModifier.Element:
                        atkPwr = charAttacker.chara.element + charAttacker.chara.mgcPower;
                        break;
                }
                defPwr = charTarget.chara.spirit + charTarget.chara.mgcDefence;
                break;
        }
        damageCalc = ((atkPwr / defPwr) * movePower * (movePower * Random.Range(.65f, .75f))) * Random.Range(.5f, .75f);
        if (e != elementType.NonElemental) damageCalc = CalculateElementalResistance(target, damageCalc, e);
        critProbability = Random.Range(1, 101);
        if (critProbability >= critRate) damageCalc *= Random.Range(1.75f, 2.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);
        if (damageToGive < 0) damageToGive = 0;
        BattleManager.instance.activeBattlers[target].chara.currentHp -= damageToGive;
        if(BattleManager.instance.activeBattlers[target].chara.isPlayer)
        {
            if(BattleManager.instance.activeBattlers[target].chara.currentHp>0) BattleManager.instance.activeBattlers[target].chara.SetAnimatorTrigger("hitTrigger");
            else BattleManager.instance.activeBattlers[target].chara.SetAnimatorTrigger("deathTrigger");
        }
        else if(BattleManager.instance.CheckIfCurrentBattlerIsBoss(target))
        {
            BattleManager.instance.activeBattlers[target].GetComponent<BattleEnemyIA>().SetBossAnimationTrigger("damageTrigger");
        } 
        if (!AudioManager.instance.BattleSFX[2].isPlaying) AudioManager.instance.BattleSFX[2].Play();
        Instantiate(BattleManager.instance.damageNumber, BattleManager.instance.activeBattlers[target].transform.position, BattleManager.instance.activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        if (critProbability >= critRate) GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = Color.yellow;
        battleUI.UpdateUIStats(BattleManager.instance);
    }

    public float CalculateElementalResistance(int target, float movePower, elementType e)
    {
        Battler charTarget = BattleManager.instance.activeBattlers[target];
        if (!charTarget.chara.isPlayer)
        {
            if (charTarget.GetComponent<EnemyStats>().elementalType == e) movePower /= 2;
            if (charTarget.GetComponent<EnemyStats>().isWeakToElement == e) movePower *= 2;
        }

        switch (e)
        {
            case elementType.Fire:
                movePower -= ((movePower * charTarget.chara.fireResistance) / 50);
                break;
            case elementType.Ice:
                movePower -= ((movePower * charTarget.chara.iceResistance) / 50);
                break;
            case elementType.Electric:
                movePower -= ((movePower * charTarget.chara.electricResistance) / 50);
                break;
            case elementType.Plasma:
                movePower -= ((movePower * charTarget.chara.plasmaResistance) / 50);
                break;
            case elementType.Psychic:
                movePower -= ((movePower * charTarget.chara.psychicResistance) / 50);
                break;
            case elementType.Toxic:
                movePower -= ((movePower * charTarget.chara.toxicResistance) / 50);
                break;
        }

        return movePower;
    }
}

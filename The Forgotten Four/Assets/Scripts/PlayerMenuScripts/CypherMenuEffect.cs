using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CypherMenuEffect : MonoBehaviour
{
    public BattleMove move;

    public int HealEffect(CharStats targetCharacter, CharStats casterCharacter)
    {
        int amountToHeal = Mathf.FloorToInt(casterCharacter.GetCharStats().GetSpirit() * 1.5f + move.movePower);
        int currentHP = targetCharacter.GetCharStats().GetCurrentHP();
        currentHP += amountToHeal;
        if (currentHP > targetCharacter.GetCharStats().GetMaxHP()) targetCharacter.GetCharStats().SetCurrentHP((targetCharacter.GetCharStats().GetMaxHP()));
        else targetCharacter.GetCharStats().SetCurrentHP(currentHP);
        return amountToHeal;
    }

    public void SupportEffect()
    {

    }
}

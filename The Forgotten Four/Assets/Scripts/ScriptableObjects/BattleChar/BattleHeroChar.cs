using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Battle Objects/Create Battle Char/Hero", order = 2)]
public class BattleHeroChar : BattleChar
{
    public HeroCharSpecifics heroCharSpecifics;

    public void SetCyphersOnBattle(CharStats chara)
    {
        movesAvailable = chara.cypherList;
    }
}

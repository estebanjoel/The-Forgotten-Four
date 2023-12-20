using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

public class BattleChar : ScriptableObject
{
    public bool isPlayer;
    public List<string> movesAvailable = new List<string>();

    public string charName;
    [Header("Stats")]
    [SerializeField] Stats battleStats;
    public Stats BattleStats { get { return battleStats; } }
    
    [Header("Main Stats")]
    public int currentHp;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int strength;
    public int dexterity;
    public int vitality;
    public int defence;
    public int element;
    public int spirit;
    public int wpnPower;
    public int armrPower;
    [Header("Secondary Stats")]
    public int evasion;
    public int initiative;
    public int accuracy;
    public int mgcPower;
    public int mgcDefence;
    [Header("Elemental Resistances")]
    public int fireResistance;
    public int iceResistance;
    public int electricResistance;
    public int plasmaResistance;
    public int psychicResistance;
    public int toxicResistance;

    public bool hasDied;
    public bool hasAnAlteratedStat;
    public int alteratedStatTimer;
    public List<int> previousAlteratedStats = new List<int>();
    public List<statTarget> previousAlteratedStatsName = new List<statTarget>();

    //public Sprite deadSprite, aliveSprite;

    //public Animator anim;


    public void SetCharacterStats(Stats myChar, bool player)
    {
        charName = myChar.charName;
        currentHp = myChar.GetCurrentHP();
        maxHP = myChar.GetMaxHP();
        currentMP = myChar.GetCurrentMP();
        maxMP = myChar.GetMaxMP();
        strength = myChar.GetStrength();
        dexterity = myChar.GetDexterity();
        vitality = myChar.GetVitality();
        defence = myChar.GetDefence();
        element = myChar.GetElement();
        spirit = myChar.GetSpirit();
        wpnPower = myChar.GetWeaponPower();
        armrPower = myChar.GetArmourPower();
        evasion = myChar.GetEvasion();
        initiative = myChar.GetInitiative();
        accuracy = myChar.GetAccuracy();
        mgcPower = myChar.GetMagicPower();
        mgcDefence = myChar.GetMagicDefence();
        fireResistance = myChar.GetFireResistance();
        iceResistance = myChar.GetIceResistance();
        electricResistance = myChar.GetElectricResistance();
        plasmaResistance = myChar.GetPlasmaResistance();
        psychicResistance = myChar.GetPsychicResistance();
        toxicResistance = myChar.GetToxicResistance();
        isPlayer = player;
    }

    public void changeAlteredStatBool(bool flag)
    {
        hasAnAlteratedStat = flag;
    }

    public void setAlteredStatTimer(int timer)
    {
        alteratedStatTimer = timer;
    }
}

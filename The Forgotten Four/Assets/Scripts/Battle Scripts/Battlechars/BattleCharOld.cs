using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

public abstract class BattleCharOld : MonoBehaviour
{
    public bool isPlayer;
    public List<string> movesAvailable = new List<string>();

    public string charName;
    [Header("Stats")]
    [SerializeField] Stats battleStats;
    
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

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

    public Animator anim;

    void Start()
    {
    }


    public void SetCharacterStats(CharStats myChar)
    {
        charName = myChar.charName;
        currentHp = myChar.GetCharStats().GetCurrentHP();
        maxHP = myChar.GetCharStats().GetMaxHP();
        currentMP = myChar.GetCharStats().GetCurrentMP();
        maxMP = myChar.GetCharStats().GetMaxMP();
        strength = myChar.GetCharStats().GetStrength();
        dexterity = myChar.GetCharStats().GetDexterity();
        vitality = myChar.GetCharStats().GetVitality();
        defence = myChar.GetCharStats().GetDefence();
        element = myChar.GetCharStats().GetElement();
        spirit = myChar.GetCharStats().GetSpirit();
        wpnPower = myChar.GetCharStats().GetWeaponPower();
        armrPower = myChar.GetCharStats().GetArmourPower();
        evasion = myChar.GetCharStats().GetEvasion();
        initiative = myChar.GetCharStats().GetInitiative();
        accuracy = myChar.GetCharStats().GetAccuracy();
        mgcPower = myChar.GetCharStats().GetMagicPower();
        mgcDefence = myChar.GetCharStats().GetMagicDefence();
        fireResistance = myChar.GetCharStats().GetFireResistance();
        iceResistance = myChar.GetCharStats().GetIceResistance();
        electricResistance = myChar.GetCharStats().GetElectricResistance();
        plasmaResistance = myChar.GetCharStats().GetPlasmaResistance();
        psychicResistance = myChar.GetCharStats().GetPsychicResistance();
        toxicResistance = myChar.GetCharStats().GetToxicResistance();
        movesAvailable = myChar.cypherList;
        isPlayer = true;
    }

    public void SetAnimatorTrigger(string t)
    {
        anim.SetTrigger(t);
    }

    public bool IsCharDeathAnimationActive()
    {
        if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Death") return true;
        else return false;
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

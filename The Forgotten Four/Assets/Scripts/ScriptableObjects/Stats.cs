using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Create Stats", order = 0)]
public class Stats : ScriptableObject
{
    public string charName;
    [Header("HP & MP")]
    [SerializeField] int currentHP;
    [SerializeField] int maxHP;
    [SerializeField] int currentMP;
    [SerializeField] int maxMP;
    [Header("Main Stats")]
    [SerializeField] int strength;
    [SerializeField] int dexterity;
    [SerializeField] int vitality;
    [SerializeField] int defence;
    [SerializeField] int element;
    [SerializeField] int spirit;

    [Header("Secondary Stats")]
    [SerializeField] int weaponPower;
    [SerializeField] int armourPower;
    [SerializeField] int evasion;
    [SerializeField] int initiative;
    [SerializeField] int accuracy;
    [SerializeField] int magicPower;
    [SerializeField] int magicDefence;

    [Header("Elemental Resitances")]
    [SerializeField] int fireResistance;
    [SerializeField] int iceResistance;
    [SerializeField] int electricResistance;
    [SerializeField] int plasmaResistance;
    [SerializeField] int psychicResistance;
    [SerializeField] int toxicResistance;

    #region StatsGetters
    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public int GetCurrentMP()
    {
        return currentMP;
    }

    public int GetMaxMP()
    {
        return maxMP;
    }

    public int GetStrength()
    {
        return strength;
    }

    public int GetDexterity()
    {
        return dexterity;
    }

    public int GetVitality()
    {
        return vitality;
    }

    public int GetDefence()
    {
        return defence;
    }

    public int GetElement()
    {
        return element;
    }

    public int GetSpirit()
    {
        return spirit;
    }

    public int GetWeaponPower()
    {
        return weaponPower;
    }

    public int GetArmourPower()
    {
        return armourPower;
    }

    public int GetEvasion()
    {
        return evasion;
    }

    public int GetInitiative()
    {
        return initiative;
    }

    public int GetAccuracy()
    {
        return accuracy;
    }

    public int GetMagicPower()
    {
        return magicPower;
    }

    public int GetMagicDefence()
    {
        return magicDefence;
    }

    public int GetFireResistance()
    {
        return fireResistance;
    }

    public int GetIceResistance()
    {
        return iceResistance;
    }

    public int GetElectricResistance()
    {
        return electricResistance;
    }

    public int GetPlasmaResistance()
    {
        return plasmaResistance;
    }

    public int GetPsychicResistance()
    {
        return psychicResistance;
    }

    public int GetToxicResistance()
    {
        return toxicResistance;
    }

    #endregion

    #region StatsSetters

    public void SetCurrentHP(int hp)
    {  
        currentHP = hp; 
    }

    public void SetMaxHP(int hp)
    {
        maxHP = hp;
    }

    public void SetCurrentMP(int mp)
    {
        currentMP = mp;
    }

    public void SetMaxMP(int mp)
    {
        maxMP = mp;
    }

    public void SetStrength(int stat)
    {
        strength = stat;
    }

    public void SetDexterity(int stat)
    {
        dexterity = stat;
    }

    public void SetVitality(int stat)
    {
        vitality = stat;
    }

    public void SetDefence(int stat)
    {
        defence = stat;
    }

    public void SetElement(int stat)
    {
        element = stat;
    }

    public void SetSpirit(int stat)
    {
        spirit = stat;
    }

    public void SetWeaponPower(int stat)
    {
        weaponPower = stat;
    }

    public void SetArmourPower(int stat)
    {
        armourPower = stat;
    }

    public void SetEvasion(int stat)
    {
        evasion = stat;
    }

    public void SetInitiative(int stat)
    {
        initiative = stat;
    }

    public void SetAccuracy(int stat)
    {
        accuracy = stat;
    }

    public void SetMagicPower(int stat)
    {
        magicPower = stat;
    }

    public void SetMagicDefence(int stat)
    {
        magicDefence = stat;
    }

    public void SetFireResistance(int resistance)
    {
        fireResistance = resistance;
    }

    public void SetIceResistance(int resistance)
    {
        iceResistance = resistance;
    }

    public void SetElectricResistance(int resistance)
    {
        electricResistance = resistance;
    }

    public void SetPlasmaResistance(int resistance)
    {
        plasmaResistance = resistance;
    }

    public void SetPsychicResistance(int resistance)
    {
        psychicResistance = resistance;
    }

    public void SetToxicResistance(int resistance)
    {
        toxicResistance = resistance;
    }
    
    #endregion

}

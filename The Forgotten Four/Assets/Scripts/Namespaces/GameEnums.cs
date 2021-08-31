using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnums
{
    public enum characterClass
    {
        Soldier,
        Mage,
        Healer,
        Tank
    }
    
    public enum elementType
    {
        None,
        NonElemental,
        Fire,
        Ice,
        Electric,
        Plasma,
        Psychic,
        Toxic,
        Heal,
        Support
    }

    public enum moveType
    {
        Attack,
        Cypher
    }

    public enum moveModifier
    {
        Element,
        Physical
    }

    public enum moveTarget
    {
        Single,
        All,
        Self
    }

    public enum statTarget
    {
        None,
        Strength,
        Dexterity,
        Vitality,
        Defence,
        Element,
        Spirit,
        WeaponPower,
        ArmorPower,
        MagicPower,
        MagicDefence,
        Accuracy,
        Evasion,
        Initiative,
        FireResistance,
        IceResistance,
        ElectricResistance,
        PlasmaResistance,
        PsychicResistance,
        ToxicResistance,
        HP,
        MP
    }

    public enum equipItemCharTarget
    {
        None,
        Anyone,
        Heavy,
        Soft,
        Sanji,
        Kenya,
        Yakov,
        RX10
    }
}
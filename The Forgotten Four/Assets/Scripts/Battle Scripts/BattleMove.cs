using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

[System.Serializable]
public class BattleMove
{
    public string moveName;
    public moveType mType;
    public elementType eType;
    public moveModifier mModifier;
    public moveTarget mTarget;
    public statTarget sTarget;
    public int movePower;
    public int moveCost;
    public AttackEffect myEffect;
    public AudioClip myAudioClip;
    public string description;
}

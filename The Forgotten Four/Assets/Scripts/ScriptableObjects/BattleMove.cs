using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

[CreateAssetMenu(fileName = "BattleMove", menuName = "Improvus/Battle Objects/Create Battle Move", order = 3)]
public class BattleMove: ScriptableObject
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

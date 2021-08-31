using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBG : MonoBehaviour
{
    [SerializeField] Sprite spriteBG;

    public Sprite GetSprite()
    {
        return spriteBG;
    }
}
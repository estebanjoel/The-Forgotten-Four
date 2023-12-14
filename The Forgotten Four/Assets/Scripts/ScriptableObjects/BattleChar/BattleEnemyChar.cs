using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Improvus/Create Battle Char/Enemy", order = 2)]
public class BattleEnemyChar : BattleChar
{
    [Header ("EnemyChar Specific Variables")]
    public bool shouldFade;
    public float fadeSpeed = 1f;
    public bool isBoss;
    public EnemyCharSpecifics enemyCharSpecifics;
    /*void Update()
    {
        if (shouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }*/
}

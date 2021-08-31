using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    public int currentHP;
    public int maxHP;
    private int topHP = 9999;
    public int currentMP;
    public int maxMP;
    private int topMP = 999;
    public int[] mpLvlBonus;
    public int strength;
    public int dexterity;
    public int vitality;
    public int defence;
    public int element;
    public int spirit;
    public int wpnPwr;
    public int armrPwr;
    public string equippedWpn;
    public string equippedArmr;
    public Sprite charIamge;

    // Use this for initialization
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }

        maxHP+=Mathf.FloorToInt(100+(vitality*5*Random.Range(1.5f,1.9f)));
        currentHP = maxHP;
        maxMP += 30 + Mathf.FloorToInt(element*2.5f);
        currentMP = maxMP;
        wpnPwr += strength / 2;
        armrPwr += defence / 2;

        string[] myItems = GameManager.instance.itemsHeld;
        for(int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i] == equippedWpn)
            {
                wpnPwr += GameManager.instance.referenceItems[i].weaponStrength;
            }
        }
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i] == equippedArmr)
            {
                armrPwr += GameManager.instance.referenceItems[i].armorStrength;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f + (vitality * 2.5f));
                if (maxHP > topHP)
                {
                    maxHP = topHP;
                }
                currentHP = maxHP;

                maxMP += Mathf.FloorToInt(element * 1.05f);
                if(maxMP > topMP)
                {
                    maxMP = topMP;
                }
                currentMP = maxMP;
            }
        }

        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}

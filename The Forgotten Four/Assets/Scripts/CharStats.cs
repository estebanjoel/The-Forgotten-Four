using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

public class CharStats : MonoBehaviour
{
    public string charName;
    public characterClass charClass;
    [Header("Level & XP")]
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    [Header("Stats")]
    [SerializeField] Stats charStat;
     
    [Header("HP & MP")]
    // public int currentHP;
    // public int maxHP;
    private int topHP = 9999;
    // public int currentMP;
    // public int maxMP;
    private int topMP = 999;
    // [Header("Main Stats")]
    // // public int strength;
    // // public int dexterity;
    // // public int vitality;
    // // public int defence;
    // // public int element;
    // // public int spirit;
    // [Header("Secondary Stats")]
    // public int wpnPwr;
    // public int armrPwr;
    // public int evasion;
    // public int initiative;
    // public int accuracy;
    // public int mgcPower;
    // public int mgcDefence;
    // [Header("Elemental Resitances")]
    // public int fireResistance;
    private int topResistance = 100;
    // public int iceResistance;
    // private int topIceResistance = 100;
    // // public int electricResistance;
    // private int topElectricResistance = 100;
    // // public int plasmaResistance;
    // private int topPlasmaResistance = 100;
    // // public int psychicResistance;
    // private int topPsychicResistance = 100;
    // // public int toxicResistance;
    // private int topToxicResistance = 100;
    [Header("Equip")]
    public string equippedWpn;
    public string equippedArmr;
    [Header("LevelUpModifiers")]
    public int strengthModifier;
    public int dexterityModifier;
    public int vitalityModifier;
    public int defenceModifier;
    public int elementModifier;
    public int spiritModifier;
    public int hpModifier;
    public int mpModifier;
    public int weaponPowerModifier;
    public int armorPowerModifier;
    public int magicPowerModifier;
    public int magicDefenceModifier;
    public int accuracyModifier;
    public int evasionModifier;
    public int initiativeModifier;
    public int ResistanceModifier;
    public int fireResistanceModifier;
    public int iceResistanceModifier;
    public int electricResistanceModifier;
    public int plasmaResistanceModifier;
    public int psychicResistanceModifier;
    public int toxicResistanceModifier;
    [Header("Others")]
    public Sprite charImage;
    public List<string> cypherList = new List<string>();
    public string[] CypherRewards;

    #if UNITY_EDITOR
    public int equippedWpnIndex;
    public int equippedArmrIndex;
    public int currentCypherIndex;
    #endif

    // Use this for initialization
    void Start()
    {
        SetLevelUPRate();
        AssignStatsOnFirstLevel();
        setWeaponAndArmor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckExp();
    }

    public Stats GetCharStats()
    {
        return charStat;
    }

    public void SetCharStats(Stats stats)
    {
        charStat = stats;
    }

    public void AssignStatsOnFirstLevel()
    {
        int maxHP = charStat.GetMaxHP();
        maxHP += Mathf.FloorToInt(100 + (GetCharStats().GetVitality() * 5 * Random.Range(1.5f, 1.9f)));
        charStat.SetMaxHP(maxHP);
        charStat.SetCurrentHP(charStat.GetMaxHP());
        int maxMP = charStat.GetMaxMP();
        maxMP += 30 + Mathf.FloorToInt(GetCharStats().GetElement() * 2.5f);
        charStat.SetMaxMP(maxMP);
        charStat.SetCurrentMP(charStat.GetMaxMP());
        setSecondaryStats();
        setElementalResistances();
    }

    public void SetLevelUPRate()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    public void setWeaponAndArmor()
    {
        string[] myItems = GameManager.instance.itemsHeld;
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i] == equippedWpn)
            {
                EquipItem myEquippedItem = (EquipItem) GameManager.instance.referenceItems[i];
                int wpnPwr = GetCharStats().GetWeaponPower();
                wpnPwr += myEquippedItem.weaponStrength;
                GetCharStats().SetWeaponPower(wpnPwr);
            }
        }
        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i] == equippedArmr)
            {
                EquipItem myEquippedItem = (EquipItem) GameManager.instance.referenceItems[i];
                int armrPwr = GetCharStats().GetArmourPower();
                armrPwr += myEquippedItem.armourStrength;
                GetCharStats().SetArmourPower(armrPwr);
            }
        }
    }

    public void setSecondaryStats()
    {
        int wpnPwr = GetCharStats().GetWeaponPower();
        wpnPwr += GetCharStats().GetStrength() / 2;
        GetCharStats().SetWeaponPower(wpnPwr);

        int armrPwr = GetCharStats().GetArmourPower();
        armrPwr += GetCharStats().GetDefence() / 2;
        GetCharStats().SetArmourPower(armrPwr);

        int evasion = GetCharStats().GetEvasion();
        evasion += Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.5f, 0.75f));
        GetCharStats().SetEvasion(evasion);

        int initiative = GetCharStats().GetInitiative();
        initiative += Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.5f, 0.1f));
        GetCharStats().SetInitiative(initiative);

        int accuracy = GetCharStats().GetAccuracy();
        accuracy += Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.5f, 0.75f));
        GetCharStats().SetAccuracy(accuracy);

        int mgcPower = GetCharStats().GetMagicPower();
        mgcPower += Mathf.FloorToInt(GetCharStats().GetElement() * Random.Range(0.5f, 0.75f));
        GetCharStats().SetMagicPower(mgcPower);

        int mgcDefence = GetCharStats().GetMagicDefence();
        mgcDefence += Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0.5f, 0.75f));
        GetCharStats().SetMagicDefence(mgcDefence);
    }

    public void setElementalResistances()
    {
        int resistance = GetCharStats().GetFireResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetFireResistance(resistance);

        resistance = GetCharStats().GetIceResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetIceResistance(resistance);

        resistance = GetCharStats().GetElectricResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetElectricResistance(resistance);

        resistance = GetCharStats().GetPlasmaResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetPlasmaResistance(resistance);

        resistance = GetCharStats().GetPsychicResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetPsychicResistance(resistance);

        resistance = GetCharStats().GetToxicResistance();
        resistance += Mathf.FloorToInt(GetCharStats().GetSpirit() / 2);
        GetCharStats().SetToxicResistance(resistance);
    }

    public void restoreHP()
    {
        charStat.SetCurrentHP(charStat.GetMaxHP());
    }

    public void restoreMP()
    {
        charStat.SetCurrentMP(charStat.GetMaxMP());
    }
    
    public void CheckExp()
    {
        if (playerLevel < maxLevel)
        {
            if (currentEXP >= expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];
                LevelUP();
            }
        }

        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }

    public void LevelUP()
    {
        playerLevel++;
        if (playerLevel % 3 == 0)
        {
            SetStatsPerClass();
        }
        AddHpAndMp();
        AddSecondaryStatsPoints();
        AddElementalResistances();
        AddCypherRewards();
    }

    public void AddHpAndMp()
    {
        hpModifier = Mathf.FloorToInt(GetCharStats().GetVitality() * 2.5f);
        int maxHP = charStat.GetMaxHP();
        maxHP = Mathf.FloorToInt(maxHP * 1.05f + hpModifier);
        if (maxHP > topHP)
        {
            maxHP = topHP;
        }
        charStat.SetMaxHP(maxHP);
        charStat.SetCurrentHP(charStat.GetMaxHP());

        mpModifier = Mathf.FloorToInt(GetCharStats().GetElement() * 1.05f);
        int maxMP = charStat.GetMaxMP();
        maxMP += mpModifier;
        if (maxMP > topMP)
        {
            maxMP = topMP;
        }
        charStat.SetMaxMP(maxMP);
        charStat.SetCurrentMP(charStat.GetMaxMP());
    }

    public void SetStatsPerClass()
    {
        switch (charClass)
        {
            case characterClass.Soldier:
                SetStatsModifier(true, false, false, false);
                break;
            case characterClass.Mage:
                SetStatsModifier(false, true, false, false);
                break;
            case characterClass.Healer:
                SetStatsModifier(false, false, true, false);
                break;
            case characterClass.Tank:
                SetStatsModifier(false, false, false, true);
                break;
        }
    }

    public void SetStatsModifier(bool soldier, bool mage, bool healer, bool tank)
    {
        if (soldier)
        {
            AddMainStatsPoints(2, 3, 2, 2, 1, 1);
        }

        if (mage)
        {
            AddMainStatsPoints(1, 1, 3, 2, 3, 2);
        }

        if (healer)
        {
            AddMainStatsPoints(1, 2, 1, 1, 2, 3);
        }

        if (tank)
        {
            AddMainStatsPoints(2, 1, 3, 3, 1, 2);
        }
    }

    public void AddMainStatsPoints(int str, int dex, int vit, int def, int ele, int spt)
    {
        strengthModifier = Random.Range(str, 4);
        strengthModifier += GetCharStats().GetStrength();
        GetCharStats().SetStrength(strengthModifier);

        dexterityModifier = Random.Range(dex, 4);
        dexterityModifier += GetCharStats().GetDexterity();
        GetCharStats().SetDexterity(dexterityModifier);

        vitalityModifier = Random.Range(vit, 4);
        vitalityModifier += GetCharStats().GetVitality();
        GetCharStats().SetVitality(vitalityModifier);

        defenceModifier = Random.Range(def, 4);
        defenceModifier += GetCharStats().GetDefence();
        GetCharStats().SetDefence(defenceModifier);

        elementModifier = Random.Range(ele, 4);
        elementModifier += GetCharStats().GetElement();
        GetCharStats().SetElement(elementModifier);

        spiritModifier = Random.Range(spt, 4);
        spiritModifier += GetCharStats().GetSpirit();
        GetCharStats().SetSpirit(spiritModifier);
    }

    public void AddSecondaryStatsPoints()
    {
        weaponPowerModifier = Mathf.FloorToInt(GetCharStats().GetStrength()*Random.Range(0.25f,0.5f));
        weaponPowerModifier += GetCharStats().GetWeaponPower();
        GetCharStats().SetWeaponPower(weaponPowerModifier);

        armorPowerModifier = Mathf.FloorToInt(GetCharStats().GetDefence() * Random.Range(0.25f, 0.5f));
        armorPowerModifier += GetCharStats().GetArmourPower();
        GetCharStats().SetArmourPower(armorPowerModifier);

        evasionModifier = Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.125f, 0.25f));
        evasionModifier += GetCharStats().GetEvasion();
        GetCharStats().SetEvasion(evasionModifier);

        initiativeModifier = Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.25f, 0.5f));
        initiativeModifier += GetCharStats().GetInitiative();
        GetCharStats().SetInitiative(initiativeModifier);

        accuracyModifier = Mathf.FloorToInt(GetCharStats().GetDexterity() * Random.Range(0.125f, 0.25f));
        accuracyModifier += GetCharStats().GetAccuracy();
        GetCharStats().SetAccuracy(accuracyModifier);

        magicPowerModifier = Mathf.FloorToInt(GetCharStats().GetElement() * Random.Range(0.125f, 0.25f));
        magicPowerModifier += GetCharStats().GetMagicPower();
        GetCharStats().SetMagicPower(magicPowerModifier);


        magicDefenceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0.125f, 0.25f));
        magicDefenceModifier += GetCharStats().GetMagicDefence();
        GetCharStats().SetMagicDefence(magicDefenceModifier);
    }

    public void AddElementalResistances()
    {
        fireResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        fireResistanceModifier += GetCharStats().GetFireResistance();
        if (fireResistanceModifier > topResistance) fireResistanceModifier = topResistance;
        GetCharStats().SetFireResistance(fireResistanceModifier);

        iceResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        iceResistanceModifier += GetCharStats().GetIceResistance();
        if (iceResistanceModifier > topResistance) iceResistanceModifier = topResistance;
        GetCharStats().SetIceResistance(iceResistanceModifier);

        electricResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        electricResistanceModifier += GetCharStats().GetElectricResistance();
        if (electricResistanceModifier > topResistance) electricResistanceModifier = topResistance;
        GetCharStats().SetElectricResistance(electricResistanceModifier);

        plasmaResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        plasmaResistanceModifier += GetCharStats().GetPlasmaResistance();
        if (plasmaResistanceModifier > topResistance) plasmaResistanceModifier = topResistance;
        GetCharStats().SetPlasmaResistance(plasmaResistanceModifier);
        
        psychicResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        psychicResistanceModifier += GetCharStats().GetPsychicResistance();
        if (psychicResistanceModifier > topResistance) psychicResistanceModifier = topResistance;
        GetCharStats().SetPsychicResistance(psychicResistanceModifier);

        toxicResistanceModifier = Mathf.FloorToInt(GetCharStats().GetSpirit() * Random.Range(0, 0.25f));
        toxicResistanceModifier += GetCharStats().GetToxicResistance();
        if (toxicResistanceModifier > topResistance) toxicResistanceModifier = topResistance;
        GetCharStats().SetToxicResistance(toxicResistanceModifier);
        
    }

    //Función utilizable para cuando se una un personaje a la party y así comience con el mismo nivel de la party
    public void MatchCurrentMaxLevel()
    {
        int currentMaxLevel = 1;
        for(int i = 0; i < PlayerController.instance.partyStats.Length; i++)
        {
            if(currentMaxLevel< PlayerController.instance.partyStats[i].playerLevel && PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                currentMaxLevel = PlayerController.instance.partyStats[i].playerLevel;
            }
        }
        while (playerLevel < currentMaxLevel)
        {
            LevelUP();
        }
    }

    public void AddCypherRewards()
    {
        if(playerLevel<5 && playerLevel>1) cypherList.Add(CypherRewards[playerLevel-2]);
    }
}

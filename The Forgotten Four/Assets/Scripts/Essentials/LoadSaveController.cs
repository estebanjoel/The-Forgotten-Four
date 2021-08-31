using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaveController : MonoBehaviour
{
    public static LoadSaveController instance;
    public int activeSlot;
    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if(instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     SaveGame();
        // }
        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     LoadGame();
        // }
    }

    public void SaveGame(int slot)
    {
        PlayerPrefs.SetInt("Saved_Slot_"+slot,slot);
        
        #region GameManagerInfo
        PlayerPrefs.SetString("Saved_Slot_"+slot+"_Current_Scene", GameManager.instance.levelManager.currentScene);
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Start_Position_x", GameManager.instance.lastCheckPoint.position.x);
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Start_Position_y", GameManager.instance.lastCheckPoint.position.y);
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Start_Position_z", GameManager.instance.lastCheckPoint.position.z);
        #endregion
        
        #region PlayerControllerInfo
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_z", PlayerController.instance.transform.position.z);
        #endregion

        #region CharStatsInfo
        for(int i = 0; i<PlayerController.instance.partyStats.Length;i++)
        {
            if(PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_"+PlayerController.instance.partyStats[i].charName+"_Active","yes");
                //Main Stats
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Level", PlayerController.instance.partyStats[i].playerLevel);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_XP", PlayerController.instance.partyStats[i].currentEXP);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_XP_To_Next_Level", PlayerController.instance.partyStats[i].expToNextLevel[PlayerController.instance.partyStats[i].playerLevel]);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_HP", PlayerController.instance.partyStats[i].GetCharStats().GetCurrentHP());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Max_HP", PlayerController.instance.partyStats[i].GetCharStats().GetMaxHP());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_MP", PlayerController.instance.partyStats[i].GetCharStats().GetCurrentMP());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Max_MP", PlayerController.instance.partyStats[i].GetCharStats().GetMaxMP());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_strength", PlayerController.instance.partyStats[i].GetCharStats().GetStrength());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_dexterity", PlayerController.instance.partyStats[i].GetCharStats().GetDexterity());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_vitality", PlayerController.instance.partyStats[i].GetCharStats().GetVitality());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_defence", PlayerController.instance.partyStats[i].GetCharStats().GetDefence());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_element", PlayerController.instance.partyStats[i].GetCharStats().GetElement());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_spirit", PlayerController.instance.partyStats[i].GetCharStats().GetSpirit());
                //Secondary Stats
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_wpnPwr", PlayerController.instance.partyStats[i].GetCharStats().GetWeaponPower());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_armrPwr", PlayerController.instance.partyStats[i].GetCharStats().GetArmourPower());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_evasion", PlayerController.instance.partyStats[i].GetCharStats().GetEvasion());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_initiative", PlayerController.instance.partyStats[i].GetCharStats().GetInitiative());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_accuracy", PlayerController.instance.partyStats[i].GetCharStats().GetAccuracy());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_mgcPower", PlayerController.instance.partyStats[i].GetCharStats().GetMagicPower());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_mgcDefence", PlayerController.instance.partyStats[i].GetCharStats().GetMagicDefence());
                //Elemental Resistances
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_fireResistance", PlayerController.instance.partyStats[i].GetCharStats().GetFireResistance());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_iceResistance", PlayerController.instance.partyStats[i].GetCharStats().GetIceResistance());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_electricResistance", PlayerController.instance.partyStats[i].GetCharStats().GetElectricResistance());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_plasmaResistance", PlayerController.instance.partyStats[i].GetCharStats().GetPlasmaResistance());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_psychicResistance", PlayerController.instance.partyStats[i].GetCharStats().GetPsychicResistance());
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_toxicResistance", PlayerController.instance.partyStats[i].GetCharStats().GetToxicResistance());
                // Equipped Items
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_equippedWpn", PlayerController.instance.partyStats[i].equippedWpn);
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_equippedArmr", PlayerController.instance.partyStats[i].equippedArmr);
                // Main Stats Modifiers
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_hp_Modifier", PlayerController.instance.partyStats[i].hpModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mp_Modifier", PlayerController.instance.partyStats[i].mpModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_strength_Modifier", PlayerController.instance.partyStats[i].strengthModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_dexterity_Modifier", PlayerController.instance.partyStats[i].dexterityModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_vitality_Modifier", PlayerController.instance.partyStats[i].vitalityModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_defence_Modifier", PlayerController.instance.partyStats[i].defenceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_element_Modifier", PlayerController.instance.partyStats[i].elementModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_spirit_Modifier", PlayerController.instance.partyStats[i].spiritModifier);
                // Secondary Stats Modifiers
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_wpnPwr_Modifier", PlayerController.instance.partyStats[i].weaponPowerModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_armrPwr_Modifier", PlayerController.instance.partyStats[i].armorPowerModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_evasion_Modifier", PlayerController.instance.partyStats[i].evasionModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_initiative_Modifier", PlayerController.instance.partyStats[i].initiativeModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_accuracy_Modifier", PlayerController.instance.partyStats[i].accuracyModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mgcPower_Modifier", PlayerController.instance.partyStats[i].magicPowerModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mgcDefence_Modifier", PlayerController.instance.partyStats[i].magicDefenceModifier);
                // Elemental Resistances Modifiers
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_fireResistance_Modifier", PlayerController.instance.partyStats[i].fireResistanceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_iceResistance_Modifier", PlayerController.instance.partyStats[i].iceResistanceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_electricResistance_Modifier", PlayerController.instance.partyStats[i].electricResistanceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_plasmaResistance_Modifier", PlayerController.instance.partyStats[i].plasmaResistanceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_psychicResistance_Modifier", PlayerController.instance.partyStats[i].psychicResistanceModifier);
                PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_toxicResistance_Modifier", PlayerController.instance.partyStats[i].toxicResistanceModifier);
                //Cypher Rewards
                for(int j = 0; j<PlayerController.instance.partyStats[i].CypherRewards.Length;j++)
                {
                    PlayerPrefs.SetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Cypher_Reward_"+j, PlayerController.instance.partyStats[i].CypherRewards[j]);
                }
            }
            else
            {
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_"+PlayerController.instance.partyStats[i].charName+"_Active","no");
            }
        }
        #endregion

        #region CurrentLevelInfo
        if(GameManager.instance.bossIsDead) PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_The_Boss_Dead", "yes");
        else PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_The_Boss_Dead", "no");
        for(int i = 0; i < GameManager.instance.itemsHeld.Length;i++)
        {
            PlayerPrefs.SetString("Saved_Slot_"+slot+"_Item_"+i, GameManager.instance.itemsHeld[i]);
        }
        for(int i = 0; i < GameManager.instance.numberOfItems.Length;i++)
        {
            PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Item_"+i+"_Quantity", GameManager.instance.numberOfItems[i]);
        }
        PlayerPrefs.SetInt("Saved_Slot_"+slot+"_Credits", GameManager.instance.credits);
        for(int i = 0; i < GameManager.instance.levelCrates.Count;i++)
        {
            if(GameManager.instance.levelCrates[i].isActive) PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_Crate_"+i+"_Active", "yes");
            else PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_Crate_"+i+"_Active", "no");
        }
        for(int i = 0; i < GameManager.instance.restorePoints.Count; i++)
        {
            if(GameManager.instance.restorePoints[i].hasBeenActivated) PlayerPrefs.SetString("Saved_Slot_"+slot+"_Restore_Point_"+i+"_Activated", "yes");
            else PlayerPrefs.SetString("Saved_Slot_"+slot+"_Restore_Point_"+i+"_Activated", "no");
        }
        for(int i = 0; i < GameManager.instance.enemiesOnScreen.Length;i++)
        {
            if(GameManager.instance.enemiesOnScreen[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_Enemy_"+i+"_Active", "yes");
            } 
            else
            {
                PlayerPrefs.SetString("Saved_Slot_"+slot+"_Is_Enemy_"+i+"_Active", "no");
            }
        }
        #endregion

        print("Game Saved");
    }

    public void LoadGame(int slot)
    {
        #region GameManagerInfo
        if(GameManager.instance.levelManager.currentScene != PlayerPrefs.GetString("Saved_Slot_"+slot+"_Current_Scene")) GameManager.instance.levelManager.LoadLevel(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Current_Scene"));
        GameManager.instance.lastCheckPoint.position = new Vector3(PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Start_Position_x"), PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Start_Position_y"), PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Start_Position_z"));
        #endregion
        
        #region PlayerControllerInfo
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_x"),PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_y"),PlayerPrefs.GetFloat("Saved_Slot_"+slot+"_Saved_Player_Pos_z"));
        #endregion

        #region CharStatsInfo
        for(int i = 0; i<PlayerController.instance.partyStats.Length;i++)
        {
            if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_"+PlayerController.instance.partyStats[i].charName+"_Active") == "yes")
            {
                // Main Stats
                PlayerController.instance.partyStats[i].gameObject.SetActive(true);
                PlayerController.instance.partyStats[i].playerLevel = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Level");
                PlayerController.instance.partyStats[i].currentEXP = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_XP");
                PlayerController.instance.partyStats[i].expToNextLevel[PlayerController.instance.partyStats[i].playerLevel] = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_XP_To_Next_Level");
                PlayerController.instance.partyStats[i].GetCharStats().SetCurrentHP(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_HP"));
                PlayerController.instance.partyStats[i].GetCharStats().SetMaxHP(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Max_HP"));
                PlayerController.instance.partyStats[i].GetCharStats().SetCurrentMP(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_MP"));
                PlayerController.instance.partyStats[i].GetCharStats().SetMaxMP(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Max_MP"));
                PlayerController.instance.partyStats[i].GetCharStats().SetStrength(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_strength"));
                PlayerController.instance.partyStats[i].GetCharStats().SetDexterity(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_dexterity"));
                PlayerController.instance.partyStats[i].GetCharStats().SetVitality(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_vitality"));
                PlayerController.instance.partyStats[i].GetCharStats().SetDefence(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_defence"));
                PlayerController.instance.partyStats[i].GetCharStats().SetElement(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_element"));
                PlayerController.instance.partyStats[i].GetCharStats().SetSpirit(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_spirit"));
                // Secondary Stats
                PlayerController.instance.partyStats[i].GetCharStats().SetWeaponPower(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_wpnPwr"));
                PlayerController.instance.partyStats[i].GetCharStats().SetArmourPower(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_armrPwr"));
                PlayerController.instance.partyStats[i].GetCharStats().SetEvasion(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_evasion"));
                PlayerController.instance.partyStats[i].GetCharStats().SetInitiative(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_initiative"));
                PlayerController.instance.partyStats[i].GetCharStats().SetAccuracy(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_accuracy"));
                PlayerController.instance.partyStats[i].GetCharStats().SetMagicPower(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_mgcPower"));
                PlayerController.instance.partyStats[i].GetCharStats().SetMagicDefence(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_mgcDefence"));
                // Elemental Resistances
                PlayerController.instance.partyStats[i].GetCharStats().SetFireResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_fireResistance"));
                PlayerController.instance.partyStats[i].GetCharStats().SetIceResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_iceResistance"));
                PlayerController.instance.partyStats[i].GetCharStats().SetElectricResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_electricResistance"));
                PlayerController.instance.partyStats[i].GetCharStats().SetPlasmaResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_plasmaResistance"));
                PlayerController.instance.partyStats[i].GetCharStats().SetPsychicResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_psychicResistance"));
                PlayerController.instance.partyStats[i].GetCharStats().SetToxicResistance(PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_toxicResistance"));
                // Equipped Items
                PlayerController.instance.partyStats[i].equippedWpn = PlayerPrefs.GetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_equippedWpn");
                PlayerController.instance.partyStats[i].equippedArmr = PlayerPrefs.GetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Current_equippedArmr");
                // Main Stats Modifiers
                PlayerController.instance.partyStats[i].hpModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_hp_Modifier");
                PlayerController.instance.partyStats[i].mpModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mp_Modifier");
                PlayerController.instance.partyStats[i].strengthModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_strength_Modifier");
                PlayerController.instance.partyStats[i].dexterityModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_dexterity_Modifier");
                PlayerController.instance.partyStats[i].vitalityModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_vitality_Modifier");
                PlayerController.instance.partyStats[i].defenceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_defence_Modifier");
                PlayerController.instance.partyStats[i].elementModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_element_Modifier");
                PlayerController.instance.partyStats[i].spiritModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_spirit_Modifier");
                // Secondary Stats Modifiers
                PlayerController.instance.partyStats[i].weaponPowerModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_wpnPwr_Modifier");
                PlayerController.instance.partyStats[i].armorPowerModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_armrPwr_Modifier");
                PlayerController.instance.partyStats[i].evasionModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_evasion_Modifier");
                PlayerController.instance.partyStats[i].initiativeModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_initiative_Modifier");
                PlayerController.instance.partyStats[i].accuracyModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_accuracy_Modifier");
                PlayerController.instance.partyStats[i].magicPowerModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mgcPower_Modifier");
                PlayerController.instance.partyStats[i].magicDefenceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_mgcDefence_Modifier");
                // Elemental Resistances Modifiers
                PlayerController.instance.partyStats[i].fireResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_fireResistance_Modifier");
                PlayerController.instance.partyStats[i].iceResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_iceResistance_Modifier");
                PlayerController.instance.partyStats[i].electricResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_electricResistance_Modifier");
                PlayerController.instance.partyStats[i].plasmaResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_plasmaResistance_Modifier");
                PlayerController.instance.partyStats[i].psychicResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_psychicResistance_Modifier");
                PlayerController.instance.partyStats[i].toxicResistanceModifier = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_toxicResistance_Modifier");
                
                if(PlayerController.instance.partyStats[i].playerLevel>1)
                {
                    for(int j = 0; j<PlayerController.instance.partyStats[i].CypherRewards.Length;j++)
                    {
                        if(j+2<PlayerController.instance.partyStats[i].playerLevel)
                        {
                            PlayerController.instance.partyStats[i].cypherList.Add(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Saved_"+PlayerController.instance.partyStats[i].charName+"_Cypher_Reward_"+j));
                        }
                    }
                }
            }
            else
            {
                PlayerController.instance.partyStats[i].gameObject.SetActive(false);
            }
        }
        #endregion
    
        #region CurrentLevelInfo
        if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_The_Boss_Dead") == "yes") GameManager.instance.bossIsDead = true;
        else GameManager.instance.bossIsDead = false;
        for(int i = 0; i < GameManager.instance.itemsHeld.Length;i++)
        {
            GameManager.instance.itemsHeld[i] = PlayerPrefs.GetString("Saved_Slot_"+slot+"_Item_"+i);
        }
        for(int i = 0; i < GameManager.instance.numberOfItems.Length;i++)
        {
            GameManager.instance.numberOfItems[i] = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Item_"+i+"_Quantity");
        }
        GameManager.instance.credits = PlayerPrefs.GetInt("Saved_Slot_"+slot+"_Credits");
        for(int i = 0; i < GameManager.instance.levelCrates.Count;i++)
        {
            if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_Crate_"+i+"_Active") == "yes")
            {
                GameManager.instance.levelCrates[i].isActive = true;
                GameManager.instance.levelCrates[i].gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.levelCrates[i].isActive = false;
                GameManager.instance.levelCrates[i].DeactivateCrate();
            } 
        }
        for(int i = 0; i< GameManager.instance.restorePoints.Count ;i++)
        {
            if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Restore_Point_"+i+"_Activated") == "yes")
            {
                GameManager.instance.restorePoints[i].ActivatePoint();
            }
        }
        for(int i = 0; i < GameManager.instance.enemiesOnScreen.Length;i++)
        {
            if(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_Enemy_"+i+"_Active") == "yes")
            {
                Debug.Log(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Is_Enemy_"+i+"_Active"));
                Debug.Log(i);
                GameManager.instance.enemiesOnScreen[i].gameObject.SetActive(true);
            } 
            else
            {
                GameManager.instance.enemiesOnScreen[i].gameObject.SetActive(false);
            } 
        }
        #endregion
        print("Game Loaded");
    }

    public void StartLoadCo()
    {
        StartCoroutine(LoadCo());
    }
    
    public IEnumerator LoadCo()
    {
        GameManager.instance.eventManager.DeactivateEvent();
        PlayerController.instance.canMove = false;
        GameManager.instance.eventManager.eventPanel.HideAllPanels();
        UIFade.instance.FadeToBlack();
        GameManager.instance.levelManager.SetNewLevelValues();
        yield return new WaitForSeconds(1f);
        LoadGame(activeSlot);
        PlayerController.instance.canMove = false;
        UIFade.instance.FadeFromBlack();
        yield return new WaitForSeconds(1f);
        PlayerController.instance.canMove = true;
    }

    public void StartLoadInMenuCo(int slot)
    {
        StartCoroutine(LoadInMenuCo(slot));
    }
    
    public IEnumerator LoadInMenuCo(int slot)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(PlayerPrefs.GetString("Saved_Slot_"+slot+"_Current_Scene"));
        activeSlot = slot;
        yield return new WaitForSeconds(1f);
        LoadSaveController.instance.StartLoadCo();        
    }
}

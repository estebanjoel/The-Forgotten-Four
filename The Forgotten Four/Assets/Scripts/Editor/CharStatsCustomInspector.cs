using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using GameEnums;
using System;

[CustomEditor(typeof(CharStats))]
public class CharStatsCustomInspector : Editor
{
   CharStats charStats;
   int topHP = 9999;
   int topMP = 999;
   int expToNextLevelIndex;
   string[] levelArray;
   int topMainStat = 99;
   int initialStrength, initialDexterity, initialVitality, initialDefence, initialElement, initialSpirit;
   int topSecondaryStat = 200;
   int topElementalResistance = 100;
   int equippedWpnPower, equippedArmrPower;
   string[] WpnArray;
   string[] ArmrArray;
   bool hasFirstLevelStatsSetted;
   bool hasEquippedItemsSetted;

   void OnEnable()
   {
       charStats = target as CharStats;    
       levelArray = new string[0];
       charStats.expToNextLevel = new int[0];
       expToNextLevelIndex = 0;
       levelArray = new string[0];
   }

   public override void OnInspectorGUI()
   {
       EditorGUILayout.LabelField(charStats.charName+" LV " + charStats.playerLevel, EditorStyles.boldLabel);
       EditorGUILayout.HelpBox("You must have a PNG file with the name 'UI_<character name>_Head' on 'Editor Default Resources/CharStatsCustomInspector/' Folder in order to show the texture on the Inspector.", MessageType.Info);
       
       EditorGUILayout.Space();

       charStats.SetCharStats((Stats) EditorGUILayout.ObjectField("Stats Scriptable Object:", charStats.GetCharStats(), typeof(Stats)));
       
      if(charStats.GetCharStats() == null)
      {
         EditorGUILayout.HelpBox("You need a 'Stats' Scriptable Object in order to show the rest of this inspector.", MessageType.Warning);
      }

      else
      {
         //Sección de HP y MP
         GUILayout.Label("HP & MP", EditorStyles.boldLabel);
         Texture charTex = (Texture) EditorGUIUtility.Load("/CharStatsCustomInspector/UI_"+charStats.charName+"_Head.png");
         EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
         GUI.DrawTexture(GUILayoutUtility.GetRect(75, 75), charTex, ScaleMode.ScaleToFit);
         EditorGUILayout.BeginVertical();
         EditorGUILayout.BeginVertical(hpBoxStyle);
         charStats.GetCharStats().SetMaxHP(EditorGUILayout.IntField("Max HP:", charStats.GetCharStats().GetMaxHP()));
         if(charStats.GetCharStats().GetMaxHP() > topHP) charStats.GetCharStats().SetMaxHP(topHP);
         if(charStats.GetCharStats().GetMaxHP() < 0) charStats.GetCharStats().SetMaxHP(0);
         charStats.GetCharStats().SetCurrentHP(EditorGUILayout.IntSlider("Current HP: ", charStats.GetCharStats().GetCurrentHP(), 0, charStats.GetCharStats().GetMaxHP()));
         EditorGUILayout.EndVertical();

         EditorGUILayout.BeginVertical(mpBoxStyle);
         charStats.GetCharStats().SetMaxMP(EditorGUILayout.IntField("Max MP:", charStats.GetCharStats().GetMaxMP()));
         if(charStats.GetCharStats().GetMaxMP() > topMP) charStats.GetCharStats().SetMaxMP(topMP);
         if(charStats.GetCharStats().GetMaxMP() < 0) charStats.GetCharStats().SetMaxMP(0);
         charStats.GetCharStats().SetCurrentMP(EditorGUILayout.IntSlider("Current MP: ", charStats.GetCharStats().GetCurrentMP(), 0, charStats.GetCharStats().GetMaxMP()));
         EditorGUILayout.EndVertical();
         
         EditorGUILayout.EndVertical();
         EditorGUILayout.EndHorizontal();
         
         EditorGUILayout.Space();

         //Sección de XP y XP para subir de nivel
         GUILayout.Label("XP", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         charStats.baseEXP = EditorGUILayout.IntField("Base EXP:", charStats.baseEXP);
         if(charStats.baseEXP < 0) charStats.baseEXP = 0;
         charStats.currentEXP = EditorGUILayout.IntSlider("Current EXP: ", charStats.currentEXP, 0, charStats.baseEXP);
         EditorGUILayout.Space();
         if(GUILayout.Button("Set EXP Rate"))
         {
            charStats.SetLevelUPRate();
         }
         if(GUILayout.Button("Reset EXP Rate"))
         {
            charStats.expToNextLevel = new int[0];
            expToNextLevelIndex = 0;
            levelArray = new string[0];
         }

         if(charStats.expToNextLevel.Length > 0)
         {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(normalBoxStyle);
            EditorGUILayout.LabelField("EXP To Next Level", EditorStyles.boldLabel);
            if(levelArray.Length <= 0)
            {
                  levelArray = new string[charStats.expToNextLevel.Length];
                  for(int i = charStats.playerLevel; i < levelArray.Length; i++)
                  {
                     int level = i+1;
                     levelArray[i] = level.ToString();
                  }
            }
            expToNextLevelIndex = EditorGUILayout.Popup("EXP To Level:", expToNextLevelIndex ,levelArray);
            if (expToNextLevelIndex > 0)EditorGUILayout.LabelField("EXP needed to reach level " + levelArray[expToNextLevelIndex]+" : "+charStats.expToNextLevel[expToNextLevelIndex]);
            EditorGUILayout.EndVertical();
         }

         EditorGUILayout.EndVertical();
         
         EditorGUILayout.Space();

         //Stats Principales
         GUILayout.Label("Main Stats", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(statBoxStyle);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         EditorGUILayout.HelpBox("The Main Stats can only scale to "+topMainStat, MessageType.Info);
         EditorGUILayout.EndVertical();
         charStats.GetCharStats().SetStrength(EditorGUILayout.IntSlider("Strength:", charStats.GetCharStats().GetStrength(), 0, topMainStat));
         charStats.GetCharStats().SetDexterity(EditorGUILayout.IntSlider("Dexterity:", charStats.GetCharStats().GetDexterity(), 0, topMainStat));
         charStats.GetCharStats().SetVitality(EditorGUILayout.IntSlider("Vitality:", charStats.GetCharStats().GetVitality(), 0, topMainStat));
         charStats.GetCharStats().SetDefence(EditorGUILayout.IntSlider("Defence:", charStats.GetCharStats().GetDefence(), 0, topMainStat));
         charStats.GetCharStats().SetElement(EditorGUILayout.IntSlider("Element:", charStats.GetCharStats().GetElement(), 0, topMainStat));
         charStats.GetCharStats().SetSpirit(EditorGUILayout.IntSlider("Spirit:", charStats.GetCharStats().GetSpirit(), 0, topMainStat));
         if(charStats.playerLevel == 1) SetInitialStatValues();
         EditorGUILayout.EndVertical();

         EditorGUILayout.Space();

         //Stats Secundarios
         GUILayout.Label("Secondary Stats", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(statBoxStyle);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         EditorGUILayout.HelpBox("The Secondary Stats can only scale to "+topSecondaryStat, MessageType.Info);
         EditorGUILayout.EndVertical();
         charStats.GetCharStats().SetWeaponPower(EditorGUILayout.IntSlider("Weapon Power:", charStats.GetCharStats().GetWeaponPower() , 0, topSecondaryStat));
         charStats.GetCharStats().SetArmourPower(EditorGUILayout.IntSlider("Armour Power:", charStats.GetCharStats().GetArmourPower() , 0, topSecondaryStat));
         charStats.GetCharStats().SetEvasion(EditorGUILayout.IntSlider("Evasion:", charStats.GetCharStats().GetEvasion() , 0, topSecondaryStat));
         charStats.GetCharStats().SetInitiative(EditorGUILayout.IntSlider("Initiative:", charStats.GetCharStats().GetInitiative() , 0, topSecondaryStat));
         charStats.GetCharStats().SetAccuracy(EditorGUILayout.IntSlider("Accuracy:", charStats.GetCharStats().GetAccuracy() , 0, topSecondaryStat));
         charStats.GetCharStats().SetMagicPower(EditorGUILayout.IntSlider("Magic Power:", charStats.GetCharStats().GetMagicPower() , 0, topSecondaryStat));
         charStats.GetCharStats().SetMagicDefence(EditorGUILayout.IntSlider("Magic Defence:", charStats.GetCharStats().GetMagicDefence() , 0, topSecondaryStat));
         EditorGUILayout.EndVertical();
         
         EditorGUILayout.Space();

         //Resistencias Elementales
         GUILayout.Label("Elemental Resistances", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(statBoxStyle);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         EditorGUILayout.HelpBox("The Elemental Resistances can only scale to "+topElementalResistance, MessageType.Info);
         EditorGUILayout.EndVertical();
         charStats.GetCharStats().SetFireResistance(EditorGUILayout.IntSlider("Fire Resistance", charStats.GetCharStats().GetFireResistance(), 0, topElementalResistance));
         charStats.GetCharStats().SetIceResistance(EditorGUILayout.IntSlider("Ice Resistance", charStats.GetCharStats().GetIceResistance(), 0, topElementalResistance));
         charStats.GetCharStats().SetElectricResistance(EditorGUILayout.IntSlider("Electric Resistance", charStats.GetCharStats().GetElectricResistance(), 0, topElementalResistance));
         charStats.GetCharStats().SetPlasmaResistance(EditorGUILayout.IntSlider("Plasma Resistance", charStats.GetCharStats().GetPlasmaResistance(), 0, topElementalResistance));
         charStats.GetCharStats().SetPsychicResistance(EditorGUILayout.IntSlider("Psychic Resistance", charStats.GetCharStats().GetPsychicResistance(), 0, topElementalResistance));
         charStats.GetCharStats().SetToxicResistance(EditorGUILayout.IntSlider("Toxic Resistance", charStats.GetCharStats().GetToxicResistance(), 0, topElementalResistance));
         EditorGUILayout.EndVertical();

         EditorGUILayout.Space();

         //Equipo
         GUILayout.Label("Equip", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(equipBoxStyle);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         EditorGUILayout.HelpBox("In order to use correctly this part, you need at least an Item type prefab in the Paths 'Assets/Prefabs/Items/Weapons/' and 'Assets/prefabs/Items/Armor/' respectively.", MessageType.Info);
         EditorGUILayout.EndVertical();
         WpnArray = GetEquipItem(true, false);
         charStats.equippedWpnIndex = EditorGUILayout.Popup("Equipped Weapon:", charStats.equippedWpnIndex, WpnArray);
         charStats.equippedWpn = WpnArray[charStats.equippedWpnIndex];
         ArmrArray = GetEquipItem(false,true);
         charStats.equippedArmrIndex = EditorGUILayout.Popup("Equipped Armour:", charStats.equippedArmrIndex, ArmrArray);
         charStats.equippedArmr = ArmrArray[charStats.equippedArmrIndex];
         if(hasEquippedItemsSetted)
         {
            EditorGUILayout.BeginVertical(normalBoxStyle);
            EditorGUILayout.HelpBox("If you change the equipped item, remember to reset the equipped items values and set them again, in order to see the changes in the Weapon Power & Armour Power Stats.", MessageType.Info);
            EditorGUILayout.EndVertical();
         }
         EditorGUILayout.EndVertical();

         EditorGUILayout.Space();

         //Cyphers
         GUILayout.Label("Cyphers", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(cypherBoxStyle);
         GUILayout.Label("Current Cyphers List", EditorStyles.boldLabel);
         string[] cyphers = CurrentCyphers();
         
         EditorGUILayout.BeginVertical(normalBoxStyle);
         charStats.currentCypherIndex = EditorGUILayout.Popup("Choose a Cypher form your list:", charStats.currentCypherIndex, cyphers);
         EditorGUILayout.HelpBox("In a future, when you select a cypher, in this box it will give all the selected Cypher details. But first, the main project need some changes in order to show it.", MessageType.Info);
         EditorGUILayout.EndVertical();
         
         GUILayout.Label("Cypher Rewards per Level:", EditorStyles.boldLabel);
         EditorGUILayout.BeginVertical(normalBoxStyle);
         for(int i = 0; i < charStats.CypherRewards.Length; i++)
         {
            EditorGUILayout.LabelField("° LV " + (i+2).ToString() +": " + charStats.CypherRewards[i]);
         }
         EditorGUILayout.EndVertical();
         EditorGUILayout.EndVertical();

         EditorGUILayout.Space();

         //Sprite para UI
         charStats.charImage = (Sprite)EditorGUILayout.ObjectField("Character Sprite: ", charStats.charImage, typeof(Sprite));

         EditorGUILayout.Space();

         //Botones
         EditorGUILayout.BeginVertical(normalBoxStyle);
         EditorGUILayout.HelpBox("You can use the following buttons to affect the character stats and level", MessageType.Info);
         EditorGUILayout.EndVertical();
         EditorGUILayout.Space();
         
         EditorGUILayout.BeginVertical(normalBoxStyle);
         //Botones para setear y resetear los stats en nivel 1
         if(charStats.playerLevel == 1)
         {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Set Stats on First Level"))
            {
               if(!hasFirstLevelStatsSetted)
               {
                  charStats.AssignStatsOnFirstLevel();
                  hasFirstLevelStatsSetted = true;
               }
            }
            if(GUILayout.Button("Restart Stats on First Level"))
            {
               RestartStatsOnFirstLevel();
               hasFirstLevelStatsSetted = false;
            }
            EditorGUILayout.EndHorizontal();
         }

         if(hasFirstLevelStatsSetted) EditorGUILayout.HelpBox("Stats setted. You must press the 'Restart Stats on First Level' button in order to retry.", MessageType.Info);

         EditorGUILayout.BeginHorizontal();
         //Botones para setear y resetear el valor de los items equipados. Así incrementará o reducirá el Weapon Power y Armor Power del personaje
         if(GUILayout.Button("Set Equipped Items"))
         {
            if(!hasEquippedItemsSetted)
            {
               setWeaponAndArmorValues();
               hasEquippedItemsSetted = true;
            }
         }
         if(GUILayout.Button("Reset Equipped Items"))
         {
            if(hasEquippedItemsSetted)
            {
               int wpnPwr = charStats.GetCharStats().GetWeaponPower();
               wpnPwr -= equippedWpnPower;
               charStats.GetCharStats().SetWeaponPower(wpnPwr);
               int armrPwr = charStats.GetCharStats().GetArmourPower();
               armrPwr -= equippedArmrPower;
               charStats.GetCharStats().SetArmourPower(armrPwr);
               hasEquippedItemsSetted = false;
            }
         }
         
         EditorGUILayout.EndHorizontal();

         if(hasEquippedItemsSetted) EditorGUILayout.HelpBox("Equipped Items setted. You must press the 'Restart Equipped Items' button in order to retry.", MessageType.Info);

         EditorGUILayout.Space();

         EditorGUILayout.BeginHorizontal();
         if(GUILayout.Button("Level UP"))
         {
            if(charStats.playerLevel < charStats.maxLevel)
            {
               charStats.LevelUP(); 
               expToNextLevelIndex = charStats.playerLevel;
            } 
         }
         if(GUILayout.Button("Return to Level 1"))
         {
            if(charStats.playerLevel != 1)
            {
               charStats.GetCharStats().SetStrength(initialStrength);
               charStats.GetCharStats().SetDexterity(initialDexterity);
               charStats.GetCharStats().SetVitality(initialVitality);
               charStats.GetCharStats().SetDefence(initialDefence);
               charStats.GetCharStats().SetElement(initialElement);
               charStats.GetCharStats().SetSpirit(initialSpirit);
               RestartStatsOnFirstLevel();
               charStats.playerLevel = 1;
               hasFirstLevelStatsSetted = false;
               charStats.cypherList.RemoveRange(1, charStats.cypherList.Count-1);
            }
         }
         EditorGUILayout.EndHorizontal();
         if(charStats.playerLevel > 1) EditorGUILayout.HelpBox("You have just leveled up! Check the stats in order to see the character progressing for every level augmentation.",MessageType.Info);
         else EditorGUILayout.HelpBox("You are currently in level 1. You can set the main stats manually, which will be saved as the initial values.\nDon't forget to Set the EXP Rate in order to know how much EXP will need your character in order to advance to the next level.",MessageType.Info);
         EditorGUILayout.EndVertical();
      }
      

      if(GUI.changed)
      {
        EditorUtility.SetDirty(charStats);
        EditorSceneManager.MarkSceneDirty(charStats.gameObject.scene); 
      }

   }

    private void SetInitialStatValues()
    {
        initialStrength = charStats.GetCharStats().GetStrength();
        initialDexterity = charStats.GetCharStats().GetDexterity();
        initialVitality = charStats.GetCharStats().GetVitality();
        initialDefence = charStats.GetCharStats().GetDefence();
        initialElement = charStats.GetCharStats().GetElement();
        initialSpirit = charStats.GetCharStats().GetSpirit();
    }

    #region boxStyles
    private static GUIStyle hpBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = Color.green;
         return boxStyle;
      }
   }
   private static GUIStyle mpBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = Color.cyan;
         return boxStyle;
      }
   }

   private static GUIStyle normalBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = new Color(.95f,.95f,.95f,1f);
         return boxStyle;
      }
   }

   private static GUIStyle statBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = Color.blue;
         return boxStyle;
      }
   }

   private static GUIStyle equipBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = Color.cyan;
         return boxStyle;
      }
   }

   private static GUIStyle cypherBoxStyle
   {
      get
      {
         var boxStyle = new GUIStyle(EditorStyles.helpBox);
         GUI.backgroundColor = Color.magenta;
         return boxStyle;
      }
   }
   #endregion

   private string[] GetEquipItem(bool isWeapon, bool isArmour)
   {
      string[] items = new string[0];
      string itemPath = "Assets/Prefabs/Items/";

      if(isWeapon) itemPath+="Weapons";
      else if(isArmour) itemPath+="Armor";
      else return items;

      string[] itemsGuids = AssetDatabase.FindAssets("", new[] {itemPath});
      items = new string[itemsGuids.Length];

      for(int i = 0; i < itemsGuids.Length; i++)
      {
         EquipItem newItem = (EquipItem)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(itemsGuids[i]), typeof(Item));
         if(CheckIfItemIsCompatibleWithChar(newItem))
         {
            items[i] = newItem.name;
         } 
      }

      int filledSlots = 0;

      for(int i = 0; i < items.Length; i++)
      {
         if(items[i] != null) filledSlots++;
      }

      string[] compatibleItems = new string[filledSlots];

      for(int i = 0; i < items.Length; i++)
      {
         if(items[i] != null)
         {
            for(int j = 0; j < compatibleItems.Length; j++)
            {
               if(compatibleItems[j] == null)
               {
                  compatibleItems[j] = items[i];
                  break;
               }
            }
         }
      }

      return compatibleItems;
   }

   private bool CheckIfItemIsCompatibleWithChar(EquipItem item)
   {
      switch(item.itemCharTarget)
      {
         case equipItemCharTarget.None:
            return false;
         case equipItemCharTarget.Anyone:
            return true;
         case equipItemCharTarget.Heavy:
            if(charStats.charClass == characterClass.Soldier || charStats.charClass == characterClass.Tank) return true;
            else return false;
         case equipItemCharTarget.Soft:
            if(charStats.charClass == characterClass.Mage || charStats.charClass == characterClass.Healer) return true;
            else return false;
         case equipItemCharTarget.Sanji:
            if(charStats.charClass == characterClass.Soldier) return true;
            else return false;
         case equipItemCharTarget.Kenya:
            if(charStats.charClass == characterClass.Mage) return true;
            else return false;
         case equipItemCharTarget.Yakov:
            if(charStats.charClass == characterClass.Healer) return true;
            else return false;
         case equipItemCharTarget.RX10:
            if(charStats.charClass == characterClass.Tank) return true;
            else return false;
      }
      return false;
   }

   private void RestartStatsOnFirstLevel()
   {
      charStats.GetCharStats().SetMaxHP(0);
      charStats.GetCharStats().SetCurrentHP(0);
      charStats.GetCharStats().SetMaxMP(0);
      charStats.GetCharStats().SetCurrentMP(0);
      charStats.GetCharStats().SetWeaponPower(0);
      charStats.GetCharStats().SetArmourPower(0);
      charStats.GetCharStats().SetEvasion(0);
      charStats.GetCharStats().SetInitiative(0);
      charStats.GetCharStats().SetAccuracy(0);
      charStats.GetCharStats().SetMagicPower(0);
      charStats.GetCharStats().SetMagicDefence(0);
      charStats.GetCharStats().SetFireResistance(0);
      charStats.GetCharStats().SetIceResistance(0);
      charStats.GetCharStats().SetElectricResistance(0);
      charStats.GetCharStats().SetPlasmaResistance(0);
      charStats.GetCharStats().SetPsychicResistance(0);
      charStats.GetCharStats().SetToxicResistance(0);
   }

   private void setWeaponAndArmorValues()
   {
      EquipItem equippedItem = (EquipItem) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Items/Armor/" + charStats.equippedArmr + ".prefab", typeof(Item));
      equippedArmrPower = equippedItem.armourStrength;
      equippedItem.armourStrength += charStats.GetCharStats().GetArmourPower();
      charStats.GetCharStats().SetArmourPower(equippedItem.armourStrength);

      equippedItem = (EquipItem) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Items/Weapons/" + charStats.equippedWpn + ".prefab", typeof(Item));
      equippedWpnPower = equippedItem.weaponStrength;
      equippedItem.weaponStrength += charStats.GetCharStats().GetWeaponPower();
      charStats.GetCharStats().SetWeaponPower(equippedItem.weaponStrength);
   }

   private string[] CurrentCyphers()
   {
      string[] currentCyphers = new string[charStats.cypherList.Count];
      for(int i = 0; i < currentCyphers.Length; i++)
      {
         currentCyphers[i] = charStats.cypherList[i];
      }
      return currentCyphers;
   }
}

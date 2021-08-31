using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMenu : MonoBehaviour
{
    public GameObject[] menuArray;
    public GameObject questionPanel;
    public bool toMainMenu, quit;
    public AudioSource playerMenuSource;
    public AudioClip[] playerMenuClips;
    public PlayerPanel[] playerPanels;
    public GameObject currentPlayerPanel;
    public ItemButton[] itemButtons;
    public Image ItemActionsPanel, UseTargetPanel, ItemMessagePanel;
    public EquipPanel[] equipCharPanels;
    public GameObject currentEquipCharPanel;
    public Button[] cypherCharButtons;
    public CypherPanel[] cypherCharPanels;
    public CharStats[] myChars;

    void Start()
    {
        playerMenuSource = AudioManager.instance.PlatformSFX[3];
    }

    public void showMenu(int index)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            menuArray[i].SetActive(false);
        }

        menuArray[index].SetActive(true);
        switch (index) {
            case 0:
                ShowCharStats(0);
                break;
            case 1:
                ShowItems();
                break;
            case 2:
                ShowEquipCharPanel(0);
                break;
            case 3:
                ShowCypherCharButtons();
                ShowCypherCharPanel(0);
                break;
            default:
                break;
        }
        PlaySelectAudio();
    }

    public int PlayerPanelIndex(int i)
    {
        if(i<0)
        {
            i = playerPanels.Length-1;
        }
        else if(i>playerPanels.Length-1)
        {
            i = 0;
        }

        return i;
    }
    public void ShowCharStats(int i)
    {
        i = PlayerPanelIndex(i);
        if(currentPlayerPanel!=null)
        {
            currentPlayerPanel.SetActive(false);
        }
        playerPanels[i].gameObject.SetActive(true);
        currentPlayerPanel = playerPanels[i].gameObject;
        CharStats(i);
    }

    public void CharStats(int i)
    {
        CharStats myCharacter = PlayerController.instance.partyStats[i];
        playerPanels[i].charHead.sprite = myCharacter.charImage;
        playerPanels[i].nameText.text = myCharacter.charName;
        playerPanels[i].levelText.text = "LV: "+myCharacter.playerLevel;
        if((float)myCharacter.currentEXP/myCharacter.expToNextLevel[myCharacter.playerLevel]<0.1f)
        {
            playerPanels[i].xpBarFiller.fillAmount = 0.1f;
        }
        else
        {
            playerPanels[i].xpBarFiller.fillAmount = (float)myCharacter.currentEXP/myCharacter.expToNextLevel[myCharacter.playerLevel];
        }
        playerPanels[i].hpValueText.text = "HP: "+myCharacter.GetCharStats().GetCurrentHP()+" / "+myCharacter.GetCharStats().GetMaxHP();
        playerPanels[i].hpBarFiller.fillAmount = (float)myCharacter.GetCharStats().GetCurrentHP()/myCharacter.GetCharStats().GetMaxHP();
        playerPanels[i].mpValueText.text = "MP: "+myCharacter.GetCharStats().GetCurrentMP()+" / "+myCharacter.GetCharStats().GetMaxMP();
        playerPanels[i].mpBarFiller.fillAmount = (float)myCharacter.GetCharStats().GetCurrentMP()/myCharacter.GetCharStats().GetMaxMP();
        playerPanels[i].strText.text = "Strength: "+myCharacter.GetCharStats().GetStrength();
        playerPanels[i].dexText.text = "Dexterity: "+myCharacter.GetCharStats().GetDexterity();
        playerPanels[i].vitText.text = "Vitality: "+myCharacter.GetCharStats().GetVitality();
        playerPanels[i].defText.text = "Defence: "+myCharacter.GetCharStats().GetDefence();
        playerPanels[i].eleText.text = "Element: "+myCharacter.GetCharStats().GetElement();
        playerPanels[i].spiText.text = "Spirit: "+myCharacter.GetCharStats().GetSpirit();
        playerPanels[i].wpnPwrText.text = "Weapon Power: "+myCharacter.GetCharStats().GetWeaponPower();
        playerPanels[i].armrPwrText.text = "Armor Power: "+myCharacter.GetCharStats().GetArmourPower();
        playerPanels[i].mgcPwrText.text = "Magic Power: "+myCharacter.GetCharStats().GetMagicPower();
        playerPanels[i].mgcDefText.text = "Magic Defence: "+myCharacter.GetCharStats().GetMagicDefence();
        playerPanels[i].accText.text = "Accuracy: "+myCharacter.GetCharStats().GetAccuracy();
        playerPanels[i].evaText.text = "Evasion: "+myCharacter.GetCharStats().GetEvasion();
        playerPanels[i].iniText.text = "Initiative: "+myCharacter.GetCharStats().GetInitiative();
        playerPanels[i].fireResText.text = "Fire RES: %"+myCharacter.GetCharStats().GetFireResistance();
        playerPanels[i].iceResText.text = "Ice RES: %"+myCharacter.GetCharStats().GetIceResistance();
        playerPanels[i].elecResText.text = "Electric RES: %"+myCharacter.GetCharStats().GetElectricResistance();
        playerPanels[i].plasResText.text = "Plasma RES: %"+myCharacter.GetCharStats().GetPlasmaResistance();
        playerPanels[i].psyResText.text = "Psychic RES: %"+myCharacter.GetCharStats().GetPsychicResistance();
        playerPanels[i].toxResText.text = "Toxic RES: %"+myCharacter.GetCharStats().GetToxicResistance();
        int previousI = PlayerPanelIndex(i-1);
        while(!PlayerController.instance.partyStats[previousI].gameObject.activeInHierarchy)
        {
            previousI = PlayerPanelIndex(previousI-1);
        }
        playerPanels[i].previousCharText.text = PlayerController.instance.partyStats[previousI].charName;
        int nextI = PlayerPanelIndex(i+1);
        while(!PlayerController.instance.partyStats[nextI].gameObject.activeInHierarchy)
        {
            nextI = PlayerPanelIndex(nextI+1);
        }
        playerPanels[i].nextCharText.text = PlayerController.instance.partyStats[nextI].charName;
    }

    public void MoveStatPanel(int i)
    {
        for(int j = 0; j<playerPanels.Length;j++)
        {
            if(playerPanels[j].gameObject == currentPlayerPanel)
            {
                int k= PlayerPanelIndex(j+i);
                if(PlayerController.instance.partyStats[k].gameObject.activeInHierarchy)
                {
                    ShowCharStats(k);
                }
                else
                {
                    bool hasSelected = false;
                    while(!hasSelected)
                    {
                        k+=i;
                        k=PlayerPanelIndex(k);
                        if(PlayerController.instance.partyStats[k].gameObject.activeInHierarchy)
                        {
                            ShowCharStats(k);
                            hasSelected = true;
                        }
                    }
                }
                break;
            }
        }
    }

    public void ShowItems()
    {
        ItemActionsPanel.gameObject.SetActive(false);
        UseTargetPanel.gameObject.SetActive(false);
        ItemMessagePanel.gameObject.SetActive(false);
        UpdateItemQuantity();
    }

    public void UpdateItemQuantity()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;
            if (i < GameManager.instance.itemsHeld.Length)
            {
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    if(GameManager.instance.numberOfItems[i] > 0)
                    {
                        string itemName = GameManager.instance.itemsHeld[i];
                        itemButtons[i].gameObject.SetActive(true);
                        itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemName).itemSprite;
                        itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                    }
                    else
                    {
                        itemButtons[i].gameObject.SetActive(false);
                    }
                    
                }

                else
                {
                    itemButtons[i].gameObject.SetActive(false);
                }
            }

            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void MoveEquipPanel(int i)
    {
        for(int j = 0; j<equipCharPanels.Length;j++)
        {
            if(equipCharPanels[j].gameObject == currentEquipCharPanel)
            {
                int k= PlayerPanelIndex(j+i);
                if(PlayerController.instance.partyStats[k].gameObject.activeInHierarchy)
                {
                    ShowEquipCharPanel(k);
                }
                else
                {
                    bool hasSelected = false;
                    while(!hasSelected)
                    {
                        k+=i;
                        k=PlayerPanelIndex(k);
                        if(PlayerController.instance.partyStats[k].gameObject.activeInHierarchy)
                        {
                            ShowEquipCharPanel(k);
                            hasSelected = true;
                        }
                    }
                }
                break;
            }
        }
    }

    public void ShowEquipCharPanel(int i)
    {
        i = PlayerPanelIndex(i);
        if(currentEquipCharPanel!=null)
        {
            currentEquipCharPanel.SetActive(false);
        }
        equipCharPanels[i].gameObject.SetActive(true);
        currentEquipCharPanel = equipCharPanels[i].gameObject;
        EquipCharPanel(i);
    
    }
    public void EquipCharPanel(int character)
    {
        for (int i = 0; i < myChars.Length; i++)
        {
            if (i == character)
            {
                equipCharPanels[i].gameObject.SetActive(true);
                ShowEquipCharStats(character);
                ShowEquipButtons(character);
            }
            else
            {
                equipCharPanels[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowEquipCharStats(int character)
    {
        equipCharPanels[character].CharIndex = character;
        equipCharPanels[character].charHead.sprite = myChars[character].charImage;
        equipCharPanels[character].levelText.text = "LV: "+myChars[character].playerLevel;
        if((float)myChars[character].currentEXP/myChars[character].expToNextLevel[myChars[character].playerLevel] < 0.1f) equipCharPanels[character].xpBarFiller.fillAmount = 0.1f;
        else equipCharPanels[character].xpBarFiller.fillAmount = (float)myChars[character].currentEXP/myChars[character].expToNextLevel[myChars[character].playerLevel];
        equipCharPanels[character].hpText.text = myChars[character].GetCharStats().GetCurrentHP()+" / "+ myChars[character].GetCharStats().GetMaxHP();
        equipCharPanels[character].mpText.text = myChars[character].GetCharStats().GetCurrentMP()+" / "+ myChars[character].GetCharStats().GetMaxMP();
        equipCharPanels[character].strengthText.text = "STR: "+myChars[character].GetCharStats().GetStrength().ToString();
        equipCharPanels[character].dexterityText.text = "DEX: "+myChars[character].GetCharStats().GetDexterity().ToString();
        equipCharPanels[character].vitalityText.text = "VIT: "+myChars[character].GetCharStats().GetVitality().ToString();
        equipCharPanels[character].defenceText.text = "DEF: "+myChars[character].GetCharStats().GetDefence().ToString();
        equipCharPanels[character].elementText.text = "ELE: "+myChars[character].GetCharStats().GetElement().ToString();
        equipCharPanels[character].spiritText.text = "SPI: "+myChars[character].GetCharStats().GetSpirit().ToString();
        equipCharPanels[character].weaponPowerText.text = "W.P.: "+myChars[character].GetCharStats().GetWeaponPower().ToString();
        equipCharPanels[character].armorPowerText.text = "A.P.: "+myChars[character].GetCharStats().GetArmourPower().ToString();
        equipCharPanels[character].magicPowerText.text = "M.P.: "+myChars[character].GetCharStats().GetMagicPower().ToString();
        equipCharPanels[character].magicDefenceText.text = "M.D.: "+myChars[character].GetCharStats().GetMagicDefence().ToString();
        equipCharPanels[character].accuracyText.text = "ACC: "+myChars[character].GetCharStats().GetAccuracy().ToString();
        equipCharPanels[character].evasionText.text = "EVA: "+myChars[character].GetCharStats().GetEvasion().ToString();
        equipCharPanels[character].initiativeText.text = "INI: "+myChars[character].GetCharStats().GetInitiative().ToString();
        equipCharPanels[character].fireResistanceText.text = "FI.R.: "+myChars[character].GetCharStats().GetFireResistance()+"%";
        equipCharPanels[character].iceResistanceText.text = "IC.R.: "+myChars[character].GetCharStats().GetIceResistance()+"%";
        equipCharPanels[character].electricResistanceText.text = "EL.R.: "+myChars[character].GetCharStats().GetElectricResistance()+"%";
        equipCharPanels[character].plasmaResistanceText.text = "PL.R.: "+myChars[character].GetCharStats().GetPlasmaResistance()+"%";
        equipCharPanels[character].psychicResistanceText.text = "PS.R.: "+myChars[character].GetCharStats().GetPsychicResistance()+"%";
        equipCharPanels[character].toxicResistanceText.text = "TO.R.: "+myChars[character].GetCharStats().GetToxicResistance()+"%";
        for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
        {
            if(GameManager.instance.itemsHeld[i] == myChars[character].equippedWpn)
            {
                equipCharPanels[character].equippedWeapon.equippedItemText.text = myChars[character].equippedWpn;
                equipCharPanels[character].equippedWeapon.equippedItemReference = GameManager.instance.referenceItems[i];
                equipCharPanels[character].equippedWeapon.equippedItemSprite.sprite =equipCharPanels[character].equippedWeapon.equippedItemReference.itemSprite;
                break;
            }
        }
        for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
        {
            if(GameManager.instance.itemsHeld[i] == myChars[character].equippedArmr)
            {
                equipCharPanels[character].equippedArmor.equippedItemText.text = myChars[character].equippedArmr;
                equipCharPanels[character].equippedArmor.equippedItemReference = GameManager.instance.referenceItems[i];
                equipCharPanels[character].equippedArmor.equippedItemSprite.sprite =equipCharPanels[character].equippedArmor.equippedItemReference.itemSprite;
                break;
            }
        }
        equipCharPanels[character].HideAllStatModifiers();
        int previousI = PlayerPanelIndex(character-1);
        while(!PlayerController.instance.partyStats[previousI].gameObject.activeInHierarchy)
        {
            previousI = PlayerPanelIndex(previousI-1);
        }
        equipCharPanels[character].previousCharName.text = PlayerController.instance.partyStats[previousI].charName;
        int nextI = PlayerPanelIndex(character+1);
        while(!PlayerController.instance.partyStats[nextI].gameObject.activeInHierarchy)
        {
            nextI = PlayerPanelIndex(nextI+1);
        }
        equipCharPanels[character].nextCharName.text = PlayerController.instance.partyStats[nextI].charName;
    }
    
    public void ShowEquipButtons(int character)
    {
        EquipMenuButton[] myButtons = equipCharPanels[character].panelButtons.buttons;
        int buttonsAvailable = 0;
        List<Item> myItems = new List<Item>();
        for(int i = 0; i < GameManager.instance.referenceItems.Length;i++)
        {
            if(GameManager.instance.referenceItems[i].isArmor || GameManager.instance.referenceItems[i].isWeapon)
            {
                if(GameManager.instance.numberOfItems[i]>0)
                {
                    buttonsAvailable++;
                    myItems.Add(GameManager.instance.referenceItems[i]);
                }
                
            } 
        }
        for(int i = 0; i < myButtons.Length; i++)
        {
            if(buttonsAvailable>0)
            {
                myButtons[i].gameObject.SetActive(true);
                myButtons[i].itemReference = myItems[i];
                if(myItems[i].isArmor)
                {
                    myButtons[i].isArmor = true;
                    myButtons[i].isWeapon = false;
                }
                if(myItems[i].isWeapon)
                {
                    myButtons[i].isArmor = false;
                    myButtons[i].isWeapon = true;
                }
                myButtons[i].CheckIfButtonIsAvailable();
                myButtons[i].SetButtonValues();
                buttonsAvailable--;
            }

            else
            {
                myButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public void ShowCypherCharButtons()
    {
        for(int i = 0; i < cypherCharButtons.Length; i++)
        {
            if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                cypherCharButtons[i].gameObject.SetActive(true);
            }

            else
            {
                cypherCharButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public void ShowCypherCharPanel(int character)
    {
        for (int i = 0; i < myChars.Length; i++)
        {
            if (i == character)
            {
                cypherCharPanels[i].gameObject.SetActive(true);
                ShowCypherButtons(character);
            }
            else
            {
                cypherCharPanels[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowCypherButtons(int character)
    {
        bool cypherAvailable;
        BattleMove[] myMoves = BattleManager.instance.movesList;
        for(int i = 0; i < cypherCharPanels[character].cypherButtons.Length;i++)
        {
            cypherAvailable = false;
            if (i < myChars[character].cypherList.Count)
            {
                cypherAvailable = true;
            }
            if (cypherAvailable)
            {
                cypherCharPanels[character].cypherButtons[i].gameObject.SetActive(true);
                for(int j = 0; j < myMoves.Length; j++)
                {
                    if(myChars[character].cypherList[i] == myMoves[j].moveName)
                    {
                        string moveText = myMoves[j].moveName;
                        string moveCost = myMoves[j].moveCost.ToString();
                        string description = myMoves[j].description;
                        cypherCharPanels[character].cypherButtons[i].SetValues(character,moveText, moveCost, description, myMoves[j].eType);
                        cypherCharPanels[character].cypherButtons[i].CheckIfCypherCanUseInMenu();
                    }
                }
            }
            else
            {
                cypherCharPanels[character].cypherButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public void Resume()
    {
        PlaySelectAudio();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenMenuQuestionPanel()
    {
        PlaySelectAudio();
        questionPanel.SetActive(true);
        questionPanel.transform.GetChild(0).gameObject.SetActive(true);
        questionPanel.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        PlayConfirmAudio();
        toMainMenu = true;
    }

    public void OpenQuitQuestionPanel()
    {
        PlaySelectAudio();
        questionPanel.SetActive(true);
        questionPanel.transform.GetChild(0).gameObject.SetActive(false);
        questionPanel.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CloseQuestionPanel()
    {
        PlayCancelAudio();
        questionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayConfirmAudio();
        quit = true;
    }

    public void PlayMoveAudio()
    {
        playerMenuSource = AudioManager.instance.ChangeAudioClip(playerMenuSource, playerMenuClips[0]);
        playerMenuSource.Play();
    }
    public void PlaySelectAudio()
    {
        playerMenuSource = AudioManager.instance.ChangeAudioClip(playerMenuSource, playerMenuClips[1]);
        playerMenuSource.Play();
    }

    public void PlayCancelAudio()
    {
        playerMenuSource = AudioManager.instance.ChangeAudioClip(playerMenuSource, playerMenuClips[2]);
        playerMenuSource.Play();
    }

    public void PlayConfirmAudio()
    {
        playerMenuSource = AudioManager.instance.ChangeAudioClip(playerMenuSource, playerMenuClips[3]);
        playerMenuSource.Play();
    }

    public void PlayUseItemAudio()
    {
        playerMenuSource = AudioManager.instance.ChangeAudioClip(playerMenuSource, playerMenuClips[4]);
        playerMenuSource.Play();
    }
}


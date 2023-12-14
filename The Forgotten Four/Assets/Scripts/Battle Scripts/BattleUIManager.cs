using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class BattleUIManager : MonoBehaviour
{
    public BattleButtons buttons;
    public GameObject currentPanel, previousPanel;
    #region Panels
    public GameObject topPanel;
    public GameObject playerPanel;
    public GameObject playerStatsPanel;
    public GameObject playerButtonsHolder;
    public GameObject targetMenu;
    public GameObject itemMenu;
    public GameObject cypherMenu;
    #endregion
    public Image[] charStats, hpBarFillers, mpBarFillers;
    public Text[] playerName, playerHP, playerMP;
    public bool playerHasSelectedAnOption;
    public GameObject[] statsToModifyPanels;

    void Start()
    {
        currentPanel = playerButtonsHolder;
        previousPanel = currentPanel;
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void CheckActiveBattlePanel(BattleManager bm)
    {
        if (currentPanel == cypherMenu || currentPanel == itemMenu)
        {
            HidePanel(playerStatsPanel);
        }
        else
        {
            ShowPanel(playerStatsPanel);
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPanel != previousPanel)
            {
                bm.audioManager.PlayUICancelAudio();
            }
            currentPanel.SetActive(false);
            previousPanel.SetActive(true);
            if (previousPanel == cypherMenu)
            {
                bm.activeBattlers[bm.currentTurn].chara.currentMP += bm.pointsInCasePlayerGoesBack;
            }
            currentPanel = previousPanel;
            if (currentPanel == cypherMenu || currentPanel == itemMenu)
            {
                previousPanel = playerButtonsHolder;
            }

            if (currentPanel == playerButtonsHolder)
            {
                playerHasSelectedAnOption = false;
            }
        }

        if (playerHasSelectedAnOption)
        {
            HidePanel(playerButtonsHolder);
        }
        else
        {
            ShowPanel(playerButtonsHolder);
        }
    }

    public void ChangeCurrentPanel(GameObject newPanel)
    {
        previousPanel = currentPanel;
        currentPanel = newPanel;
    }

    public void ChangeTopPanelText(string text)
    {
        topPanel.transform.GetChild(0).GetComponent<Text>().text = text;
    }

    public void SetStatToModifyText(GameObject panel, string statText)
    {
        panel.GetComponentInChildren<Text>().text = statText;
    }

    public void UpdateUIStats(BattleManager bm)
    {
        for (int i = 0; i < charStats.Length; i++)
        {
            charStats[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bm.activeBattlers.Count; i++)
        {
            if (bm.activeBattlers[i].chara.isPlayer)
            {
                ShowPanel(playerPanel);
                ChangeCurrentPanel(playerButtonsHolder);
                for (int j = 0; j < playerName.Length; j++)
                {
                    if (playerName[j].text == bm.activeBattlers[i].chara.charName)
                    {
                        Battler playerData = bm.activeBattlers[i];
                        charStats[j].gameObject.SetActive(true);
                        playerName[j].text = playerData.chara.charName;
                        playerHP[j].text = Mathf.Clamp(playerData.chara.currentHp, 0, int.MaxValue) + " / " + playerData.chara.maxHP;
                        playerMP[j].text = Mathf.Clamp(playerData.chara.currentMP, 0, int.MaxValue) + " / " + playerData.chara.maxMP;
                        hpBarFillers[j].fillAmount = (((float)playerData.chara.currentHp * 1) / (float)playerData.chara.maxHP);
                        mpBarFillers[j].fillAmount = (((float)playerData.chara.currentMP * 1) / (float)playerData.chara.maxMP);
                        if(!bm.activeBattlers[i].chara.hasAnAlteratedStat) HidePanel(statsToModifyPanels[j]);
                        else ShowPanel(statsToModifyPanels[j]);
                    }
                }
            }
            else
            {
                HidePanel(playerButtonsHolder);
            }
        }
    }

    public void OpenTargetMenu(string moveName)
    {
        bool isCypher = false;
        bool isItem = false;
        bool targetIsEnemy = false;
        bool allTarget = false;
        bool selfTarget = false;
        bool singleTarget = false;
        int moveIndex = 0;
        ChangeCurrentPanel(targetMenu);
        ShowPanel(targetMenu);
        playerHasSelectedAnOption = true;
        for (int i = 0; i < BattleManager.instance.movesList.Length; i++)
        {
            if (BattleManager.instance.movesList[i].moveName == moveName)
            {
                isCypher = true;
                moveIndex = i;
                break;
            }
        }
        for(int i = 0; i<GameManager.instance.itemsHeld.Length;i++)
        {
            if(GameManager.instance.itemsHeld[i] == moveName)
            {
                isItem = true;
                moveIndex = i;
                break;
            }
        }

        if(isCypher)
        {
            if (BattleManager.instance.movesList[moveIndex].eType != elementType.Heal && BattleManager.instance.movesList[moveIndex].eType != elementType.Support)
            {
                targetIsEnemy = true;
            }
            switch(BattleManager.instance.movesList[moveIndex].mTarget)
            {
                case moveTarget.All:
                    allTarget = true;
                    break;
                case moveTarget.Self:
                    selfTarget = true;
                    break;
                case moveTarget.Single:
                    singleTarget = true;
                    break;
            }

            if(singleTarget)
            {
                if (targetIsEnemy)
                {
                    List<int> enemies = new List<int>();
                    for (int i = 0; i < BattleManager.instance.activeBattlers.Count; i++)
                    {
                        if (!BattleManager.instance.activeBattlers[i].chara.isPlayer)
                        {
                            enemies.Add(i);
                        }
                    }            

                    for (int i = 0; i < buttons.targetButtons.Length; i++)
                    {
                        if (enemies.Count > i && BattleManager.instance.activeBattlers[enemies[i]].chara.currentHp > 0)
                        {
                            buttons.targetButtons[i].gameObject.SetActive(true);
                            buttons.targetButtons[i].moveName = moveName;
                            buttons.targetButtons[i].activeBattlerTarget = enemies[i];
                            buttons.targetButtons[i].targetName.text = BattleManager.instance.activeBattlers[enemies[i]].chara.charName;
                            buttons.targetButtons[i].isSingle = true;
                        }

                        else
                        {
                            buttons.targetButtons[i].gameObject.SetActive(false);
                        }
                    }
                }

                else
                {
                    List<int> players = new List<int>();
                    for (int i = 0; i < BattleManager.instance.activeBattlers.Count; i++)
                    {
                        if (BattleManager.instance.activeBattlers[i].chara.isPlayer)
                        {
                            players.Add(i);
                        }
                    }

                    for (int i = 0; i < buttons.targetButtons.Length; i++)
                    {
                        if (players.Count > i)
                        {
                            buttons.targetButtons[i].gameObject.SetActive(true);
                            buttons.targetButtons[i].moveName = moveName;
                            buttons.targetButtons[i].activeBattlerTarget = players[i];
                            buttons.targetButtons[i].targetName.text = BattleManager.instance.activeBattlers[players[i]].chara.charName;
                            buttons.targetButtons[i].isSingle = true;
                        }

                        else
                        {
                            buttons.targetButtons[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
            if(selfTarget)
            {
                buttons.targetButtons[0].gameObject.SetActive(true);
                buttons.targetButtons[0].moveName = moveName;
                buttons.targetButtons[0].activeBattlerTarget = BattleManager.instance.currentTurn;
                buttons.targetButtons[0].targetName.text = BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.charName;
                buttons.targetButtons[0].isSingle = true;
                for(int i = 1; i< buttons.targetButtons.Length;i++)
                {
                    buttons.targetButtons[i].gameObject.SetActive(false);
                }
            }
            if(allTarget)
            {
                int firstIndex = 0;
                if(targetIsEnemy)
                {
                    for(int i = 0; i<BattleManager.instance.activeBattlers.Count; i++)
                    {
                        if(!BattleManager.instance.activeBattlers[i].chara.isPlayer)
                        {
                            firstIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    for(int i = 0; i<BattleManager.instance.activeBattlers.Count; i++)
                    {
                        if(BattleManager.instance.activeBattlers[i].chara.isPlayer)
                        {
                            firstIndex = i;
                            break;
                        }
                    }
                }
                buttons.targetButtons[firstIndex].gameObject.SetActive(true);
                buttons.targetButtons[firstIndex].moveName = moveName;
                buttons.targetButtons[firstIndex].activeBattlerTarget = firstIndex;
                buttons.targetButtons[firstIndex].targetName.text = "All";
                buttons.targetButtons[firstIndex].isSingle = false;
                for(int i = 0; i< buttons.targetButtons.Length;i++)
                {
                    if(i!=firstIndex) buttons.targetButtons[i].gameObject.SetActive(false);
                }
            }
        }
        if(isItem)
        {
            List<int> players = new List<int>();
            for (int i = 0; i < BattleManager.instance.activeBattlers.Count; i++)
            {
                if (BattleManager.instance.activeBattlers[i].chara.isPlayer)
                {
                    players.Add(i);
                }
            }

            for (int i = 0; i < buttons.targetButtons.Length; i++)
            {
                if (players.Count > i)
                {
                    buttons.targetButtons[i].gameObject.SetActive(true);
                    buttons.targetButtons[i].moveName = moveName;
                    buttons.targetButtons[i].activeBattlerTarget = players[i];
                    buttons.targetButtons[i].targetName.text = BattleManager.instance.activeBattlers[players[i]].chara.charName;
                    buttons.targetButtons[i].isSingle = true;
                }

                else
                {
                    buttons.targetButtons[i].gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void openCypherMenu()
    {
        ChangeCurrentPanel(cypherMenu);
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.movesAvailable.Count == 0)
        {
            ShowPanel(topPanel);
            ChangeTopPanelText("Not Cyphers Available");
        }

        else
        {
            HidePanel(topPanel);
            HidePanel(playerStatsPanel);
            ShowPanel(cypherMenu);
            playerHasSelectedAnOption = true;
            for (int i = 0; i < buttons.cypherButtons.Length; i++)
            {
                if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.movesAvailable.Count > i)
                {
                    buttons.cypherButtons[i].gameObject.SetActive(true);
                    buttons.cypherButtons[i].cypherName = BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.movesAvailable[i];
                    buttons.cypherButtons[i].nameText.text = buttons.cypherButtons[i].cypherName;
                    for (int j = 0; j < BattleManager.instance.movesList.Length; j++)
                    {
                        if (BattleManager.instance.movesList[j].moveName == buttons.cypherButtons[i].cypherName)
                        {
                            buttons.cypherButtons[i].cypherCost = BattleManager.instance.movesList[j].moveCost;
                            buttons.cypherButtons[i].costText.text = buttons.cypherButtons[i].cypherCost.ToString();
                        }
                    }
                }
                else
                {
                    buttons.cypherButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void OpenItemMenu()
    {
        ChangeCurrentPanel(itemMenu);
        HidePanel(topPanel);
        HidePanel(playerStatsPanel);
        ShowPanel(itemMenu);
        playerHasSelectedAnOption = true;
        int[] itemQuantities = GameManager.instance.numberOfItems;
        for (int i = 0; i < buttons.itemButtons.Length; i++)
        {
            if (i < BattleManager.instance.battleItems.items.Count)
            {
                if (itemQuantities[i] > 0)
                {
                    buttons.itemButtons[i].gameObject.SetActive(true);
                    buttons.itemButtons[i].itemName = BattleManager.instance.battleItems.items[i].itemName;
                    buttons.itemButtons[i].NameText.text = BattleManager.instance.battleItems.items[i].itemName;
                    for (int j = 0; j < itemQuantities.Length; j++)
                    {
                        if (i == j)
                        {
                            buttons.itemButtons[i].itemQuantity = itemQuantities[i];
                            buttons.itemButtons[i].QuantityText.text = buttons.itemButtons[i].itemQuantity.ToString();
                        }
                    }
                }

                else
                {
                    buttons.itemButtons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                buttons.itemButtons[i].gameObject.SetActive(false);
            }
        }
    }

}

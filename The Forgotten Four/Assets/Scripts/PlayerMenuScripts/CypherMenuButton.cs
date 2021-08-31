using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class CypherMenuButton : MonoBehaviour
{
    public int caster;
    public Text descriptionText;
    public Text nameText, mpCostText;
    public string cypherDescription;
    public elementType cypherType;
    public GameObject cypherTargetPanel;
    public CypherTargetMenu targetMenu;
    BattleMove myMove;
    bool isAvailable;
    public bool playerSelectedACypher;

    public void CheckIfCypherCanUseInMenu()
    {
        isAvailable = false;
        if(cypherType == elementType.Heal || cypherType == elementType.Support)
        {
            isAvailable = true;
        }
        if (!isAvailable)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = new Color(255, 0, 0, 0.58f);
            GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0.19f);
        }
        else
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            GetComponentInChildren<Text>().color = new Color(255, 255, 255, 1f);
        }
    }

    public void SetValues(int c, string name, string mpCost, string desc, elementType type)
    {
        caster = c;
        nameText.text = name;
        mpCostText.text = mpCost;
        cypherDescription = desc;
        cypherType = type;
        cypherTargetPanel.SetActive(false);
    }

    public void ShowDescriptionOnPanel()
    {
        if (!playerSelectedACypher)
        {
            descriptionText.gameObject.SetActive(true);
        }
        descriptionText.text = cypherDescription;

    }

    public void ActivateSelectionOnMenu()
    {
        playerSelectedACypher = true;
    }

    public void Press()
    {
        descriptionText.gameObject.SetActive(false);
        playerSelectedACypher = true;
        cypherTargetPanel.SetActive(true);
        for(int i = 0; i < BattleManager.instance.movesList.Length; i++)
        {
            if(BattleManager.instance.movesList[i].moveName == nameText.text)
            {
                myMove = BattleManager.instance.movesList[i];
                break;
            }
        }
        for(int i=0; i < targetMenu.targetButtons.Length; i++)
        {
            if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                targetMenu.targetButtons[i].gameObject.SetActive(true);
                targetMenu.targetButtons[i].SetValues(caster,this, myMove, PlayerController.instance.partyStats[i].name);
            }
            else
            {
                targetMenu.targetButtons[i].gameObject.SetActive(false);
            }
        }
    }
}

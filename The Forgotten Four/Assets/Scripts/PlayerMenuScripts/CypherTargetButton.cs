using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEnums;

public class CypherTargetButton : MonoBehaviour
{
    CharStats myCharTarget, myCharCaster;
    public CypherMenuEffect cypherEffect;
    public CypherMenuButton cypherButton;
    public string targetName;
    public GameObject targetPanel;
    public GameObject CypherDescriptionText;
    private string messageModifier;
    private int intModifier;
    

    public void SetValues(int character, CypherMenuButton button, BattleMove move, string name)
    {
        myCharCaster = PlayerController.instance.partyStats[character];
        cypherButton = button;
        cypherEffect.move = move;
        targetName = name;
        GetComponentInChildren<Text>().text = targetName;
    }

    public void Press()
    {
        if (myCharCaster.GetCharStats().GetCurrentMP() < cypherEffect.move.moveCost)
        {
            targetPanel.SetActive(false);
            CypherDescriptionText.SetActive(true);
            messageModifier = "Not Enough MP!";
            CypherDescriptionText.GetComponent<Text>().text = messageModifier;
            cypherButton.Invoke("ActivateSelectionOnMenu", 0.25f);
        }

        else
        {
            for (int i = 0; i < PlayerController.instance.partyStats.Length; i++)
            {
                if (PlayerController.instance.partyStats[i].name == targetName)
                {
                    myCharTarget = PlayerController.instance.partyStats[i];
                    break;
                }
            }
            switch (cypherEffect.move.eType)
            {
                case elementType.Heal:
                    intModifier = cypherEffect.HealEffect(myCharTarget, myCharCaster);
                    break;
                case elementType.Support:
                    break;
            }
            int currentMP = myCharCaster.GetCharStats().GetCurrentMP();
            currentMP -= cypherEffect.move.moveCost;
            myCharCaster.GetCharStats().SetCurrentMP(currentMP);
            targetPanel.SetActive(false);
            CypherDescriptionText.SetActive(true);
            messageModifier = ModifyMessage();
            CypherDescriptionText.GetComponent<Text>().text = messageModifier;
            cypherButton.Invoke("ActivateSelectionOnMenu", 0.25f);
        }
    }

    public string ModifyMessage()
    {
        messageModifier = "";

        switch (cypherEffect.move.eType)
        {
            case elementType.Heal:
                messageModifier = targetName + " has recovered " + intModifier + " HP!";
                break;
            case elementType.Support:
                break;
        }

        return messageModifier;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCypherSelect : MonoBehaviour
{
    public string cypherName;
    public int cypherCost;
    public Text nameText, costText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= cypherCost)
        {
            BattleManager.instance.cypherMenu.SetActive(false);
            BattleManager.instance.playerStatsPanel.SetActive(true);
            BattleManager.instance.OpenTargetMenu(cypherName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= cypherCost;
            BattleManager.instance.pointsInCasePlayerGoesBack = cypherCost;
        }

        else
        {
            BattleManager.instance.warningWindow.SetActive(true);
            BattleManager.instance.warningWindow.transform.GetChild(0).GetComponent<Text>().text = "Not enough MP!";
        }
    }
}

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

    public void Press()
    {
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.currentMP >= cypherCost)
        {
            BattleManager.instance.battleUI.HidePanel(BattleManager.instance.battleUI.cypherMenu);
            BattleManager.instance.battleUI.ShowPanel(BattleManager.instance.battleUI.playerStatsPanel);
            BattleManager.instance.battleUI.OpenTargetMenu(cypherName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].chara.currentMP -= cypherCost;
            BattleManager.instance.pointsInCasePlayerGoesBack = cypherCost;
        }

        else
        {

            BattleManager.instance.battleUI.ShowPanel(BattleManager.instance.battleUI.topPanel);
            BattleManager.instance.battleUI.ChangeTopPanelText("Not enough MP!");
        }
    }
}

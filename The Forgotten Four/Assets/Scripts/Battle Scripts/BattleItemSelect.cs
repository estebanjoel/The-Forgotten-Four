using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public string itemName;
    public int itemQuantity;
    public Text NameText, QuantityText;

    public void Press()
    {
        BattleManager.instance.battleUI.HidePanel(BattleManager.instance.battleUI.itemMenu);
        BattleManager.instance.battleUI.ShowPanel(BattleManager.instance.battleUI.playerStatsPanel);
        BattleManager.instance.battleUI.OpenTargetMenu(itemName);
    }
}

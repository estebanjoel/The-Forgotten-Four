using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public string itemName;
    public int itemQuantity;
    public Text NameText, QuantityText;
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
        BattleManager.instance.cypherMenu.SetActive(false);
        BattleManager.instance.playerStatsPanel.SetActive(true);
    }
}

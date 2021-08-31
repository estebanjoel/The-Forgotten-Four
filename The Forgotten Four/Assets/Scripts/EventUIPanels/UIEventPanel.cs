using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventPanel : MonoBehaviour
{
    public GameObject currentPanel, previousPanel;
    public GameObject eventMessagePanel;
    public GameObject itemMessagePanel;
    public Text itemMessageText;
    public GameObject levelUpPanel;
    public LevelUPCharPanels levelUPCharPanels;
    [Header ("Load_Save Panels")]
    public GameObject LoadSavePanel;
    public GameObject MainLoadSavePanel;
    public GameObject SavePanel;
    public GameObject LoadPanel;
    public GameObject OverwritePanel;
    public GameObject SaveSuccessfullPanel;
    public GameObject LoadSuccessfullPanel;
    public Button[] saveSlots;
    public Button[] loadSlots;
    public GameObject restoreEventPanel;
    public GameObject keycardEventPanel;
    public Text keycardEventText;
    public void ReturnButton()
    {
        HideAllPanels();
        Time.timeScale = 1f;
    }

    public void HideAllPanels()
    {
        eventMessagePanel.SetActive(false);
        itemMessagePanel.SetActive(false);
        levelUpPanel.SetActive(false);
        LoadSavePanel.SetActive(false);
        MainLoadSavePanel.SetActive(false);
        SavePanel.SetActive(false);
        LoadPanel.SetActive(false);
        OverwritePanel.SetActive(false);
        SaveSuccessfullPanel.SetActive(false);
        LoadSuccessfullPanel.SetActive(false);
        restoreEventPanel.SetActive(false);
        keycardEventPanel.SetActive(false);
    }

    public void ShowEventPanel()
    {
        eventMessagePanel.SetActive(true);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ChangeCurrentPanel(GameObject panel)
    {
        if(panel == SavePanel || panel == LoadPanel) previousPanel = MainLoadSavePanel;
        else previousPanel = currentPanel;
        currentPanel = panel;
        /*if(currentPanel!= null) previousPanel = currentPanel;
        currentPanel = panel;*/
    }

    public void GoToPreviousPanel()
    {
        if(currentPanel == OverwritePanel) previousPanel = SavePanel;
        currentPanel.SetActive(false);
        previousPanel.SetActive(true);
        ChangeCurrentPanel(previousPanel);
    }

    public void ChangeTextOnPanel(Text myText, string message)
    {
        myText.text = message;
    }

    public void ShowLevelUpCharPanel(int i)
    {
        levelUPCharPanels.panels[i].gameObject.SetActive(true);
        SetLevelUPModifiersPanel(PlayerController.instance.partyStats[i],i);
    }

    public void SetLevelUPModifiersPanel(CharStats myChar, int i)
    {
        levelUPCharPanels.panels[i].charNameText.text = myChar.charName;
        levelUPCharPanels.panels[i].strengthText.text = "STR+"+myChar.strengthModifier;
        levelUPCharPanels.panels[i].dexterityText.text = "DEX+"+myChar.dexterityModifier;
        levelUPCharPanels.panels[i].vitalityText.text = "VIT+"+myChar.vitalityModifier;
        levelUPCharPanels.panels[i].defenceText.text = "DEF+"+myChar.defenceModifier;
        levelUPCharPanels.panels[i].elementText.text = "ELE+"+myChar.elementModifier;
        levelUPCharPanels.panels[i].spiritText.text = "STR+"+myChar.spiritModifier;
        levelUPCharPanels.panels[i].hpText.text = "HP+"+myChar.hpModifier;
        levelUPCharPanels.panels[i].mpText.text = "MP+"+myChar.mpModifier;
        levelUPCharPanels.panels[i].weaponPowerText.text = "W.P.+"+myChar.weaponPowerModifier;
        levelUPCharPanels.panels[i].armorPowerText.text = "A.P.+"+myChar.armorPowerModifier;
        levelUPCharPanels.panels[i].magicPowerText.text = "M.P.+"+myChar.magicPowerModifier;
        levelUPCharPanels.panels[i].magicDefenceText.text = "M.D.+"+myChar.magicDefenceModifier;
        levelUPCharPanels.panels[i].accuracyText.text = "ACC+"+myChar.accuracyModifier;
        levelUPCharPanels.panels[i].evasionText.text = "EVA+"+myChar.evasionModifier;
        levelUPCharPanels.panels[i].initiativeText.text = "INI+"+myChar.initiativeModifier;
        levelUPCharPanels.panels[i].fireResistanceText.text = "FireR+"+myChar.fireResistanceModifier;
        levelUPCharPanels.panels[i].iceResistanceText.text = "IceR+"+myChar.iceResistanceModifier;
        levelUPCharPanels.panels[i].electricResistanceText.text = "ElecR+"+myChar.electricResistanceModifier;
        levelUPCharPanels.panels[i].plasmaResistanceText.text = "PlasR+"+myChar.plasmaResistanceModifier;
        levelUPCharPanels.panels[i].psychicResistanceText.text = "PsyR+"+myChar.psychicResistanceModifier;
        levelUPCharPanels.panels[i].toxicResistanceText.text = "ToxR+"+myChar.toxicResistanceModifier;
        if(myChar.playerLevel<5) levelUPCharPanels.panels[i].newCypherText.text = "You have acquired:\n"+myChar.CypherRewards[myChar.playerLevel-2];
    }

    public void HideAllLevelUpCharPanels()
    {
        for(int i = 0; i<levelUPCharPanels.panels.Length;i++)
        {
            levelUPCharPanels.panels[i].gameObject.SetActive(false);
        }
    }
}

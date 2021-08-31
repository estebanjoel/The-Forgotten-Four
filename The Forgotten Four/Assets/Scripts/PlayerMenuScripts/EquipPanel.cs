using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipPanel : MonoBehaviour
{
    [Header("Char Bar")]
    public int CharIndex;
    public Image charHead;
    public Text levelText;
    public Image xpBarFiller;
    public Text hpText;
    public Text mpText;
    [Header("Char Main Stats")]
    public Text strengthText;
    public Text dexterityText;
    public Text vitalityText;
    public Text defenceText;
    public Text elementText;
    public Text spiritText;
    [Header("Char Secondary Stats")]
    public Text weaponPowerText;
    public Text armorPowerText;
    public Text magicPowerText;
    public Text magicDefenceText;
    public Text accuracyText;
    public Text evasionText;
    public Text initiativeText;
    [Header("Char Elemental Resistances")]
    public Text fireResistanceText;
    public Text iceResistanceText;
    public Text electricResistanceText;
    public Text plasmaResistanceText;
    public Text psychicResistanceText;
    public Text toxicResistanceText;
    [Header("Equipped Items")]
    public EquippedItem equippedWeapon;
    public EquippedItem equippedArmor;
    [Header("Footer Text")]
    public Text previousCharName;
    public Text nextCharName;
    public EquipMenuButtons panelButtons;

    public void HideAllStatModifiers()
    {
        strengthText.transform.GetChild(0).gameObject.SetActive(false);
        dexterityText.transform.GetChild(0).gameObject.SetActive(false);
        vitalityText.transform.GetChild(0).gameObject.SetActive(false);
        defenceText.transform.GetChild(0).gameObject.SetActive(false);
        elementText.transform.GetChild(0).gameObject.SetActive(false);
        spiritText.transform.GetChild(0).gameObject.SetActive(false);
        weaponPowerText.transform.GetChild(0).gameObject.SetActive(false);
        armorPowerText.transform.GetChild(0).gameObject.SetActive(false);
        magicPowerText.transform.GetChild(0).gameObject.SetActive(false);
        magicDefenceText.transform.GetChild(0).gameObject.SetActive(false);
        accuracyText.transform.GetChild(0).gameObject.SetActive(false);
        evasionText.transform.GetChild(0).gameObject.SetActive(false);
        initiativeText.transform.GetChild(0).gameObject.SetActive(false);
        fireResistanceText.transform.GetChild(0).gameObject.SetActive(false);
        iceResistanceText.transform.GetChild(0).gameObject.SetActive(false);
        electricResistanceText.transform.GetChild(0).gameObject.SetActive(false);
        plasmaResistanceText.transform.GetChild(0).gameObject.SetActive(false);
        psychicResistanceText.transform.GetChild(0).gameObject.SetActive(false);
        toxicResistanceText.transform.GetChild(0).gameObject.SetActive(false);
    }
}

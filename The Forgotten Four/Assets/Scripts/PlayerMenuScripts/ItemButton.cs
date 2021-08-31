using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Image itemActionPanel;
    public GameObject itemTargetPanel, itemMessagePanel;
    public ItemUseButton useButton;
    
    // Start is called before the first frame update

    public void ShowItemDetails()
    {
        itemNameText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]).itemName;
        itemDescriptionText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]).description;
        itemMessagePanel.SetActive(false);
        itemTargetPanel.SetActive(false);
        itemActionPanel.gameObject.SetActive(true);
        useButton.itemToUse = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]);
    }

}

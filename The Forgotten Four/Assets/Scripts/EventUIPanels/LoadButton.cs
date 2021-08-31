using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
    public SaveSlot mySlot;
    
    // Start is called before the first frame update
    void Start()
    {
        mySlot = GetComponent<SaveSlot>();
    }

    // Update is called once per frame
    public void Load()
    {
        LoadSaveController.instance.activeSlot = mySlot.slot;
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindObjectOfType<MenuFadeScreen>().FadeToBlack();
            LoadSaveController.instance.StartLoadInMenuCo(mySlot.slot);
        }
        else
        {
            LoadSaveController.instance.StartLoadCo();
        }
    }

    
}

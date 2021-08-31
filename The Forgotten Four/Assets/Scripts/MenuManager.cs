using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public UIManager manager;
    public int currentOption;
    public GameObject[] buttons;

    private void Awake()
    {
        ChangeSelection();
    }
    void Update()
    {
        CheckKeys();
    }

    void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigationUp();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigationDown();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            manager.ShowScreen(currentOption+1);
        }
    }

    void NavigationUp()
    {
        currentOption--;
        if (currentOption < 0)
        {
            currentOption = (buttons.Length) - 1;
        }

        ChangeSelection();
    }

    void NavigationDown()
    {
        currentOption++;
        if (currentOption > (buttons.Length) - 1)
        {
            currentOption = 0;
        }

        ChangeSelection();

    }

    void ChangeSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = new Vector3(1, 1, 1);
        }

        buttons[currentOption].transform.localScale = new Vector3(1, 1, 1) * 1.25f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] screens = new GameObject[3];
    public ExitGame exitGame;
    public int currentScreen;
    public int mainMenuScreen = 0;
    public int gameplayScreen = 1;
    public int exit = 2;

    void Start()
    {

        exitGame = transform.GetComponent<ExitGame>();

        ShowScreen(mainMenuScreen);
    }

    public void ShowScreen(int screenToShow)
    {
        currentScreen = screenToShow;
        if (currentScreen == gameplayScreen) SceneManager.LoadScene("Map_Play");
        else if(currentScreen == exit) exitGame.Invoke("Exit", 1f);
    }

}

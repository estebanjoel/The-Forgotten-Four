using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTest : MonoBehaviour
{
    public GameObject loadPanel;
    public string[] charNames;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void showLoadPanel()
    {
        loadPanel.SetActive(true);
        for(int i = 0; i<loadPanel.GetComponentsInChildren<SaveSlot>().Length;i++)
        {
            loadPanel.GetComponentsInChildren<SaveSlot>()[i].CheckSlotInfoInMainMenu();
        }
    }

    public void hideLoadPanel()
    {
        loadPanel.SetActive(false);
    }

    public void ChangeScene()
    {
        StartCoroutine(ChangeSceneCo());
    }

    public void Quit()
    {
        StartCoroutine(QuitCo());
    }

    public IEnumerator ChangeSceneCo()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("IntroScene");
    }

    public IEnumerator QuitCo()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}

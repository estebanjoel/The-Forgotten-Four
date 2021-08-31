using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string currentScene;
    public string[] levels;
    
    public bool CheckCurrentLevel()
    {
        if(SceneManager.GetActiveScene().name == currentScene)
        {
            return true;
        }

        else
        {
            currentScene = SceneManager.GetActiveScene().name;
            return false;
        }
    }

    public bool CheckIfSceneIsGameplay()
    {
        if (currentScene == "MainMenu" || currentScene == "EndProto") return false;
        else return true;
    }

    public IEnumerator NextLevelCo()
    {
        yield return new WaitForSeconds(1.5f);

        // if(currentScene==levels[levels.Length-1])
        if(currentScene==levels[1])
        {
            LoadLevel("endProto");
        }

        else
        {
            for(int i = 0; i<levels.Length-1;i++)
            {
                if(levels[i] == currentScene)
                {
                    LoadLevel(levels[i+1]);
                    break;
                }
            }
        }
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void SetNewLevelValues()
    {
        GameManager.instance.SetLevelElements();
        PlayerController.instance.SetNewLevelValues();
        CameraController.instance.UpdateCameraBounds();
        CameraController.instance.UpdateTarget();
        //UIFade.instance.FadeFromBlack();
    }
    
}

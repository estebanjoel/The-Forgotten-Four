using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject CustomCamera;
    public GameObject gameManager;
    public GameObject battleManager;
    public GameObject audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
            UIFade.instance.FadeFromBlack();
        }

        if (PlayerController.instance == null)
        {
            Instantiate(player);
        }

        if(CameraController.instance == null)
        {
            Instantiate(CustomCamera);
        }

        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (BattleManager.instance == null)
        {
            Instantiate(battleManager);
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }
}

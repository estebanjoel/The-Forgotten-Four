using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFadeScreen : MonoBehaviour
{
    public Image fadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToBlack;
    private float timer;
    public Button[] menuButtons;
    public Text[] menuTexts;
    public AudioClip myClip;
    
    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            timer += Time.deltaTime;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * timer));
            if (fadeScreen.color.a >= 1f)
            {
                shouldFadeToBlack = false;
            }

            foreach(Button button in menuButtons)
            {
                button.gameObject.SetActive(false);
            }
            foreach(Text text in menuTexts)
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        timer = 0;
        Camera.main.GetComponent<AudioSource>().Stop();
        Camera.main.GetComponent<AudioSource>().clip = myClip;
        Camera.main.GetComponent<AudioSource>().Play();
        Camera.main.GetComponent<AudioSource>().loop = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public string firstLevel;
    bool shouldFadeToBlack;
    bool gameHasStarted;
    float fadeTimer;
    float skipTimer;
    float fadeSpeed = 1.5f;
    public Image fadeScreen;
    public AudioSource introSource;
    public AudioClip introClip;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        skipTimer+= Time.deltaTime;

        if (shouldFadeToBlack)
        {
            fadeTimer += Time.deltaTime;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * fadeTimer));
            if(fadeScreen.color.a >= 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if(Input.GetButtonDown("Cancel") && !gameHasStarted)
        {
            if(skipTimer>=3f)
            {
                startGame();
            }
        }
    }

    public void PlayClip()
    {
        introSource.Stop();
        introSource.clip = introClip;
        introSource.loop = false;
        introSource.Play();
    }

    public void startGame()
    {
        PlayClip();
        animator.SetTrigger("startTrigger");
        FadeToBlack();
        gameHasStarted = true;
        StartCoroutine(startGameCo());
    }

    public IEnumerator startGameCo()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(firstLevel);
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        fadeTimer = 0;
    }

}

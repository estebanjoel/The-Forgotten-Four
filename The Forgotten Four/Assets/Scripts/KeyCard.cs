using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    public string cardName;
    public bool hasBeenGrabbed;
    public bool hasEncounteredAPlayer;
    public float topBound, bottomBound, target, speed;
    public AudioClip keycardAudio;

    void Start() 
    {
        target = topBound;    
    }
    
    void Update() 
    {
        if(Input.GetButtonDown("Fire1")) CheckIfPlayerIsAtKey();
        if(target == topBound)
        {
            if(transform.position.y <= target) transform.position+=transform.up * speed * Time.deltaTime;
            else target = bottomBound;
        }
        if(target == bottomBound)
        {
            if(transform.position.y >= target) transform.position-=transform.up * speed * Time.deltaTime;
            else target = topBound;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player") hasEncounteredAPlayer = true;
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player") hasEncounteredAPlayer = false;
    }

    public void CheckIfPlayerIsAtKey()
    {
        if(hasEncounteredAPlayer)
        {
            hasBeenGrabbed = true;
            AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformSFX[2], keycardAudio);
            AudioManager.instance.PlatformSFX[2].Play();
            GameManager.instance.eventManager.GrabKeyEvent(cardName, GameObject.FindObjectOfType<Door>());
            gameObject.SetActive(false);
        }
    }
}

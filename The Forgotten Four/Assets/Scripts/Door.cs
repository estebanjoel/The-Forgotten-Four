using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyCards doorKeys;
    // Start is called before the first frame update
    bool encounteredAPlayer;
    Animator anim;
    public AudioClip doorAudio;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1")) CheckIfAllKeysAreCollected();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Player") encounteredAPlayer = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag=="Player") encounteredAPlayer = false;
    }

    public void CheckIfAllKeysAreCollected()
    {
        if(encounteredAPlayer)
        {
            bool allKeysAreCollected = true;
            for(int i = 0; i<doorKeys.keycards.Length;i++)
            {
                if(!doorKeys.keycards[i].hasBeenGrabbed)
                {
                    allKeysAreCollected = false;
                    break;
                }
            }
            if(allKeysAreCollected)
            {
                AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformSFX[2], doorAudio);
                AudioManager.instance.PlatformSFX[2].Play();
                anim.SetBool("openDoor", true);
                Invoke("ChangeColliderToTrigger",0.15f);
            }
        }
    }

    public void ChangeColliderToTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
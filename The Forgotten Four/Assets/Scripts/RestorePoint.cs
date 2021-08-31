using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePoint : MonoBehaviour
{
    Animator anim;
    public bool hasEncounteredAPlayer;
    public bool hasBeenActivated;
    public AudioClip restoreClip;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBeenActivated) anim.SetTrigger("usedTrigger");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            hasEncounteredAPlayer = true;
        }
    }

    public void ActivatePoint()
    {
        hasBeenActivated = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformSFX[2], restoreClip);
        AudioManager.instance.PlatformSFX[2].Play();
    }
}

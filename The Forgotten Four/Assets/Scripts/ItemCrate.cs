using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrate : MonoBehaviour
{
    public string[] itemsContained;
    public int[] itemsContainedQuantity;
    public bool hasBeenOpened;
    public bool isActive;
    public Animator anim;
    public AudioClip crateClip;

    void Start() 
    {
        anim=GetComponent<Animator>();
        isActive = true;
    }
    
    // Start is called before the first frame update
    public void OpenCrate()
    {
        hasBeenOpened = true;
        anim.SetBool("isOpen",true);
        for(int i = 0; i<GameManager.instance.itemsHeld.Length; i++)
        {
            for(int j = 0; j < itemsContained.Length; j++)
            {
                if (GameManager.instance.itemsHeld[i] == itemsContained[j])
                {
                    GameManager.instance.numberOfItems[i] += itemsContainedQuantity[j];
                }
            }
        }
        AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformSFX[2], crateClip);
        AudioManager.instance.PlatformSFX[2].Play();
    }

    public void DeactivateCrate()
    {
        isActive = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public string CrateMessage(string message)
    {
        message = "You have found:\n";
        for(int i = 0; i < itemsContained.Length; i++)
        {
            message += itemsContained[i] + " x" + itemsContainedQuantity[i] + "\n";
        }
        return message;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(isActive) OpenCrate();
        }
    }
}

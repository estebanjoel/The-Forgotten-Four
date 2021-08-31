using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float effectLength;
    public int sfx;
    
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.PlaySFX(sfx);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, effectLength);
    }
}

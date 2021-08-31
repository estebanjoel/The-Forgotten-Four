using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovingPlatform : MonoBehaviour
{
    public float minPos, maxPos, target, speed;
    public bool movementVertical, movementHorizontal;
    private bool targetMin, targetMax;
    [SerializeField] bool canPlayOnEditMode;

    // Start is called before the first frame update
    void Start()
    {
        targetMax = true;
    }

    // Update is called once per frame
    void Update()
    {
       if(!Application.isPlaying)
       {
           if(canPlayOnEditMode)
           {
                if (movementVertical) MoveVertically();
                if (movementHorizontal) MoveHorizontally();
           }
       }
       else
       {
            canPlayOnEditMode = false;
            if (movementVertical) MoveVertically();
            if (movementHorizontal) MoveHorizontally();
       }
    }

    public void MoveVertically()
    {
        movementHorizontal = false;
        if (targetMax)
        {
            target = maxPos;
            transform.position += transform.up * speed * Time.deltaTime;
            if (transform.position.y >= target)
            {
                targetMax = false;
                targetMin = true;
            }
        }

        if (targetMin)
        {
            target = minPos;
            transform.position -= transform.up * speed * Time.deltaTime;
            if (transform.position.y <= target)
            {
                targetMin = false;
                targetMax = true;
            }
        }
    }

    public void MoveHorizontally()
    {
        movementVertical = false;
        if (targetMax)
        {
            target = maxPos;
            transform.position += transform.right * speed * Time.deltaTime;
            if (transform.position.x >= target)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                targetMax = false;
                targetMin = true;
            }
        }

        if (targetMin)
        {
            target = minPos;
            transform.position -= transform.right * speed * Time.deltaTime;
            if (transform.position.x <= target)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                targetMin = false;
                targetMax = true;
            }
        }
    }
}

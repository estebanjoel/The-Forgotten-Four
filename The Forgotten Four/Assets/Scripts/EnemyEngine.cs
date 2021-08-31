using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngine : MonoBehaviour
{
    public bool hasFlipped;
    public Transform leftPositionChild, rightPositionChild;
    public EnemyController myController;
    
    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<EnemyController>();
        leftPositionChild = transform.GetChild(0);
        rightPositionChild = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (myController.hasFlipped)
        {
            leftPositionChild.GetChild(0).gameObject.SetActive(false);
            rightPositionChild.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            hasFlipped = CheckFlip();
            if (hasFlipped)
            {
                leftPositionChild.GetChild(0).gameObject.SetActive(false);
                rightPositionChild.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                leftPositionChild.GetChild(0).gameObject.SetActive(true);
                rightPositionChild.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public bool CheckFlip()
    {
        if (myController.GetComponent<SpriteRenderer>().flipX)
        {
            hasFlipped = true;
        }

        else
        {
            hasFlipped = false;
        }

        return hasFlipped;
    }
}

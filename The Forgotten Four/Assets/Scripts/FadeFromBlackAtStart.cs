using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFromBlackAtStart : MonoBehaviour
{
    void Start()
    {
        UIFade.instance.FadeFromBlack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CypherMenuCharButton : MonoBehaviour
{
    public BattleChar myChar;
    public Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText.text = myChar.charName;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyController[] enemiesOnScreen;
    public GameObject battleManager;

    public CheckPoint[] checkPoints;
    public Transform startPoint;
    public Transform lastCheckPoint;

    public bool battleActive;
    private string savedScene;
    private int activeEnemy;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;
    
    // Start is called before the first frame update
    void Start()
    {
        enemiesOnScreen = GameObject.FindObjectsOfType<EnemyController>();
        
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        battleManager = GameObject.FindGameObjectWithTag("Battle");

        savedScene = SceneManager.GetActiveScene().name;
        checkPoints = GameObject.FindObjectsOfType<CheckPoint>();
        startPoint = GameObject.Find("StartingPoint").transform;
        PlayerController.instance.transform.position = startPoint.position;
        lastCheckPoint = startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckIfPlayerIsOnPortal())
        {
            if (CheckIfSceneIsGameplay())
            {   
                if (battleActive)
                {
                    PlayerController.instance.canMove = false;
                    if (BattleManager.instance.battleHasEnded)
                    {
                        StartCoroutine(endBattleCo(activeEnemy));
                    }
                }

                else
                {
                    PlayerController.instance.canMove = true;
                    UpdateStartPoint();
                    CheckBattle();
                }
            }
        }

        else
        {
            StartCoroutine(endProtoCo());
        }
        
    }

    public void CheckBattle()
    {
        /*if (PlayerController.instance.gotDamage)
        {
            BattleManager.instance.BattleStart();
        }*/

        string currentScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < enemiesOnScreen.Length; i++)
        {
            if (enemiesOnScreen[i].gotDamage && enemiesOnScreen[i].gameObject.activeInHierarchy)
            {
                activeEnemy = i;
                StartCoroutine(startBattleCo(activeEnemy));
            }        
        }
    }

    public void UpdateStartPoint()
    {
        for(int i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i].playerHasPassedThroughHere)
            {
                lastCheckPoint = checkPoints[i].transform;
            }
        }
    }

    public bool CheckIfPlayerIsOnPortal()
    {
        if (PlayerController.instance.isOnPortal)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool CheckIfSceneIsGameplay()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != savedScene)
        {
            if (currentScene == "MainMenu" || currentScene == "EndProto")
            {
                if (PlayerController.instance != null)
                {
                    Destroy(PlayerController.instance.gameObject);
                }
                if (BattleManager.instance != null)
                {
                    Destroy(BattleManager.instance.gameObject);
                }
                if (CameraController.instance != null)
                {
                    Destroy(CameraController.instance.gameObject);
                }
                if (UIFade.instance != null)
                {
                    Destroy(UIFade.instance.gameObject);
                }
                if (instance != null)
                {
                    Destroy(gameObject);
                }
            }

            else
            {
                savedScene = currentScene;
                return true;
            }
        }

        return true;
    }

    public Item GetItemDetails(string ItemToGrab)
    {
        for(int i=0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i].itemName == ItemToGrab)
            {
                return referenceItems[i];
            }
        }
        return null;
    }

    public IEnumerator startBattleCo(int i)
    {
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        BattleManager.instance.BattleStart(enemiesOnScreen[i].enemyParty);
        enemiesOnScreen[i].gameObject.SetActive(false);
        UIFade.instance.FadeFromBlack();
    }

    public IEnumerator endBattleCo(int i)
    {
        StartCoroutine(BattleManager.instance.EndBattleCo());
        yield return new WaitForSeconds(1f);
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        StartCoroutine(BattleManager.instance.EndBattle2Co());
        enemiesOnScreen[i].gameObject.SetActive(false);
    }

    public IEnumerator endProtoCo()
    {
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndProto");
    }
}

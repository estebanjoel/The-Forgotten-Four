using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EventManager eventManager;
    public LevelManager levelManager;
    public EnemyController[] enemiesOnScreen;
    public GameObject battleManager;
    public BattleBG battleBG;
    public GameObject theMenu;
    public LevelMusic lvlMusic;
    public CheckPoint[] checkPoints;
    public Transform startPoint;
    public Transform lastCheckPoint;

    public bool gameHasBeenSaved;
    public bool battleActive;
    public bool hasBeenAnLevelUP;
    public bool[] levelUPChars;
    public List<SavingPoint> savingPoints;
    public List<RestorePoint> restorePoints;
    private int activeEnemy;
    private bool platformMovementVertical, platformMovementHorizontal;
    public bool bossIsDead;
    private bool allPlayersAreDead;
    private bool battleHasEnded;
    private bool startPositionSetted;
    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;
    public List<ItemCrate> levelCrates = new List<ItemCrate>();
    public int credits;

    public GameObject warningText;
    public AudioClip battleTransitionSFX, portalSFX;
    string messageToPanel;

    // Start is called before the first frame update
    void Start()
    {

        #region Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        #endregion

        SetLevelElements();
        UIFade.instance.Invoke("FadeFromBlack",1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.CheckIfSceneIsGameplay())
        {
            if(levelManager.CheckCurrentLevel())
            {
                if (!CheckIfPlayerIsOnPortal())
                {
                    if (battleActive)
                    {
                        if (BattleManager.instance.battleHasEnded)
                        {
                            if(!battleHasEnded)
                            {
                                battleHasEnded=true;
                                endBattle(activeEnemy);
                            }
                        }
                    }

                    else
                    {
                        if (!theMenu.activeInHierarchy)
                        {
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                theMenu.SetActive(true);
                                theMenu.GetComponent<PlayerMenu>().showMenu(0);
                                theMenu.GetComponent<PlayerMenu>().ShowCharStats(0);
                            }
                            CheckActiveCratesOnLevel();
                            CheckIfPlayerIsOnRestorePoint();
                            UpdateStartPoint();
                            CheckBattle();
                            if(CheckIfSavePointIsActivated())
                            {
                                if(!eventManager.eventPanel.LoadSavePanel.activeInHierarchy)
                                {
                                    if(Input.GetButtonDown("Fire1"))
                                    {
                                        eventManager.ActivateEvent();
                                        eventManager.ShowEventPanel();
                                        eventManager.eventPanel.ShowPanel(eventManager.eventPanel.LoadSavePanel);
                                        eventManager.eventPanel.ShowPanel(eventManager.eventPanel.MainLoadSavePanel);
                                    }
                                }
                            }
                            if (PlayerController.instance.hasFallenToACliff)
                            {
                                RespawnPlayerOnLastCheckPoint();
                                PlayerController.instance.hasFallenToACliff = false;
                            }
                            if (Input.GetKeyDown(KeyCode.M))
                            {
                                UIFade.instance.FadeToBlack();
                                PlayerController.instance.canMove = false;
                                StartCoroutine(levelManager.NextLevelCo());
                            }
                        }

                        else
                        {
                            Time.timeScale = 0f;

                            if (theMenu.GetComponent<PlayerMenu>().toMainMenu)
                            {
                                UIFade.instance.FadeToBlack();
                                theMenu.SetActive(false);
                                PlayerController.instance.canMove = false;
                                Time.timeScale = 1f;
                                Invoke("MainMenu", 1.5f);
                            }

                            if (theMenu.GetComponent<PlayerMenu>().quit)
                            {
                                UIFade.instance.FadeToBlack();
                                theMenu.SetActive(false);
                                PlayerController.instance.canMove = false;
                                Time.timeScale = 1f;
                                Invoke("QuitGame", 1.5f);

                            }

                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                theMenu.GetComponent<PlayerMenu>().PlayCancelAudio();
                                theMenu.SetActive(false);
                                Time.timeScale = 1f;
                            }
                        }                        
                    }

                    warningText.SetActive(false);
                }

                else
                {
                    if (bossIsDead) PortalTransition();
                    else warningText.SetActive(true);
                }
            }

            else
            {
                levelManager.SetNewLevelValues();
            }
        }

        else
        {
            DestroyAllInstances();
        }
    }

    public void SetLevelElements()
    {
        enemiesOnScreen = GameObject.FindObjectOfType<Enemies>().enemiesOnScreen;
        battleManager = GameObject.FindGameObjectWithTag("Battle");
        checkPoints = GameObject.FindObjectsOfType<CheckPoint>();
        startPoint = GameObject.Find("StartingPoint").transform;
        savingPoints = GameObject.FindObjectOfType<SavingPoints>().savingPoints;
        restorePoints = GameObject.FindObjectOfType<RestorePoints>().restorePoints;
        PlayerController.instance.transform.position = startPoint.position;
        lastCheckPoint = startPoint;
        warningText = UIFade.instance.transform.Find("LevelUnfinishedWarning").gameObject;
        warningText.SetActive(false);
        theMenu = UIFade.instance.transform.Find("PlayerMenu").gameObject;
        lvlMusic = GameObject.FindObjectOfType<LevelMusic>();
        AudioManager.instance.Ambience = AudioManager.instance.ChangeAudioClip(AudioManager.instance.Ambience, lvlMusic.levelAmbience);
        AudioManager.instance.PlatformBGM = AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformBGM, lvlMusic.levelBGM);
        levelCrates = GameObject.FindObjectOfType<ItemCrates>().levelCrates;
        AudioManager.instance.PlayPlatformBGM();
        AudioManager.instance.PlayAmbience();
        for(int i = 0; i < PlayerController.instance.partyStats.Length; i++)
        {
            if(PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
            {
                PlayerController.instance.partyStats[i].restoreHP();
                PlayerController.instance.partyStats[i].restoreMP();
            }
        }
        battleBG = GameObject.FindObjectOfType<BattleBG>();
        BattleManager.instance.setBattleBG(battleBG.GetSprite());
    }

    public void PlayBattleTransitionSFX()
    {
        AudioManager.instance.PlatformSFX[4].clip = battleTransitionSFX;
        AudioManager.instance.PlatformSFX[4].Play();
    }

    public void CheckBattle()
    {
        if (PlayerController.instance.gotDamage)
        {
            for (int i = 0; i < enemiesOnScreen.Length; i++)
            {
                if (enemiesOnScreen[i].attackedSuccesfully && enemiesOnScreen[i].gameObject.activeInHierarchy)
                {
                    UIFade.instance.FadeToBlack();
                    activeEnemy = i;
                    BattleManager.instance.EnemyAdvantage();
                    PlayerController.instance.anim.SetFloat("speedX", 0f);
                    PlayerController.instance.canMove = false;
                    StartCoroutine(startBattleCo(activeEnemy));
                    break;
                }
            }
        }

        else if (PlayerController.instance.encounteredAnEnemy)
        {
            for (int i = 0; i < enemiesOnScreen.Length; i++)
            {
                if (enemiesOnScreen[i].encounteredThePlayer && enemiesOnScreen[i].gameObject.activeInHierarchy)
                {
                    UIFade.instance.FadeToBlack();
                    activeEnemy = i;
                    PlayerController.instance.canMove = false;
                    PlayerController.instance.anim.SetFloat("speedX", 0f);
                    StartCoroutine(startBattleCo(activeEnemy));
                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < enemiesOnScreen.Length; i++)
            {
                if (enemiesOnScreen[i].gotDamage && enemiesOnScreen[i].gameObject.activeInHierarchy)
                {
                    UIFade.instance.FadeToBlack();
                    activeEnemy = i;
                    PlayerController.instance.canMove = false;
                    PlayerController.instance.anim.SetFloat("speedX", 0f);
                    BattleManager.instance.PlayerAdvantage();
                    StartCoroutine(startBattleCo(activeEnemy));
                    break;
                }
            }
        }
    }

    public void UpdateStartPoint()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i].playerHasPassedThroughHere)
            {
                lastCheckPoint = checkPoints[i].transform;
            }
        }
    }

    public bool CheckIfPlayerIsOnPortal()
    {
        if (PlayerController.instance.isOnPortal) return true;
        else return false;
    }
    
    public bool CheckIfSavePointIsActivated()
    {
        for(int i = 0; i<savingPoints.Count;i++)
        {
            if(savingPoints[i].isActivated)
            {
                return true;
            }
        }
        return false;
    }
    
    public void CheckIfPlayerIsOnRestorePoint()
    {
        for(int i = 0; i < restorePoints.Count; i++)
        {
            if(restorePoints[i].hasEncounteredAPlayer && !restorePoints[i].hasBeenActivated)
            {
                for(int j = 0; j < PlayerController.instance.partyStats.Length; j++)
                {
                    if(PlayerController.instance.partyStats[j].gameObject.activeInHierarchy)
                    {
                        PlayerController.instance.partyStats[j].restoreHP();
                        PlayerController.instance.partyStats[j].restoreMP();
                    }
                }
                restorePoints[i].ActivatePoint();
                eventManager.ActivateEvent();
                eventManager.ShowEventPanel();
                eventManager.eventPanel.ShowPanel(eventManager.eventPanel.restoreEventPanel);
            }
        }
    }
    public void PortalTransition()
    {
        UIFade.instance.FadeToBlack();
        AudioManager.instance.StopAmbience();
        AudioManager.instance.StopPlatformBGM();
        AudioManager.instance.ChangeAudioClip(AudioManager.instance.PlatformSFX[4], portalSFX);
        if (!AudioManager.instance.PlatformSFX[4].isPlaying) AudioManager.instance.PlatformSFX[4].Play();
        StartCoroutine(levelManager.NextLevelCo());
        bossIsDead=false;
        PlayerController.instance.anim.SetFloat("speedX", 0f);
        PlayerController.instance.rb.velocity = Vector2.zero;
        PlayerController.instance.canMove = false;
    }

    public void DestroyAllInstances()
    {
        if (PlayerController.instance != null) Destroy(PlayerController.instance.gameObject);
        if (BattleManager.instance != null) Destroy(BattleManager.instance.gameObject);
        if (CameraController.instance != null) Destroy(CameraController.instance.gameObject);
        if (UIFade.instance != null) Destroy(UIFade.instance.gameObject);
        if (AudioManager.instance != null) Destroy(AudioManager.instance.gameObject);
        if (instance != null) Destroy(gameObject);
    }

    public void RespawnPlayerOnLastCheckPoint()
    {
        PlayerController.instance.SetPositionAtRespawn(lastCheckPoint);
    }

    public Item GetItemDetails(string ItemToGrab)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == ItemToGrab) return referenceItems[i];
        }
        return null;
    }

    public void CheckActiveCratesOnLevel()
    {
        for (int i = 0; i < levelCrates.Count; i++)
        {
            if(levelCrates[i].hasBeenOpened && levelCrates[i].isActive)
            {
                messageToPanel = levelCrates[i].CrateMessage(messageToPanel);
                eventManager.ActivateEvent();
                eventManager.eventPanel.ChangeTextOnPanel(eventManager.eventPanel.itemMessageText, messageToPanel);
                eventManager.ShowEventPanel();
                eventManager.eventPanel.ShowPanel(eventManager.eventPanel.itemMessagePanel);
                levelCrates[i].DeactivateCrate();
                break;
            }
        }
    }

    public IEnumerator startBattleCo(int i)
    {
        battleHasEnded=false;
        hasBeenAnLevelUP = false;
        for(int j = 0; j<levelUPChars.Length;j++)
        {
            levelUPChars[j] = false;
        }
        eventManager.eventPanel.HideAllLevelUpCharPanels();
        if (!AudioManager.instance.PlatformSFX[4].isPlaying) PlayBattleTransitionSFX();
        AudioManager.instance.PlatformBGM.Pause();
        AudioManager.instance.Ambience.Pause();
        for(int j = 0; j < enemiesOnScreen.Length; j++)
        {
            enemiesOnScreen[j].canAttack = false;
        }
        if (enemiesOnScreen[i].isBoss)
        {
            if (!AudioManager.instance.BattleBGM.isPlaying)
            {
                AudioManager.instance.BattleBGM = AudioManager.instance.ChangeAudioClip(AudioManager.instance.BattleBGM, AudioManager.instance.bossBattleBGM);
                AudioManager.instance.PlayBattleBGM();
            }
            BattleManager.instance.chanceToFlee = 9999;
        }

        else
        {
            if (!AudioManager.instance.BattleBGM.isPlaying)
            {
                AudioManager.instance.BattleBGM = AudioManager.instance.ChangeAudioClip(AudioManager.instance.BattleBGM, AudioManager.instance.normalBattleBGM);
                AudioManager.instance.PlayBattleBGM();
            }
            BattleManager.instance.chanceToFlee = 50;
        }

        if (PlayerController.instance.transform.parent != null)
        {
            if (PlayerController.instance.transform.parent.gameObject.tag == "MovingPlatform")
            {
                if (PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementHorizontal)
                {
                    PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementHorizontal = false;
                    platformMovementHorizontal = true;
                }
                if (PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementVertical)
                {
                    PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementVertical = false;
                    platformMovementVertical = true;
                }
            }
        }
        yield return new WaitForSeconds(5f);
        InstantiateEnemiesOnBattle(i);
    }

    public void InstantiateEnemiesOnBattle(int i)
    {
        int xp = 0;
        for (int j = 0; j < enemiesOnScreen[i].enemyParty.Length; j++)
        {
            for (int k = 0; k < BattleManager.instance.enemyPrefabs.Length; k++)
            {
                if (enemiesOnScreen[i].enemyParty[j] == BattleManager.instance.enemyPrefabs[k].chara.charName) xp += BattleManager.instance.enemyPrefabs[k].GetComponent<EnemyStats>().xpToGive;
            }
        }
        enemiesOnScreen[i].gameObject.SetActive(false);
        UIFade.instance.FadeFromBlack();
        BattleManager.instance.BattleStart(enemiesOnScreen[i].enemyParty, xp);
    }

    public void endBattle(int i)
    {
        allPlayersAreDead = true;
        for (int j = 0; j < PlayerController.instance.partyStats.Length; j++)
        {
            if (PlayerController.instance.partyStats[j].GetCharStats().GetCurrentHP() > 0 && PlayerController.instance.partyStats[j].gameObject.activeInHierarchy)
            {
                allPlayersAreDead = false;
                break;
            }
        }
        if (allPlayersAreDead)
        {
            BattleManager.instance.endInFailure = true;
            BattleManager.instance.EndBattle();
        }

        else
        {
            if (enemiesOnScreen[i].isBoss) bossIsDead = true;
            enemiesOnScreen[i].gameObject.SetActive(false);
            BattleManager.instance.EndBattle();
        }
    }

    public void returnToPlatform()
    {
        StartCoroutine(BattleManager.instance.EndBattle2Co());
        UIFade.instance.Invoke("FadeFromBlack", 5f);
        AudioManager.instance.Invoke("StopBattleBGM", 2f);
        if (!AudioManager.instance.PlatformBGM.isPlaying) AudioManager.instance.Invoke("PlayPlatformBGM",2f);
        if (!AudioManager.instance.Ambience.isPlaying) AudioManager.instance.Invoke("PlayAmbience",2f);
        Invoke("PlatformRestart", 4.5f);
    }

    public void PlatformRestart()
    {

        if (platformMovementHorizontal)
        {
            platformMovementHorizontal = false;
            PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementHorizontal = true;
        }
        if (platformMovementVertical)
        {
            platformMovementVertical = false;
            PlayerController.instance.transform.parent.GetComponent<MovingPlatform>().movementVertical = true;
        }
        for (int j = 0; j < enemiesOnScreen.Length; j++)
        {
            enemiesOnScreen[j].canAttack = true;
        }
        PlayerController.instance.canMove = true;
        PlayerController.instance.gotDamage = false;
        PlayerController.instance.encounteredAnEnemy = false;
        if(hasBeenAnLevelUP)
        {
            eventManager.ActivateEvent();
            eventManager.ShowEventPanel();
            for(int j = 0; j<levelUPChars.Length;j++)
            {
                if(levelUPChars[j])
                {
                    eventManager.eventPanel.ShowLevelUpCharPanel(j);
                    PlayerController.instance.partyStats[j].restoreHP();
                    PlayerController.instance.partyStats[j].restoreMP();
                }
            }
            eventManager.eventPanel.ShowPanel(eventManager.eventPanel.levelUpPanel);
        }
    }

    public void RespawnEnemyOnFlee(int enemy)
    {
        enemiesOnScreen[enemy].gameObject.SetActive(true);
    }

    public void MainMenu()
    {   
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private bool battleActive;
    public bool battleHasEnded;
    public GameObject battleScene;
    public GameObject playerPanel;
    public GameObject currentPanel, previousPanel;
    public GameObject uiButtonsholder;
    public GameObject playerStatsPanel;
    public GameObject enemyAttackEffect;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;
    public List<Transform> enemyTargets = new List<Transform>();
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    public Image[] charStats, hpBarFillers, mpBarFillers;
    public List<BattleChar> activeBattlers = new List<BattleChar>();
    public SelectOption targetSelector, cypherSelector, itemSelector;
    public int currentTurn;
    public int chanceToFlee;
    public int pointsInCasePlayerGoesBack;
    public bool turnWaiting;
    public bool playerAdvantage, enemyAdvantage;
    public bool playerHasSelectedAnOption;
    public BattleMove[] movesList;
    public List<Item> battleItems = new List<Item>();
    public DamageNumber damageNumber;
    public Text[] playerName, playerHP, playerMP;
    public GameObject warningWindow;
    public GameObject targetMenu;
    public GameObject cypherMenu;
    public GameObject itemMenu;
    public BattleCypherSelect[] cypherButtons;
    public BattleTargetButton[] targetButtons;
    public BattleItemSelect[] itemButtons;

    // Start is called before the first frame update
    void Start()
    { 

        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        currentPanel = uiButtonsholder;
        previousPanel = currentPanel;

    }

    // Update is called once per frame
    void Update()
    {
        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    playerPanel.SetActive(true);
                    if (currentPanel == cypherMenu)
                    {
                        playerStatsPanel.SetActive(false);
                    }
                    else
                    {
                        playerStatsPanel.SetActive(true);
                    }
                    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentPanel.SetActive(false);
                        previousPanel.SetActive(true);
                        if (previousPanel == cypherMenu)
                        {
                            activeBattlers[currentTurn].currentMP += pointsInCasePlayerGoesBack;
                        }
                        currentPanel = previousPanel;
                        if (currentPanel == cypherMenu || currentPanel == itemMenu)
                        {
                            previousPanel = uiButtonsholder;
                        }

                        if(currentPanel == uiButtonsholder)
                        {
                            playerHasSelectedAnOption = false;
                        }

                        
                    }
                    if (playerHasSelectedAnOption)
                    {
                        uiButtonsholder.SetActive(false);
                    }
                    else
                    {
                        uiButtonsholder.SetActive(true);
                    }
                }

                else
                {
                    playerPanel.SetActive(false);
                    StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!battleActive)
        {
            battleActive = true;
            battleHasEnded = false;
            GameManager.instance.battleActive = true;
            battleScene.SetActive(true);
            for(int i = 0; i < charStats.Length; i++)
            {
                charStats[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == PlayerController.instance.partyStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = PlayerController.instance.partyStats[i];
                            activeBattlers[i].currentHp = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].dexterity = thePlayer.dexterity;
                            activeBattlers[i].vitality = thePlayer.vitality;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].element = thePlayer.element;
                            activeBattlers[i].spirit = thePlayer.spirit;
                            activeBattlers[i].wpnPower = thePlayer.wpnPwr;
                            activeBattlers[i].armrPower = thePlayer.armrPwr;
                            charStats[i].gameObject.SetActive(true);
                        }
                    }

                }

            }

           for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].transform.position, enemyPositions[i].transform.rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                            targetSelector.positions.Add(enemyTargets[i].transform);
                        }
                    }
                }
            }

            for (int i = 0; i < GameManager.instance.referenceItems.Length; i++)
            {
                if (GameManager.instance.referenceItems[i].isItem)
                {
                    battleItems.Add(GameManager.instance.referenceItems[i]);
                }
            }

            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
            warningWindow.SetActive(true);
            warningWindow.GetComponentInChildren<Text>().text = "Battle Start!";
        }
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        StartCoroutine(FirstTurnCo());
    }

    public IEnumerator FirstTurnCo()
    {
        playerPanel.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        UpdateUIStats();
        targetMenu.SetActive(false);
        if (activeBattlers[currentTurn].isPlayer)
        {
            warningWindow.GetComponentInChildren<Text>().text = "Is " + activeBattlers[currentTurn].charName + "'s turn";
        }
        else
        {
            warningWindow.GetComponentInChildren<Text>().text = "Is Enemy's Turn";
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if (activeBattlers[i].currentHp == 0)
            {
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }

                else
                {
                    activeBattlers[i].EnemyFade();
                    /*for (int j = 0; j < enemyPositions.Length; j++)
                    {
                        if (enemyPositions[j] == activeBattlers[i].transform.parent)
                        {
                            targetSelector.positions.RemoveAt(j);
                        }
                    }*/
                }

            }

            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }

                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                battleHasEnded = true;
                //StartCoroutine(EndBattleCo());
                //end battle in victory
            }

            else
            {
                battleHasEnded = true;
                //end battle in failure
            }
        }

        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }

            UpdateUIStats();
            warningWindow.SetActive(true);
            if (activeBattlers[currentTurn].isPlayer)
            {
                warningWindow.GetComponentInChildren<Text>().text = "Is " + activeBattlers[currentTurn].charName + "'s turn";
            }
            else
            {
                warningWindow.GetComponentInChildren<Text>().text = "Is Enemy's Turn";
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        string moveType = "";
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                AttackEffect attackEffect = Instantiate(movesList[i].myEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                attackEffect.GetComponent<SpriteRenderer>().flipX = true;
                movePower = movesList[i].movePower;
                moveType = movesList[i].moveType;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower, moveType);
    }

    public void DealDamage(int target, int movePower, string damageType)
    {
        float atkPwr = 0;
        float defPwr = 0;
        if (damageType == "Attack")
        {
            atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
            defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;
        }
        
        if(damageType == "Cypher")
        {
            atkPwr = activeBattlers[currentTurn].element + activeBattlers[currentTurn].wpnPower;
            defPwr = activeBattlers[target].spirit + activeBattlers[target].armrPower;
        }
        float critProbability = Random.Range(1, 101);
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        if (critProbability >= 75)
        {
            damageCalc *= Random.Range(2f, 2.5f);
        }
        int damageToGive = Mathf.RoundToInt(damageCalc);

        activeBattlers[target].currentHp -= damageToGive;

        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        if (critProbability >= 75)
        {
            GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = Color.yellow;
        }

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    previousPanel = currentPanel;
                    currentPanel = uiButtonsholder;
                    BattleChar playerData = activeBattlers[i];
                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + " / " + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + " / " + playerData.maxMP;
                    hpBarFillers[i].fillAmount = (((float)playerData.currentHp * 1) / (float)playerData.maxHP);
                    mpBarFillers[i].fillAmount = (((float)playerData.currentMP * 1) / (float)playerData.maxMP);
                }

                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        warningWindow.SetActive(false);
        int movePower = 0;
        string moveType = "";
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                AttackEffect attackEffect = Instantiate(movesList[i].myEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                attackEffect.GetComponent<SpriteRenderer>().flipX = true;
                movePower = movesList[i].movePower;
                moveType = movesList[i].moveType;
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower, moveType);
        targetMenu.SetActive(false);
        NextTurn();
    }
    public void OpenTargetMenu(string moveName)
    {
        previousPanel = currentPanel;
        currentPanel = targetMenu;
        targetMenu.SetActive(true);
        playerHasSelectedAnOption = true;
        List<int> enemies = new List<int>();
        targetSelector.positions = new List<Transform>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (enemies.Count > i && activeBattlers[enemies[i]].currentHp>0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = enemies[i];
                targetButtons[i].targetName.text = activeBattlers[enemies[i]].charName;
                targetSelector.positions.Add(targetButtons[i].transform);
            }

            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void openCypherMenu()
    {
        previousPanel = currentPanel;
        currentPanel = cypherMenu;
        if (activeBattlers[currentTurn].movesAvailable.Length == 0)
        {
            warningWindow.SetActive(true);
            warningWindow.transform.GetChild(0).GetComponent<Text>().text = "Not Cyphers Available";
        }

        else
        {
            warningWindow.SetActive(false);
            playerStatsPanel.SetActive(false);
            cypherMenu.SetActive(true);
            playerHasSelectedAnOption = true;
            for (int i = 0; i < cypherButtons.Length; i++)
            {
                if (activeBattlers[currentTurn].movesAvailable.Length > i)
                {
                    cypherButtons[i].gameObject.SetActive(true);
                    cypherButtons[i].cypherName = activeBattlers[currentTurn].movesAvailable[i];
                    cypherButtons[i].nameText.text = cypherButtons[i].cypherName;
                    for (int j = 0; j < movesList.Length; j++)
                    {
                        if (movesList[j].moveName == cypherButtons[i].cypherName)
                        {
                            cypherButtons[i].cypherCost = movesList[j].moveCost;
                            cypherButtons[i].costText.text = cypherButtons[i].cypherCost.ToString();
                            cypherSelector.positions.Add(cypherButtons[i].transform);
                        }
                    }
                }
                else
                {
                    cypherButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void OpenItemMenu()
    {
        previousPanel = currentPanel;
        currentPanel = itemMenu;
        warningWindow.SetActive(false);
        playerStatsPanel.SetActive(false);
        itemMenu.SetActive(true);
        playerHasSelectedAnOption = true;
        int[] itemQuantities = GameManager.instance.numberOfItems;
        for (int i = 0; i < itemButtons.Length; i++)
            {
                if (i<battleItems.Count)
                {
                    itemButtons[i].gameObject.SetActive(true);
                    itemButtons[i].itemName = battleItems[i].itemName;
                    itemButtons[i].NameText.text = battleItems[i].itemName;
                    for (int j = 0; j < itemQuantities.Length;j++)
                    {
                        if (i == j)
                        {
                            itemButtons[i].itemQuantity = itemQuantities[j];
                            itemButtons[i].QuantityText.text = itemButtons[i].itemQuantity.ToString();
                            itemSelector.positions.Add(itemButtons[i].transform);
                        }
                    }
                }
                else
                {
                    itemButtons[i].gameObject.SetActive(false);
                }
            }
    }

    public void featNotImplemented()
    {
        warningWindow.transform.GetChild(0).GetComponent<Text>().text = "Not implemented yet";
        warningWindow.SetActive(true);
    }

    public void Flee()
    {
        int fleeSuccess = Random.Range(0, 100);
        if (fleeSuccess < chanceToFlee)
        {
            StartCoroutine(EndBattleCo());
        }

        else
        {
            NextTurn();
            warningWindow.transform.GetChild(0).GetComponent<Text>().text = "Can't Escape!";
            warningWindow.SetActive(true);
        }
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        playerPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        warningWindow.SetActive(false);
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).transform.Find("Canvas").gameObject.SetActive(false);
    }

    public IEnumerator EndBattle2Co()
    {
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < PlayerController.instance.partyStats.Length; j++)
                {
                    if (activeBattlers[i].charName == PlayerController.instance.partyStats[j].charName)
                    {
                        PlayerController.instance.partyStats[j].currentHP = activeBattlers[i].currentHp;
                        PlayerController.instance.partyStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }
            Destroy(activeBattlers[i].gameObject);
        }
        yield return new WaitForSeconds(1f);
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        yield return new WaitForSeconds(1f);
        UIFade.instance.FadeFromBlack();
        GameManager.instance.battleActive = false;
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndProto");
    }
}

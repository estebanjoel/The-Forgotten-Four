using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameEnums;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public GameObject battleScene;
    public BattleDamageCalculator damageCalculator;
    public BattleXPManager xpManager;
    public BattleUIManager battleUI;
    public BattleIndicators indicators;
    public BattleItems battleItems;
    public BattleAudioManager audioManager;
    public bool battleActive;
    private bool hasSorted, canCheckPanels, firstTurnStarted, battleHasStarted;
    public bool endInVictory, endInFlee, endInFailure;
    public bool hasClickedFleeButton;
    public bool playerFirst, enemyFirst;
    public bool battleHasEnded;
    public GameObject enemyAttackEffect;
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;
    public List<BattleChar> activeBattlers = new List<BattleChar>();
    public int currentTurn;
    public int chanceToFlee;
    public int pointsInCasePlayerGoesBack;
    public bool turnWaiting;
    public BattleMove[] movesList;
    public DamageNumber damageNumber;
    AudioClip moveClip;

    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion
        hasSorted = false;
        audioManager.BattleEffectSource = AudioManager.instance.BattleSFX[1];
        audioManager.BattleUISource = AudioManager.instance.BattleSFX[0];

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
                    if (canCheckPanels) battleUI.CheckActiveBattlePanel(instance);
                }

                else
                {
                    battleUI.HidePanel(battleUI.playerPanel);
                    StartCoroutine(EnemyMoveCo());
                }
            }
        }
    }

    public void setBattleBG(Sprite battleSprite)
    {
        battleScene.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = battleSprite;
    }

    public static int sortByInitiative(BattleChar p1, BattleChar p2)
    {
        return (p2.initiative.CompareTo(p1.initiative));
    }

    public void PlayerAdvantage()
    {
        playerFirst = true;
        enemyFirst = false;
    }

    public void EnemyAdvantage()
    {
        enemyFirst = true;
        playerFirst = false;
    }

    public List<BattleChar> SpecialSort(List<BattleChar> battlers)
    {
        List<BattleChar> newOrder = new List<BattleChar>();
        List<BattleChar> playerOrder = new List<BattleChar>();
        List<BattleChar> enemyOrder = new List<BattleChar>();
        
        for (int i = 0; i < battlers.Count; i++)
        {
            if (battlers[i].isPlayer)
            {
                playerOrder.Add(battlers[i]);
            }
            else
            {
                enemyOrder.Add(battlers[i]);
            }
        }

        playerOrder.Sort(sortByInitiative);
        enemyOrder.Sort(sortByInitiative);

        if (playerFirst)
        {
            for (int i = 0; i < playerOrder.Count; i++)
            {
                newOrder.Add(playerOrder[i]);
            }
            for (int i = 0; i < enemyOrder.Count; i++)
            {
                newOrder.Add(enemyOrder[i]);
            }
        }

        if (enemyFirst)
        {
            for (int i = 0; i < enemyOrder.Count; i++)
            {
                newOrder.Add(enemyOrder[i]);
            }
            for (int i = 0; i < playerOrder.Count; i++)
            {
                newOrder.Add(playerOrder[i]);
            }
        }
        battlers = newOrder;
        return battlers;
    }

    public void FirstDamage(bool toPlayer, bool toEnemy)
    {
        if (toEnemy)
        {
            for(int i = 0; i < activeBattlers.Count; i++)
            {
                if (!activeBattlers[i].isPlayer) damageCalculator.DealDamage(i, movesList[0].movePower, moveType.Attack, elementType.NonElemental, moveModifier.Physical);
            }
        }

        if (toPlayer)
        {
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].isPlayer) damageCalculator.DealDamage(i, movesList[0].movePower, moveType.Attack, elementType.NonElemental, moveModifier.Physical);
            }
        }
    }

    public void SetPlayerStats(int player)
    {
        CharStats thePlayer = PlayerController.instance.partyStats[player];
        activeBattlers[player].SetCharacterStats(thePlayer);
        battleUI.charStats[player].gameObject.SetActive(true);
    }

    public void BattleStart(string[] enemiesToSpawn, int exp)
    {
        if (!battleActive)
        {
            if(!battleHasStarted)
            {
                battleHasStarted = true;
                battleHasEnded = false;
                endInFailure = false;
                endInFlee = false;
                endInVictory = false;
                transform.GetChild(0).transform.Find("Canvas").gameObject.SetActive(true);
                battleScene.SetActive(true);
                battleUI.HidePanel(battleUI.playerPanel);
                battleUI.HidePanel(battleUI.playerButtonsHolder);
                for (int i = 0; i < battleUI.charStats.Length; i++)
                {
                    battleUI.charStats[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < playerPositions.Length; i++)
                {
                    if (PlayerController.instance.partyStats[i].gameObject.activeInHierarchy)
                    {
                        for (int j = 0; j < playerPrefabs.Length; j++)
                        {
                            if (playerPrefabs[j].charName == PlayerController.instance.partyStats[i].charName)
                            {
                                playerPositions[i].gameObject.SetActive(true);
                                BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                                newPlayer.transform.parent = playerPositions[i];
                                activeBattlers.Add(newPlayer);
                                SetPlayerStats(i);
                            }
                        }
                    }
                    else
                    {
                        playerPositions[i].gameObject.SetActive(false);
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
                            }
                        }
                    }
                }

                battleItems.SetItems(GameManager.instance.referenceItems);

                turnWaiting = true;
                currentTurn = 0;
                
                battleUI.ChangeTopPanelText("Battle Start!");
                battleUI.ShowPanel(battleUI.topPanel);
                battleUI.HidePanel(battleUI.targetMenu);
                xpManager.SetXPToGive(exp);
                transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
                StartCoroutine(FirstTurnCo());
            }
        }
    }

    public IEnumerator FirstTurnCo()
    {
        GameManager.instance.battleActive = true;
        int currentPosition = currentTurn;
        while (!indicators.indicatorPositions[currentPosition].gameObject.activeInHierarchy)
        {
            currentPosition++;
        }

        indicators.InstantiateIndicator(currentPosition);

        yield return new WaitForSeconds(0.5f);
        if (!hasSorted)
        {
            if (!playerFirst && !enemyFirst)
            {
                activeBattlers.Sort(sortByInitiative);
                hasSorted = true;
            }
            else
            {
                activeBattlers=SpecialSort(activeBattlers);
                FirstDamage(enemyFirst, playerFirst);
                hasSorted = true;
                enemyFirst = false;
                playerFirst = false;
            }
        }
        if (!firstTurnStarted)
        {
            yield return new WaitForSeconds(1.5f);
            if (activeBattlers[currentTurn].isPlayer)
            {
                battleUI.ChangeTopPanelText("Is " + activeBattlers[currentTurn].charName + "'s turn");
            }
            else
            {
                battleUI.ChangeTopPanelText("Is Enemy's Turn");
            }
            if (currentTurn == 0)
            {
                currentTurn = activeBattlers.Count - 1;
            }
            else
            {
                currentTurn--;
            }
            yield return new WaitForSeconds(3f);
            NextTurn();
            firstTurnStarted = true;
        }

        battleActive = true;
    }

    public void NextTurn()
    {
        hasClickedFleeButton = false;
        indicators.ClearTurnIndicators();
        currentTurn++;
        if (currentTurn >= activeBattlers.Count) currentTurn = 0;
        turnWaiting = true;
        if(activeBattlers[currentTurn].hasAnAlteratedStat)
        {
            if(activeBattlers[currentTurn].alteratedStatTimer>0) activeBattlers[currentTurn].alteratedStatTimer--;
            else
            {
                bool hasInitiativeChanged = false;
                for(int i = 0; i<activeBattlers[currentTurn].previousAlteratedStatsName.Count; i++)
                {
                    if(activeBattlers[currentTurn].previousAlteratedStatsName[i] == statTarget.Initiative)
                    {
                        hasInitiativeChanged = true;
                        break;
                    }
                }
                RestorePreviousStatsValues(currentTurn);
                if(hasInitiativeChanged) SortBattlers();
            }
        }
        UpdateBattle();
    }

    public void SortBattlers()
    {
        BattleChar currentChar = activeBattlers[currentTurn];
        activeBattlers.Sort(sortByInitiative);
        for(int i = 0; i<activeBattlers.Count;i++)
        {
            if(activeBattlers[i]==currentChar)
            {
                currentTurn = i;
                break;
            }
        }
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {       
            if (activeBattlers[i].currentHp < 0) activeBattlers[i].currentHp = 0;

            if (activeBattlers[i].currentHp == 0)
            {
                if (activeBattlers[i].isPlayer)
                {
                    if(!activeBattlers[i].IsCharDeathAnimationActive()) activeBattlers[i].SetAnimatorTrigger("deathTrigger");
                } 

                else
                {
                    if(CheckIfCurrentBattlerIsBoss(i)) activeBattlers[i].GetComponent<BattleEnemyIA>().SetBossAnimationTrigger("deathTrigger");
                    else activeBattlers[i].GetComponent<EnemyStats>().EnemyFade();
                } 
            }

            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    if(activeBattlers[i].IsCharDeathAnimationActive()) activeBattlers[i].SetAnimatorTrigger("reviveTrigger");
                    allPlayersDead = false;
                    canCheckPanels = true;
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
                endInVictory = true;
                endInFailure = false;
                endInFlee = false;
                battleHasEnded = true;
            }

            else
            {
                endInFailure = true;
                endInVictory = false;
                endInFlee = false;
                battleHasEnded = true;
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

            battleUI.UpdateUIStats(instance);
            battleUI.ShowPanel(battleUI.topPanel);
            if (activeBattlers[currentTurn].isPlayer)
            {
                battleUI.ChangeTopPanelText("Is " + activeBattlers[currentTurn].charName + "'s turn");
            }
            else
            {
                battleUI.ChangeTopPanelText("Is Enemy's Turn");
            }
            indicators.SetTurnIndicator(activeBattlers, currentTurn);
            CheckSpecials();
        }
    }

    public void CheckSpecials()
    {
        if(activeBattlers[currentTurn].GetComponent<YakovChar>() != null)
        {
            if(activeBattlers[currentTurn].GetComponent<YakovChar>().CheckIfIsDrunk())
            {
                StartCoroutine(YakovDrunkTurnCo());
                turnWaiting = false;
                Invoke("NextTurn", 3f);
            }
        }
    }

    
    public IEnumerator YakovDrunkTurnCo()
    {
        battleUI.ShowPanel(battleUI.topPanel);
        battleUI.HidePanel(battleUI.playerPanel);
        battleUI.ChangeTopPanelText("Yakov is Drunk!");
        int randomAction = Random.Range(1,101);
        yield return new WaitForSeconds(1f);
        if(randomAction<=10)
        {
            for(int i = 0; i < movesList.Length; i++)
            {
                if(movesList[i].moveName == "Psionic Shot")
                {
                    for(int j = 0; j < activeBattlers.Count; j++)
                    {
                        if(activeBattlers[j].isPlayer && activeBattlers[j].currentHp > 0)
                        {
                            StartCoroutine(PlayerAttackCo(movesList[i].moveName, j));
                        }
                    }
                    break;
                }
            }
        }
        else if(randomAction>10 && randomAction>=50)
        {
            int randomTarget = Random.Range(0, activeBattlers.Count);
            while(activeBattlers[randomTarget].isPlayer || activeBattlers[randomTarget].currentHp<=0)
            {
                randomTarget = Random.Range(0, activeBattlers.Count);
            }
            for(int i = 0; i < movesList.Length; i++)
            {
                if(movesList[i].moveName == "Psionic Shot")
                {
                    StartCoroutine(PlayerAttackCo(movesList[i].moveName, randomTarget));
                    break;
                }
            }
        }
        else if(randomAction>50 && randomAction>=90)
        {
            int randomTarget = Random.Range(0, activeBattlers.Count);
            while(!activeBattlers[randomTarget].isPlayer || activeBattlers[randomTarget].currentHp<=0)
            {
                randomTarget = Random.Range(0, activeBattlers.Count);
            }
            for(int i = 0; i < movesList.Length; i++)
            {
                if(movesList[i].moveName == "Cure")
                {
                    BattleMove cure = movesList[i];
                    CypherToPlayer(cure.moveName, randomTarget);
                    break;
                }
            }
        }
        else if(randomAction>90 && randomAction>=99)
        {
            for(int i = 0; i < movesList.Length; i++)
            {
                if(movesList[i].moveName == "Psionic Shot")
                {
                    for(int j = 0; j < activeBattlers.Count; j++)
                    {
                        if(!activeBattlers[j].isPlayer && activeBattlers[j].currentHp > 0)
                        {
                            StartCoroutine(PlayerAttackCo(movesList[i].moveName, j));
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            for(int i = 0; i < movesList.Length; i++)
            {
                if(movesList[i].moveName == "Cure")
                {
                    BattleMove cure = movesList[i];
                    for(int j = 0; j < activeBattlers.Count; j++)
                    {
                        if(activeBattlers[j].isPlayer && activeBattlers[j].currentHp > 0)
                        {
                            CypherToPlayer(cure.moveName, j);
                        }
                    }
                    break;
                }
            }
        }

        yield return new WaitForSeconds(1f);
    }
    
    public bool CheckIfCurrentBattlerIsBoss(int battler)
    {
        if(!activeBattlers[battler].isPlayer)
        {
            if(activeBattlers[battler].GetComponent<BattleEnemyIA>().isBoss) return true;
            else return false;
        }
        return false;
    } 
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        BattleEnemyIA myIA = activeBattlers[currentTurn].GetComponent<BattleEnemyIA>();
        if(myIA.isHealer)
        {
            int target = -1;
            target = myIA.CheckForEnemiesWounded(target);
            if(target>-1)
            {
                yield return new WaitForSeconds(1f);
                for(int i = 0; i<activeBattlers[currentTurn].movesAvailable.Count;i++)
                {
                    for(int j = 0; j<movesList.Length;j++)
                    {
                        if(activeBattlers[currentTurn].movesAvailable[i] == movesList[j].moveName)
                        {
                            if(movesList[j].eType==elementType.Heal) StartCoroutine(EnemyHealCo(target, currentTurn, movesList[j]));
                        }
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(EnemyAttackCo());
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyAttackCo());
        }
        
        yield return new WaitForSeconds(3.5f);
        NextTurn();
    }

    public bool AttackProbability(BattleChar attacker, BattleChar target)
    {
        int attackerProbability = Random.Range(1, 21);
        if (attackerProbability == 20) return true;
        int targetProbability = Random.Range(1, 21);
        if (targetProbability == 20) return false;
        float attackResult = (attacker.dexterity + attacker.accuracy + attackerProbability) - (target.dexterity + target.evasion + targetProbability/2);
        if (attackResult > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public IEnumerator EnemyAttackCo()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0) players.Add(i);
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int movePower;
        moveType movType;
        elementType element;
        moveModifier movModifier;
        bool attackProbability = false;
        bool hasSelectedAnAttack = false;
        int selectedAttack = 0;
        while(!hasSelectedAnAttack)
        {
            int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Count);
            for (int i = 0; i < movesList.Length; i++)
            {
                if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
                {
                    if(movesList[i].eType != elementType.Heal && movesList[i].eType != elementType.Support)
                    {
                        hasSelectedAnAttack = true;
                        if(CheckIfCurrentBattlerIsBoss(currentTurn))
                        {
                            BattleEnemyIA boss = activeBattlers[currentTurn].GetComponent<BattleEnemyIA>();
                            if(movesList[i].eType == elementType.Fire || movesList[i].eType == elementType.Electric) boss.SetBossAnimationTrigger("cypherTrigger");
                            else boss.SetBossAnimationTrigger("attackTrigger");
                        }
                        selectedAttack = i;
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);
        AttackEffect attackEffect = Instantiate(movesList[selectedAttack].myEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
        attackEffect.GetComponent<SpriteRenderer>().flipX = true;
        movePower = movesList[selectedAttack].movePower;
        movType = movesList[selectedAttack].mType;

        if (movType == moveType.Attack)
        {
            attackProbability = AttackProbability(activeBattlers[currentTurn], activeBattlers[selectedTarget]);
        }
        else
        {
            attackProbability = true;
        }

        moveClip = movesList[selectedAttack].myAudioClip;
        element = movesList[selectedAttack].eType;
        movModifier = movesList[selectedAttack].mModifier;
        audioManager.PlayMoveAudio(moveClip);
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        if (attackProbability)
        {
            yield return new WaitForSeconds(1f);
            damageCalculator.DealDamage(selectedTarget, movePower, movType, element, movModifier);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            Instantiate(damageNumber, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation).MissDamage();
        }

    }

    public IEnumerator EnemyHealCo(int target, int caster, BattleMove healMove)
    {
        Instantiate(enemyAttackEffect, activeBattlers[caster].transform.position, activeBattlers[caster].transform.rotation);
        yield return new WaitForSeconds(1f);
        int modifier = Mathf.FloorToInt((healMove.movePower + activeBattlers[caster].spirit + activeBattlers[caster].mgcPower) * Random.Range(0.9f,1.5f));
        moveClip = healMove.myAudioClip;
        yield return new WaitForSeconds(.5f);
        AttackEffect attackEffect = Instantiate(healMove.myEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        attackEffect.GetComponent<SpriteRenderer>().flipX = true;
        audioManager.PlayMoveAudio(moveClip);
        yield return new WaitForSeconds(1f);
        Instantiate(BattleManager.instance.damageNumber, BattleManager.instance.activeBattlers[target].transform.position, BattleManager.instance.activeBattlers[target].transform.rotation).SetDamage(modifier);
        GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = Color.green;
        activeBattlers[target].currentHp+=modifier;
        if(activeBattlers[target].currentHp > activeBattlers[target].maxHP) activeBattlers[target].currentHp = activeBattlers[target].maxHP;
    }
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        battleUI.HidePanel(battleUI.topPanel);
        StartCoroutine(PlayerAttackCo(moveName, selectedTarget));
    }

    public IEnumerator PlayerAttackCo(string moveName, int selectedTarget)
    {
        int movePower;
        moveType m;
        elementType e;
        moveModifier mM;
        bool attackProbability = false;
        battleUI.HidePanel(battleUI.targetMenu);
        battleUI.HidePanel(battleUI.playerPanel);
        canCheckPanels = false;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                movePower = movesList[i].movePower;
                m = movesList[i].mType;
                e = movesList[i].eType;
                mM = movesList[i].mModifier;
                if(m==moveType.Attack) activeBattlers[currentTurn].SetAnimatorTrigger("attackTrigger");
                if(m==moveType.Cypher) activeBattlers[currentTurn].SetAnimatorTrigger("cypherTrigger");
                yield return new WaitForSeconds(1f);
                AttackEffect attackEffect = Instantiate(movesList[i].myEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                attackEffect.GetComponent<SpriteRenderer>().flipX = true;
                if (m == moveType.Attack)
                {
                    attackProbability = AttackProbability(activeBattlers[currentTurn], activeBattlers[selectedTarget]);
                }
                else
                {
                    attackProbability = true;
                }

                moveClip = movesList[i].myAudioClip;
                audioManager.PlayMoveAudio(moveClip);
                Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                if (attackProbability)
                {
                    yield return new WaitForSeconds(1f);
                    damageCalculator.DealDamage(selectedTarget, movePower, m, e, mM);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    Instantiate(damageNumber, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation).MissDamage();
                }
                break;
            }
        }
    }
    
    public void CypherToPlayer(string moveName, int selectedTarget)
    {
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                moveClip = movesList[i].myAudioClip;
                audioManager.PlayMoveAudio(moveClip);
                if (movesList[i].eType == elementType.Heal) HealCypher(i, selectedTarget, currentTurn);
                if (movesList[i].eType == elementType.Support) SupportCypher(i, selectedTarget, currentTurn);
                if (movesList[i].eType == elementType.None && moveName == "Vodka Madness") activeBattlers[selectedTarget].GetComponent<YakovChar>().VodkaMadness();
                break;
            }
        }
        
        activeBattlers[currentTurn].SetAnimatorTrigger("cypherTrigger");
        battleUI.HidePanel(battleUI.targetMenu);
        battleUI.HidePanel(battleUI.playerPanel);
        canCheckPanels = false;
    }

    public void HealCypher(int move, int selectedCharacter, int caster)
    {
        int amountToHeal = Mathf.FloorToInt(((activeBattlers[caster].spirit * Random.Range(0.9f, 1.5f) + movesList[move].movePower) * Random.Range(0.9f, 1.25f)));
        activeBattlers[selectedCharacter].currentHp += amountToHeal;
        Instantiate(movesList[move].myEffect, activeBattlers[selectedCharacter].transform.position, activeBattlers[selectedCharacter].transform.rotation);
        DamageNumber cypherNumber = Instantiate(damageNumber, activeBattlers[selectedCharacter].transform.position, activeBattlers[selectedCharacter].transform.rotation);
        cypherNumber.GetComponentInChildren<Text>().text = amountToHeal.ToString();
        cypherNumber.GetComponentInChildren<Text>().color = Color.green;
    }

    public void SupportCypher(int move, int selectedCharacter, int caster)
    {
        Debug.Log("Support");
        statTarget statToModify = statTarget.None;
        BattleMove myMove = movesList[move];
        statToModify = SelectStat(myMove, statToModify);
        float modifier = (float) (myMove.movePower + activeBattlers[caster].element)/10;
        if(activeBattlers[selectedCharacter].hasAnAlteratedStat) RestorePreviousStatsValues(selectedCharacter);
        UpdateBattleCharStats(selectedCharacter, modifier, statToModify);
    }

    public void UpdateBattleCharStats(int target, float modifier, statTarget stat)
    {
        GameObject obj = null;
        for(int i = 0; i<playerPrefabs.Length;i++)
        {
            if(playerPrefabs[i].charName == activeBattlers[target].charName)
            {
                obj = battleUI.statsToModifyPanels[i];
            }
        }
        switch(stat)
        {
            case statTarget.Strength:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].strength);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Strength);
                activeBattlers[target].strength=Mathf.FloorToInt(activeBattlers[target].strength + modifier);
                battleUI.SetStatToModifyText(obj,"STR");
                break;
            case statTarget.Dexterity:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].dexterity);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].accuracy);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].evasion);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].initiative);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Dexterity);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Accuracy);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Evasion);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Initiative);
                activeBattlers[target].dexterity=Mathf.FloorToInt(activeBattlers[target].dexterity + modifier);
                activeBattlers[target].evasion += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.75f));
                activeBattlers[target].initiative += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.1f));
                activeBattlers[target].accuracy += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"DEX");
                break;
            case statTarget.Vitality:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].vitality);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].maxHP);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Vitality);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.HP);
                activeBattlers[target].vitality=Mathf.FloorToInt(activeBattlers[target].vitality + modifier);
                activeBattlers[target].maxHP = Mathf.FloorToInt(activeBattlers[target].maxHP * 1.05f + (activeBattlers[target].vitality * 2.5f));
                battleUI.SetStatToModifyText(obj,"VIT");
                break;
            case statTarget.Defence:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].defence);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Defence);
                activeBattlers[target].defence=Mathf.FloorToInt(activeBattlers[target].defence + modifier);
                battleUI.SetStatToModifyText(obj,"DEF");
                break;
            case statTarget.Element:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].element);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].mgcPower);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].maxMP);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Element);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.MagicPower);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.MP);
                activeBattlers[target].element=Mathf.FloorToInt(activeBattlers[target].element + modifier);
                activeBattlers[target].mgcPower += Mathf.FloorToInt(activeBattlers[target].element * Random.Range(0.125f, 0.25f));
                activeBattlers[target].maxMP += Mathf.FloorToInt(activeBattlers[target].element * 1.05f);
                battleUI.SetStatToModifyText(obj,"ELE");
                break;
            case statTarget.Spirit:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].spirit);
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].mgcDefence);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Spirit);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.MagicDefence);
                activeBattlers[target].spirit=Mathf.FloorToInt(activeBattlers[target].spirit + modifier);
                activeBattlers[target].mgcDefence += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"SPI");
                break;
            case statTarget.WeaponPower:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].wpnPower);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.WeaponPower);
                activeBattlers[target].wpnPower=Mathf.FloorToInt(activeBattlers[target].wpnPower + modifier);
                battleUI.SetStatToModifyText(obj,"W.P.");
                break;
            case statTarget.ArmorPower:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].armrPower);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.ArmorPower);
                activeBattlers[target].armrPower=Mathf.FloorToInt(activeBattlers[target].armrPower + modifier);
                battleUI.SetStatToModifyText(obj,"A.P.");
                break;
            case statTarget.MagicPower:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].mgcPower);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.MagicPower);
                activeBattlers[target].mgcPower += Mathf.FloorToInt(activeBattlers[target].element * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"M.P.");
                break;
            case statTarget.MagicDefence:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].mgcDefence);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.MagicDefence);
                activeBattlers[target].mgcDefence += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"M.D.");
                break;
            case statTarget.Accuracy:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].accuracy);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Accuracy);
                activeBattlers[target].accuracy += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"ACC");
                break;
            case statTarget.Evasion:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].evasion);
                activeBattlers[target].evasion += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"EVA");
                break;
            case statTarget.Initiative:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].initiative);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.Initiative);
                activeBattlers[target].initiative += Mathf.FloorToInt(activeBattlers[target].dexterity * Random.Range(0.5f, 0.1f));
                battleUI.SetStatToModifyText(obj,"INI");
                break;
            case statTarget.FireResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].fireResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.FireResistance);
                activeBattlers[target].fireResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"FireR");
                break;
            case statTarget.IceResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].iceResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.IceResistance);
                activeBattlers[target].iceResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"IceR");
                break;
            case statTarget.ElectricResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].electricResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.ElectricResistance);
                activeBattlers[target].electricResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"ElecR");
                break;
            case statTarget.PlasmaResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].plasmaResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.PlasmaResistance);
                activeBattlers[target].plasmaResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"PlasR");
                break;
            case statTarget.PsychicResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].psychicResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.PsychicResistance);
                activeBattlers[target].psychicResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"PsyR");
                break;
            case statTarget.ToxicResistance:
                activeBattlers[target].previousAlteratedStats.Add(activeBattlers[target].toxicResistance);
                activeBattlers[target].previousAlteratedStatsName.Add(statTarget.ToxicResistance);
                activeBattlers[target].toxicResistance += Mathf.FloorToInt(activeBattlers[target].spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"ToxR");
                break;
        }
        activeBattlers[target].changeAlteredStatBool(true);
        activeBattlers[target].setAlteredStatTimer(3);
        battleUI.UpdateUIStats(this);
    }

    public void RestorePreviousStatsValues(int target)
    {
        while (activeBattlers[target].previousAlteratedStats.Count>0)
        {
            switch(activeBattlers[target].previousAlteratedStatsName[0])
            {
                case statTarget.Strength:
                    activeBattlers[target].strength = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Dexterity:
                    activeBattlers[target].dexterity = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Vitality:
                    activeBattlers[target].vitality = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Defence:
                    activeBattlers[target].defence = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Element:
                    activeBattlers[target].element = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Spirit:
                    activeBattlers[target].spirit = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.WeaponPower:
                    activeBattlers[target].wpnPower = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.ArmorPower:
                    activeBattlers[target].armrPower = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.MagicPower:
                    activeBattlers[target].mgcPower = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.MagicDefence:
                    activeBattlers[target].mgcDefence = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Accuracy:
                    activeBattlers[target].accuracy = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Evasion:
                    activeBattlers[target].evasion = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.Initiative:
                    activeBattlers[target].initiative = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.FireResistance:
                    activeBattlers[target].fireResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.IceResistance:
                    activeBattlers[target].iceResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.ElectricResistance:
                    activeBattlers[target].electricResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.PlasmaResistance:
                    activeBattlers[target].plasmaResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.PsychicResistance:
                    activeBattlers[target].psychicResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.ToxicResistance:
                    activeBattlers[target].toxicResistance = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.HP:
                    activeBattlers[target].maxHP = activeBattlers[target].previousAlteratedStats[0];
                    break;
                case statTarget.MP:
                    activeBattlers[target].maxMP = activeBattlers[target].previousAlteratedStats[0];
                    break;
            }
            activeBattlers[target].previousAlteratedStats.RemoveAt(0);
            activeBattlers[target].previousAlteratedStatsName.RemoveAt(0);
        }
        activeBattlers[target].changeAlteredStatBool(false);
        activeBattlers[target].setAlteredStatTimer(0);
        activeBattlers[target].previousAlteratedStats.Clear();
        activeBattlers[target].previousAlteratedStatsName.Clear();
    }

    public statTarget SelectStat(BattleMove battleMove, statTarget stat)
    {
        switch(battleMove.sTarget)
        {
            case statTarget.Strength:
                stat = statTarget.Strength;
                break;
            case statTarget.Dexterity:
                stat = statTarget.Dexterity;
                break;
            case statTarget.Vitality:
                stat = statTarget.Vitality;
                break;
            case statTarget.Defence:
                stat = statTarget.Defence;
                break;
            case statTarget.Element:
                stat = statTarget.Element;
                break;
            case statTarget.Spirit:
                stat = statTarget.Spirit;
                break;
            case statTarget.WeaponPower:
                stat = statTarget.WeaponPower;
                break;
            case statTarget.ArmorPower:
                stat = statTarget.ArmorPower;
                break;
            case statTarget.MagicPower:
                stat = statTarget.MagicPower;
                break;
            case statTarget.MagicDefence:
                stat = statTarget.MagicDefence;
                break;
            case statTarget.Accuracy:
                stat = statTarget.Accuracy;
                break;
            case statTarget.Evasion:
                stat = statTarget.Evasion;
                break;
            case statTarget.Initiative:
                stat = statTarget.Initiative;
                break;
            case statTarget.FireResistance:
                stat = statTarget.FireResistance;
                break;
            case statTarget.IceResistance:
                stat = statTarget.IceResistance;
                break;
            case statTarget.ElectricResistance:
                stat = statTarget.ElectricResistance;
                break;
            case statTarget.PlasmaResistance:
                stat = statTarget.PlasmaResistance;
                break;
            case statTarget.ToxicResistance:
                stat = statTarget.ToxicResistance;
                break;
            case statTarget.PsychicResistance:
                stat = statTarget.PsychicResistance;
                break;
        }

        return stat;
    }

    public void ItemToPlayer(string itemName, int selectedTarget)
    {
        battleUI.HidePanel(battleUI.topPanel);
        Item[] myItems = GameManager.instance.referenceItems;

        for (int i = 0; i < myItems.Length; i++)
        {
            if (myItems[i].itemName == itemName)
            {
                myItems[i].UseItemInBattle(activeBattlers[selectedTarget], myItems, GameManager.instance.numberOfItems);
                break;
            }
        }
        battleUI.HidePanel(battleUI.targetMenu);
        battleUI.HidePanel(battleUI.playerPanel);
        canCheckPanels = false;
    }

    /*public void featNotImplemented()
    {
        warningWindow.transform.GetChild(0).GetComponent<Text>().text = "Not implemented yet";
        warningWindow.SetActive(true);
    }*/

    public void Flee()
    {
        hasClickedFleeButton = true;
        turnWaiting = false;
        int fleeSuccess = Random.Range(0, 100);
        if (fleeSuccess > chanceToFlee)
        {
            battleUI.HidePanel(battleUI.playerPanel);
            endInFlee = true;
            endInVictory = false;
            endInFailure = false;
            battleHasEnded = true;
        }

        else
        {
            battleUI.ChangeTopPanelText("Can't Escape!");
            battleUI.HidePanel(battleUI.playerPanel);
            battleUI.ShowPanel(battleUI.topPanel);
            Invoke("NextTurn",1f);
        }
    }

    public void EndBattle()
    {
        battleActive = false;
        battleHasStarted = false;
        hasSorted = false;
        canCheckPanels = false;
        firstTurnStarted = false;
        battleUI.HidePanel(battleUI.playerPanel);
        battleUI.ShowPanel(battleUI.topPanel);
        if (endInVictory)
        {
            battleUI.ChangeTopPanelText("Victory!");
            if (xpManager.expToGive > 0) xpManager.DivideXP(instance);
        }
        if (endInFlee)
        {
            battleUI.ChangeTopPanelText("You've escaped!");
            xpManager.SetXPToGive(0);
        }
        if (endInFailure)
        {
            battleUI.ChangeTopPanelText("You lost!");
            xpManager.SetXPToGive(0);
        }
        hasSorted = false;
        if (endInVictory || endInFlee) Invoke("ReturnToPlatform", 2f);
        if (endInFailure)
        {
            UIFade.instance.FadeToBlack();
            Invoke("GameOver", 1f);
        }
    }

    public void ReturnToPlatform()
    {
        GameManager.instance.battleActive = false;
        battleUI.HidePanel(battleUI.topPanel);
        UIFade.instance.FadeToBlack();
        GameManager.instance.returnToPlatform();
    }

    public IEnumerator EndBattle2Co()
    {
        yield return new WaitForSeconds(2f);
        if(endInFlee)
        {
            foreach(Transform position in playerPositions)
            {
                if(position.childCount>1)
                {
                    Destroy(position.GetChild(1).gameObject);
                }
            }
            
            foreach(Transform position in enemyPositions)
            {
                if(position.childCount>1)
                {
                    Destroy(position.GetChild(1).gameObject);
                }
            }
            
            indicators.ClearTurnIndicators();
        }
        transform.GetChild(0).transform.Find("Canvas").gameObject.SetActive(false);
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < PlayerController.instance.partyStats.Length; j++)
                {
                    if (activeBattlers[i].charName == PlayerController.instance.partyStats[j].charName)
                    {
                        PlayerController.instance.partyStats[j].GetCharStats().SetCurrentHP(activeBattlers[i].currentHp);
                        PlayerController.instance.partyStats[j].GetCharStats().SetCurrentMP(activeBattlers[i].currentMP);
                    }
                }
            }
            if (activeBattlers.Count > 0)
            {
                Destroy(activeBattlers[i].gameObject);
            }
        }
        battleItems.ClearItems();
        yield return new WaitForSeconds(1f);
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        yield return new WaitForSeconds(1f);
        UIFade.instance.FadeFromBlack();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("EndProto");
    }
}

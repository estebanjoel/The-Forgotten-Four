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
    public Battler[] playerPrefabs;
    public EnemyBattler[] enemyPrefabs;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;
    public List<Battler> activeBattlers = new List<Battler>();
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
                if (activeBattlers[currentTurn].chara.isPlayer)
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

    public static int sortByInitiative(Battler p1, Battler p2)
    {
        return (p2.chara.initiative.CompareTo(p1.chara.initiative));
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

    public List<Battler> SpecialSort(List<Battler> battlers)
    {
        List<Battler> newOrder = new List<Battler>();
        List<Battler> playerOrder = new List<Battler>();
        List<Battler> enemyOrder = new List<Battler>();
        
        for (int i = 0; i < battlers.Count; i++)
        {
            if (battlers[i].chara.isPlayer)
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
                if (!activeBattlers[i].chara.isPlayer) damageCalculator.DealDamage(i, movesList[0].movePower, moveType.Attack, elementType.NonElemental, moveModifier.Physical);
            }
        }

        if (toPlayer)
        {
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].chara.isPlayer) damageCalculator.DealDamage(i, movesList[0].movePower, moveType.Attack, elementType.NonElemental, moveModifier.Physical);
            }
        }
    }

    public void SetPlayerStats(int player)
    {
        CharStats thePlayer = PlayerController.instance.partyStats[player];
        activeBattlers[player].chara.SetCharacterStats(thePlayer.GetCharStats(), true);
        BattleHeroChar tempChar = (BattleHeroChar)activeBattlers[player].chara;
        tempChar.SetCyphersOnBattle(thePlayer);
        BattleChar newChar = (BattleChar)tempChar;
        activeBattlers[player].chara = newChar;
        battleUI.charStats[player].gameObject.SetActive(true);
    }

    public void SetEnemyStats(int enemy)
    {
        activeBattlers[enemy].chara.SetCharacterStats(activeBattlers[enemy].chara.BattleStats, false);
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
                            if (playerPrefabs[j].chara.charName == PlayerController.instance.partyStats[i].charName)
                            {
                                playerPositions[i].gameObject.SetActive(true);
                                Battler newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
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
                            if (enemyPrefabs[j].chara.charName == enemiesToSpawn[i])
                            {
                                EnemyBattler newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].transform.position, enemyPositions[i].transform.rotation);
                                newEnemy.transform.parent = enemyPositions[i];
                                activeBattlers.Add(newEnemy);
                                SetEnemyStats(i);
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
            if (activeBattlers[currentTurn].chara.isPlayer)
            {
                battleUI.ChangeTopPanelText("Is " + activeBattlers[currentTurn].chara.charName + "'s turn");
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
        if(activeBattlers[currentTurn].chara.hasAnAlteratedStat)
        {
            if(activeBattlers[currentTurn].chara.alteratedStatTimer>0) activeBattlers[currentTurn].chara.alteratedStatTimer--;
            else
            {
                bool hasInitiativeChanged = false;
                for(int i = 0; i<activeBattlers[currentTurn].chara.previousAlteratedStatsName.Count; i++)
                {
                    if(activeBattlers[currentTurn].chara.previousAlteratedStatsName[i] == statTarget.Initiative)
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
        Battler currentChar = activeBattlers[currentTurn];
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
            if (activeBattlers[i].chara.currentHp < 0) activeBattlers[i].chara.currentHp = 0;

            if (activeBattlers[i].chara.currentHp == 0)
            {
                if (activeBattlers[i].chara.isPlayer)
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
                if (activeBattlers[i].chara.isPlayer)
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
            while (activeBattlers[currentTurn].chara.currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }

            battleUI.UpdateUIStats(instance);
            battleUI.ShowPanel(battleUI.topPanel);
            if (activeBattlers[currentTurn].chara.isPlayer)
            {
                battleUI.ChangeTopPanelText("Is " + activeBattlers[currentTurn].chara.charName + "'s turn");
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
        if(activeBattlers[currentTurn].GetComponent<YakovSpecifics>() != null)
        {
            if(activeBattlers[currentTurn].GetComponent<YakovSpecifics>().CheckIfIsDrunk())
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
                        if(activeBattlers[j].chara.isPlayer && activeBattlers[j].chara.currentHp > 0)
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
            while(activeBattlers[randomTarget].chara.isPlayer || activeBattlers[randomTarget].chara.currentHp<=0)
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
            while(!activeBattlers[randomTarget].chara.isPlayer || activeBattlers[randomTarget].chara.currentHp<=0)
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
                        if(!activeBattlers[j].chara.isPlayer && activeBattlers[j].chara.currentHp > 0)
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
                        if(activeBattlers[j].chara.isPlayer && activeBattlers[j].chara.currentHp > 0)
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
        if(!activeBattlers[battler].chara.isPlayer)
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
                for(int i = 0; i<activeBattlers[currentTurn].chara.movesAvailable.Count;i++)
                {
                    for(int j = 0; j<movesList.Length;j++)
                    {
                        if(activeBattlers[currentTurn].chara.movesAvailable[i] == movesList[j].moveName)
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

    public bool AttackProbability(Battler attacker, Battler target)
    {
        int attackerProbability = Random.Range(1, 21);
        if (attackerProbability == 20) return true;
        int targetProbability = Random.Range(1, 21);
        if (targetProbability == 20) return false;
        float attackResult = (attacker.chara.dexterity + attacker.chara.accuracy + attackerProbability) - (target.chara.dexterity + target.chara.evasion + targetProbability/2);
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
            if (activeBattlers[i].chara.isPlayer && activeBattlers[i].chara.currentHp > 0) players.Add(i);
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
            int selectAttack = Random.Range(0, activeBattlers[currentTurn].chara.movesAvailable.Count);
            for (int i = 0; i < movesList.Length; i++)
            {
                if (movesList[i].moveName == activeBattlers[currentTurn].chara.movesAvailable[selectAttack])
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
        int modifier = Mathf.FloorToInt((healMove.movePower + activeBattlers[caster].chara.spirit + activeBattlers[caster].chara.mgcPower) * Random.Range(0.9f,1.5f));
        moveClip = healMove.myAudioClip;
        yield return new WaitForSeconds(.5f);
        AttackEffect attackEffect = Instantiate(healMove.myEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        attackEffect.GetComponent<SpriteRenderer>().flipX = true;
        audioManager.PlayMoveAudio(moveClip);
        yield return new WaitForSeconds(1f);
        Instantiate(BattleManager.instance.damageNumber, BattleManager.instance.activeBattlers[target].transform.position, BattleManager.instance.activeBattlers[target].transform.rotation).SetDamage(modifier);
        GameObject.FindObjectOfType<DamageNumber>().GetComponentInChildren<Text>().color = Color.green;
        activeBattlers[target].chara.currentHp+=modifier;
        if(activeBattlers[target].chara.currentHp > activeBattlers[target].chara.maxHP) activeBattlers[target].chara.currentHp = activeBattlers[target].chara.maxHP;
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
                if (movesList[i].eType == elementType.None && moveName == "Vodka Madness") activeBattlers[selectedTarget].GetComponent<YakovSpecifics>().VodkaMadness();
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
        int amountToHeal = Mathf.FloorToInt(((activeBattlers[caster].chara.spirit * Random.Range(0.9f, 1.5f) + movesList[move].movePower) * Random.Range(0.9f, 1.25f)));
        activeBattlers[selectedCharacter].chara.currentHp += amountToHeal;
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
        float modifier = (float) (myMove.movePower + activeBattlers[caster].chara.element)/10;
        if(activeBattlers[selectedCharacter].chara.hasAnAlteratedStat) RestorePreviousStatsValues(selectedCharacter);
        UpdateBattleCharStats(selectedCharacter, modifier, statToModify);
    }

    public void UpdateBattleCharStats(int target, float modifier, statTarget stat)
    {
        GameObject obj = null;
        for(int i = 0; i<playerPrefabs.Length;i++)
        {
            if(playerPrefabs[i].chara.charName == activeBattlers[target].chara.charName)
            {
                obj = battleUI.statsToModifyPanels[i];
            }
        }
        switch(stat)
        {
            case statTarget.Strength:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.strength);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Strength);
                activeBattlers[target].chara.strength=Mathf.FloorToInt(activeBattlers[target].chara.strength + modifier);
                battleUI.SetStatToModifyText(obj,"STR");
                break;
            case statTarget.Dexterity:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.dexterity);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.accuracy);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.evasion);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.initiative);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Dexterity);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Accuracy);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Evasion);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Initiative);
                activeBattlers[target].chara.dexterity=Mathf.FloorToInt(activeBattlers[target].chara.dexterity + modifier);
                activeBattlers[target].chara.evasion += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.75f));
                activeBattlers[target].chara.initiative += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.1f));
                activeBattlers[target].chara.accuracy += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"DEX");
                break;
            case statTarget.Vitality:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.vitality);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.maxHP);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Vitality);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.HP);
                activeBattlers[target].chara.vitality=Mathf.FloorToInt(activeBattlers[target].chara.vitality + modifier);
                activeBattlers[target].chara.maxHP = Mathf.FloorToInt(activeBattlers[target].chara.maxHP * 1.05f + (activeBattlers[target].chara.vitality * 2.5f));
                battleUI.SetStatToModifyText(obj,"VIT");
                break;
            case statTarget.Defence:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.defence);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Defence);
                activeBattlers[target].chara.defence=Mathf.FloorToInt(activeBattlers[target].chara.defence + modifier);
                battleUI.SetStatToModifyText(obj,"DEF");
                break;
            case statTarget.Element:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.element);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.mgcPower);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.maxMP);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Element);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.MagicPower);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.MP);
                activeBattlers[target].chara.element=Mathf.FloorToInt(activeBattlers[target].chara.element + modifier);
                activeBattlers[target].chara.mgcPower += Mathf.FloorToInt(activeBattlers[target].chara.element * Random.Range(0.125f, 0.25f));
                activeBattlers[target].chara.maxMP += Mathf.FloorToInt(activeBattlers[target].chara.element * 1.05f);
                battleUI.SetStatToModifyText(obj,"ELE");
                break;
            case statTarget.Spirit:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.spirit);
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.mgcDefence);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Spirit);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.MagicDefence);
                activeBattlers[target].chara.spirit=Mathf.FloorToInt(activeBattlers[target].chara.spirit + modifier);
                activeBattlers[target].chara.mgcDefence += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"SPI");
                break;
            case statTarget.WeaponPower:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.wpnPower);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.WeaponPower);
                activeBattlers[target].chara.wpnPower=Mathf.FloorToInt(activeBattlers[target].chara.wpnPower + modifier);
                battleUI.SetStatToModifyText(obj,"W.P.");
                break;
            case statTarget.ArmorPower:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.armrPower);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.ArmorPower);
                activeBattlers[target].chara.armrPower=Mathf.FloorToInt(activeBattlers[target].chara.armrPower + modifier);
                battleUI.SetStatToModifyText(obj,"A.P.");
                break;
            case statTarget.MagicPower:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.mgcPower);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.MagicPower);
                activeBattlers[target].chara.mgcPower += Mathf.FloorToInt(activeBattlers[target].chara.element * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"M.P.");
                break;
            case statTarget.MagicDefence:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.mgcDefence);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.MagicDefence);
                activeBattlers[target].chara.mgcDefence += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0.125f, 0.25f));
                battleUI.SetStatToModifyText(obj,"M.D.");
                break;
            case statTarget.Accuracy:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.accuracy);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Accuracy);
                activeBattlers[target].chara.accuracy += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"ACC");
                break;
            case statTarget.Evasion:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.evasion);
                activeBattlers[target].chara.evasion += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.75f));
                battleUI.SetStatToModifyText(obj,"EVA");
                break;
            case statTarget.Initiative:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.initiative);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.Initiative);
                activeBattlers[target].chara.initiative += Mathf.FloorToInt(activeBattlers[target].chara.dexterity * Random.Range(0.5f, 0.1f));
                battleUI.SetStatToModifyText(obj,"INI");
                break;
            case statTarget.FireResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.fireResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.FireResistance);
                activeBattlers[target].chara.fireResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"FireR");
                break;
            case statTarget.IceResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.iceResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.IceResistance);
                activeBattlers[target].chara.iceResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"IceR");
                break;
            case statTarget.ElectricResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.electricResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.ElectricResistance);
                activeBattlers[target].chara.electricResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"ElecR");
                break;
            case statTarget.PlasmaResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.plasmaResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.PlasmaResistance);
                activeBattlers[target].chara.plasmaResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"PlasR");
                break;
            case statTarget.PsychicResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.psychicResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.PsychicResistance);
                activeBattlers[target].chara.psychicResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"PsyR");
                break;
            case statTarget.ToxicResistance:
                activeBattlers[target].chara.previousAlteratedStats.Add(activeBattlers[target].chara.toxicResistance);
                activeBattlers[target].chara.previousAlteratedStatsName.Add(statTarget.ToxicResistance);
                activeBattlers[target].chara.toxicResistance += Mathf.FloorToInt(activeBattlers[target].chara.spirit * Random.Range(0, 0.25f));
                battleUI.SetStatToModifyText(obj,"ToxR");
                break;
        }
        activeBattlers[target].chara.changeAlteredStatBool(true);
        activeBattlers[target].chara.setAlteredStatTimer(3);
        battleUI.UpdateUIStats(this);
    }

    public void RestorePreviousStatsValues(int target)
    {
        while (activeBattlers[target].chara.previousAlteratedStats.Count>0)
        {
            switch(activeBattlers[target].chara.previousAlteratedStatsName[0])
            {
                case statTarget.Strength:
                    activeBattlers[target].chara.strength = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Dexterity:
                    activeBattlers[target].chara.dexterity = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Vitality:
                    activeBattlers[target].chara.vitality = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Defence:
                    activeBattlers[target].chara.defence = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Element:
                    activeBattlers[target].chara.element = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Spirit:
                    activeBattlers[target].chara.spirit = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.WeaponPower:
                    activeBattlers[target].chara.wpnPower = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.ArmorPower:
                    activeBattlers[target].chara.armrPower = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.MagicPower:
                    activeBattlers[target].chara.mgcPower = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.MagicDefence:
                    activeBattlers[target].chara.mgcDefence = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Accuracy:
                    activeBattlers[target].chara.accuracy = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Evasion:
                    activeBattlers[target].chara.evasion = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.Initiative:
                    activeBattlers[target].chara.initiative = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.FireResistance:
                    activeBattlers[target].chara.fireResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.IceResistance:
                    activeBattlers[target].chara.iceResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.ElectricResistance:
                    activeBattlers[target].chara.electricResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.PlasmaResistance:
                    activeBattlers[target].chara.plasmaResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.PsychicResistance:
                    activeBattlers[target].chara.psychicResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.ToxicResistance:
                    activeBattlers[target].chara.toxicResistance = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.HP:
                    activeBattlers[target].chara.maxHP = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
                case statTarget.MP:
                    activeBattlers[target].chara.maxMP = activeBattlers[target].chara.previousAlteratedStats[0];
                    break;
            }
            activeBattlers[target].chara.previousAlteratedStats.RemoveAt(0);
            activeBattlers[target].chara.previousAlteratedStatsName.RemoveAt(0);
        }
        activeBattlers[target].chara.changeAlteredStatBool(false);
        activeBattlers[target].chara.setAlteredStatTimer(0);
        activeBattlers[target].chara.previousAlteratedStats.Clear();
        activeBattlers[target].chara.previousAlteratedStatsName.Clear();
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
                Consumable itemToUse = (Consumable) myItems[i];
                itemToUse.UseItemInBattle(activeBattlers[selectedTarget], myItems, GameManager.instance.numberOfItems);
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
            if (activeBattlers[i].chara.isPlayer)
            {
                for (int j = 0; j < PlayerController.instance.partyStats.Length; j++)
                {
                    if (activeBattlers[i].chara.charName == PlayerController.instance.partyStats[j].charName)
                    {
                        PlayerController.instance.partyStats[j].GetCharStats().SetCurrentHP(activeBattlers[i].chara.currentHp);
                        PlayerController.instance.partyStats[j].GetCharStats().SetCurrentMP(activeBattlers[i].chara.currentMP);
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

using System;
using System.Collections;
using System.Collections.Generic;
using Channels;
using Monsters;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum BattleState { START, SCIENTISTTURN, MONSTERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    //Monster/scientist/enemy prefabs made here
   
    public GameObject monsterPrefab;
    public GameObject enemyPrefab;
    public GameObject scientistPrefab;
    
    //The position of the monster/scientist/enemy on spawn. Unsure if this is necessary, though
    public Transform monsterPos;
    public Transform enemyPos;
    public Transform scientistPos;
   
    //absolute Units! These refer to the 3 actors in the scene
    [SerializeField]
    private VoidChannel doPlayerAction;
    [SerializeField]
    private VoidChannel doEnemyAction;
    [SerializeField]
    private MonsterActionChannel queuePlayerAction;
    [SerializeField]
    private VoidChannel onPlayerDie;
    [SerializeField]
    private VoidChannel onEnemyDie;
    
    [SerializeField] private MonsterStatsChannel onPlayerStatsUpdated;
    [SerializeField] private MonsterStatsChannel onEnemyStatsUpdated;
    [SerializeField] private VoidChannel requestPlayerStats;
    [SerializeField] private VoidChannel requestEnemyStats;
    [SerializeField] private VoidChannel resetPlayerAttack;
    [SerializeField] private VoidChannel resetPlayerDefense;
    [SerializeField] private VoidChannel resetPlayerHealth;
    [SerializeField] private VoidChannel resetEnemyAttack;
    [SerializeField] private VoidChannel resetEnemyDefense;
    [SerializeField] private VoidChannel resetEnemyHealth;
    [SerializeField] private VoidChannel selectPart;
    [SerializeField] private VoidChannel restartBattle;
    
    private MonsterStats playerStats;
    private MonsterStats enemyStats;
    
    //The heads-up display objects for the 3 actors
    public BattleHUD monsterHUD;
    public BattleHUD enemyHUD;
    public BattleHUD scientistHUD;

    //text object for any dialogue needed
    public TextMeshProUGUI dialogueText;

    //The current state of the battle-see above enum for states
    public BattleState currState;

    //these are just for the ui
    GameObject tex;
    public GameObject monsterRow;
    public GameObject scientistRow;

    public GameObject partSelect;
    //Set the battlestate to start and start the coroutine
    void Start()
    {
        tex = GameObject.Find("DialogueText");
        StartBattle();
    }

    private void StartBattle()
    {
        currState = BattleState.START;
        StartCoroutine(BeginBattle());
    }
    
    IEnumerator BeginBattle()
    {
        GameObject mon = GameObject.Find("MonsterHUD");
        monsterHUD = mon.GetComponent<BattleHUD>();

        GameObject ene = GameObject.Find("EnemyHUD");
        enemyHUD = ene.GetComponent<BattleHUD>();

        GameObject sci = GameObject.Find("ScientistHUD");
        scientistHUD = sci.GetComponent<BattleHUD>();

        
        dialogueText = tex.GetComponent<TextMeshProUGUI>();
        //make GO's of the 3 lads and save em in variables for later reference
        /*
        GameObject monster = Instantiate(monsterPrefab, monsterPos);
        monsterUnit = monster.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab, enemyPos);
        enemyUnit = enemy.GetComponent<Unit>();

        GameObject scientist = Instantiate(scientistPrefab, scientistPos);
        scientistUnit = scientist.GetComponent<Unit>();
       */
        //Display enemy name
        requestPlayerStats.RaiseEvent();
        requestEnemyStats.RaiseEvent();
        //yield return new WaitForEndOfFrame();
        dialogueText.text = "A fearsome " + enemyStats.name + " approaches!";

        //set up HUD for all 3
        // TODO: Update setHUD
        monsterHUD.setHUD(playerStats);
        enemyHUD.setHUD(enemyStats);
        //scientistHUD.setHUD(scientistUnit);
        yield return new WaitForSeconds(2f);

        //progress to the scientist's turn
        currState = BattleState.SCIENTISTTURN;
        ScientistTurn();
    }

    //Display the text. Don't forget to add the buttons to actually, yknow, do stuff
    void ScientistTurn()
    {
        scientistRow.SetActive(true);
        tex.SetActive(false);
        resetPlayerAttack.RaiseEvent();
        resetPlayerDefense.RaiseEvent();
        dialogueText.text = "Make your choice: ";//attack button and defend button go next to this
    }

    //Starts the buff attack coroutine if this is clicked
    public void OnBuffAttackButton()
    {
        if (currState != BattleState.SCIENTISTTURN)
        {
            return;
        }
        StartCoroutine(MonsterAttackBuff());
    }
   
    //Starts the defense buff coroutine if this is clicked
    public void OnBuffDefenseButton()
    {
        if (currState != BattleState.SCIENTISTTURN)
        {
            return;
        }
        StartCoroutine(MonsterDefenseBuff());
    }

    //Buffs the monster's attack by referencing the proper function in the Unit script
    //Then progresses to the monster's turn
    IEnumerator MonsterAttackBuff()
    {
        tex.SetActive(true);
        scientistRow.SetActive(false);
        //monsterUnit.buffDamage(monsterUnit.damage);
        queuePlayerAction.RaiseEvent(MONSTER_ACTIONS.BUFF_ATTACK);
        doPlayerAction.RaiseEvent();
        dialogueText.text = "Your creature grows stronger";
        yield return new WaitForSeconds(2f);
        monsterRow.SetActive(true);
        tex.SetActive(false);
        dialogueText.text = "Select your monster's move";
        currState = BattleState.MONSTERTURN;
    }
    //Buffs the monster's defense by referencing the proper function in the Unit script
    //Then progresses to the monster's turn
    IEnumerator MonsterDefenseBuff()
    {
        tex.SetActive(true);
        scientistRow.SetActive(false);
        //monsterUnit.Defend(enemyUnit.damage);//?
        queuePlayerAction.RaiseEvent(MONSTER_ACTIONS.BUFF_DEFENSE);
        doPlayerAction.RaiseEvent();
        dialogueText.text = "Your creature beefs up";
        yield return new WaitForSeconds(2f);
        monsterRow.SetActive(true);
        tex.SetActive(false);
        dialogueText.text = "Select your monster's move";
        currState = BattleState.MONSTERTURN;
    }

    //If it's the monster's turn, they'll attack with the primary attack
    public void OnAttackButton()
    {
        if (currState != BattleState.MONSTERTURN)
        {
            return;
        }
        StartCoroutine(MonsterAttack());
    }

    //The monster attacks. Will keep it basic for now-just does straight up damage
    //Then checks if the enemy's dead-if so, end the round, if not, progress to enemy's turn
    IEnumerator MonsterAttack()
    {
        monsterRow.SetActive(false);
        tex.SetActive(true);
        //bool isDead = enemyUnit.TakeDamage(monsterUnit.damage);
        queuePlayerAction.RaiseEvent(MONSTER_ACTIONS.BASE_ATTACK);
        dialogueText.text = "Your monster attacks";
        doPlayerAction.RaiseEvent();
        yield return new WaitForSeconds(2f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    //Adding this for posterity-the monster's second attack, right now, functions the same as the other one
    //Hence the coroutine going to the same MonsterAttack function
    public void OnAltAttackButton()//need new coroutine for alt attack, and prolly a new function in Unit
    {
        if (currState != BattleState.MONSTERTURN)
        {
            return;
        }
        StartCoroutine(MonsterAttack());
    }
    /* this isnt needed, as the monster itself doesn't defend, but i'm keeping it commented in case the logic comes in handy
    IEnumerator MonsterDefend()
    {
        monsterUnit.Defend(5);
        //do i need something with the HUD here?
        yield return new WaitForSeconds(2f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    */
    //The enemy's actions. Basic attack, deals whatever health damage, then checks if the monster's dead.
    //if not, progress back to the scientist's turn and start over
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyStats.name + " makes its move";
        doEnemyAction.RaiseEvent();
        yield return new WaitForSeconds(1f);
        currState = BattleState.SCIENTISTTURN;
        ScientistTurn();
    }
    
    //This ends the battle and displays text appropriate to the outcome
    void EndBattle()
    {
        StopAllCoroutines();
        ResetStats();
        Debug.Log("Setting dialogue text");
        if (currState == BattleState.WON)
        {
            Debug.Log("Player won!");
            dialogueText.text = "Monster slain";
            partSelect.SetActive(true);
            StartCoroutine(DelayPartSelect());
        }
        else if (currState == BattleState.LOST)
        {
            Debug.Log("Enemy won!");
            dialogueText.text = "You were defeated";
        }
    }

    private IEnumerator DelayPartSelect()
    {
        yield return new WaitForSeconds(0.5f);
        selectPart.RaiseEvent();
    }
    
    private void OnEnable()
    {
        onPlayerDie.OnEventRaised += OnPlayerDie;
        onEnemyDie.OnEventRaised += OnEnemyDie;
        onPlayerStatsUpdated.OnEventRaised += UpdatePlayerStats;
        onEnemyStatsUpdated.OnEventRaised += UpdateEnemyStats;
        restartBattle.OnEventRaised += StartBattle;
    }

    private void OnDisable()
    {
        onPlayerDie.OnEventRaised -= OnPlayerDie;
        onEnemyDie.OnEventRaised -= OnEnemyDie;
        onPlayerStatsUpdated.OnEventRaised -= UpdatePlayerStats;
        onEnemyStatsUpdated.OnEventRaised -= UpdateEnemyStats;
        restartBattle.OnEventRaised -= StartBattle;
    }

    private void UpdatePlayerStats(MonsterStats obj)
    {
        playerStats = obj;
        monsterHUD.setHealth(obj.health);
    }

    private void UpdateEnemyStats(MonsterStats obj)
    {
        enemyStats = obj;
        enemyHUD.setHealth(obj.health);
    }

    private void OnEnemyDie()
    {
        currState = BattleState.WON;
        EndBattle();
    }

    public void OnPlayerDie()
    {
        currState = BattleState.LOST;
        EndBattle();
    }

    public void ResetStats()
    {
        resetEnemyAttack.RaiseEvent();
        resetEnemyDefense.RaiseEvent();
        resetEnemyHealth.RaiseEvent();
        resetPlayerAttack.RaiseEvent();
        resetPlayerDefense.RaiseEvent();
        resetPlayerHealth.RaiseEvent();
    }
    
}

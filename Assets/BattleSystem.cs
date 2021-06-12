using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    Unit monsterUnit;
    Unit enemyUnit;
    Unit scientistUnit;

    //The heads-up display objects for the 3 actors
    public BattleHUD monsterHUD;
    public BattleHUD enemyHUD;
    public BattleHUD scientistHUD;

    //text object for any dialogue needed
    public Text dialogueText;

    //The current state of the battle-see above enum for states
    public BattleState currState;
    
    //Set the battlestate to start and start the coroutine
    void Start()
    {
        currState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    IEnumerator BeginBattle()
    {
        //make GO's of the 3 lads and save em in variables for later reference
        GameObject monster = Instantiate(monsterPrefab, monsterPos);
        monsterUnit = monster.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab, enemyPos);
        enemyUnit = enemy.GetComponent<Unit>();

        GameObject scientist = Instantiate(scientistPrefab, scientistPos);
        scientistUnit = scientist.GetComponent<Unit>();
       
        //Display enemy name
        dialogueText.text = "A fearsome " + enemyUnit.unitName + " approaches!";

        //set up HUD for all 3
        monsterHUD.setHUD(monsterUnit);
        enemyHUD.setHUD(enemyUnit);
        scientistHUD.setHUD(scientistUnit);
        yield return new WaitForSeconds(2f);

        //progress to the scientist's turn
        currState = BattleState.SCIENTISTTURN;
        ScientistTurn();
    }

    //Display the text. Don't forget to add the buttons to actually, yknow, do stuff
    void ScientistTurn()
    {
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
        monsterUnit.buffDamage(monsterUnit.damage);
        dialogueText.text = "Your creature grows stronger";
        yield return new WaitForSeconds(2f);
        currState = BattleState.MONSTERTURN;
    }
    //Buffs the monster's defense by referencing the proper function in the Unit script
    //Then progresses to the monster's turn
    IEnumerator MonsterDefenseBuff()
    {
        monsterUnit.Defend(enemyUnit.damage);//?
        dialogueText.text = "Your creature beefs up";
        yield return new WaitForSeconds(2f);
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
        bool isDead = enemyUnit.TakeDamage(monsterUnit.damage);
        enemyHUD.setHealth(enemyUnit.currentHealth);
        dialogueText.text = "Success";
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            currState = BattleState.WON;
            EndBattle();
        }
        else
        {
            currState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
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
        dialogueText.text = enemyUnit.unitName + " attacks";
        yield return new WaitForSeconds(1f);
        bool isDead = enemyUnit.TakeDamage(enemyUnit.damage);
        enemyHUD.setHealth(enemyUnit.currentHealth);
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            currState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            currState = BattleState.SCIENTISTTURN;
            ScientistTurn();
        }
    }
    //This ends the battle and displays text appropriate to the outcome
    void EndBattle()
    {
        if (currState == BattleState.WON)
        {
            dialogueText.text = "Monster slain";
        }
        else if (currState == BattleState.LOST)
        {
            dialogueText.text = "You were defeated";
        }
    }
}

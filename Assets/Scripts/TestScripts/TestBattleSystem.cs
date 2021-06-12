using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class TestBattleSystem : MonoBehaviour
{
    //Monster/scientist/enemy prefabs made here
    //Remember: player controls the scientist
    public GameObject monsterPrefab;
    public GameObject enemyPrefab;
    public GameObject scientistPrefab;
    //The position of the monster/scientist/enemy on spawn
    public Transform monsterPos;
    public Transform enemyPos;
    public Transform scientistPos;
    //absolute Units!
    Unit monsterUnit;
    Unit enemyUnit;
    Unit scientistUnit;

    public BattleHUD monsterHUD;
    public BattleHUD enemyHUD;
    public BattleHUD scientistHUD;

    public Text dialogueText;
    
    public BattleState currState;
    // Start is called before the first frame update
    void Start()
    {
        currState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    IEnumerator BeginBattle()
    {
        //make GO's of the 3 lads and save em in variables for later reference
        GameObject monster =  Instantiate(monsterPrefab, monsterPos);
        monsterUnit = monster.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab, enemyPos);
        enemyUnit = enemy.GetComponent<Unit>();

        GameObject scientist = Instantiate(scientistPrefab, scientistPos);
        scientistUnit = scientist.GetComponent<Unit>();
        //Display enemy name
        dialogueText.text = "A fearsome " + enemyUnit.unitName + " approaches!";

        //set up HUD
        monsterHUD.setHUD(monsterUnit);
        enemyHUD.setHUD(enemyUnit);
        scientistHUD.setHUD(scientistUnit);
        yield return new WaitForSeconds(2f);

        currState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

   
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
            currState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    void EndBattle()
    {
        if(currState == BattleState.WON)
        {
            dialogueText.text = "Monster slain";
        }
        else if(currState == BattleState.LOST)
        {
            dialogueText.text = "You were defeated";
        }
    }
    void PlayerTurn()
    {
        dialogueText.text = "Make your choice: ";
    }
    IEnumerator PlayerAttack()
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

    public void OnAttackButton()
    {
        if(currState != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerDefend()
    {
        scientistUnit.Defend(5);
        //do i need something with the HUD here?
        yield return new WaitForSeconds(2f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    public void OnDefendButton()
    {
        if (currState != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerDefend());
    }
}

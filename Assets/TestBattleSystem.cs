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
        BeginBattle();
    }

    void BeginBattle()
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
    }
}

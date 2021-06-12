using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class TestBattleSystem : MonoBehaviour
{
    //Create GO prefabs
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    //unsure if needed. tutorial had sprites on "battlestations",
    //aka plots of ground the sprites could stand on. I added em just in case
    public Transform playerPosition;
    public Transform enemyPosition;
    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;
    //the current state the battle is in. see above enum for states
    public BattleState currState;
    // Start is called before the first frame update
    void Start()
    {
        currState = BattleState.START;
        SetBattle();
    }

    void SetBattle()
    {
        GameObject player = Instantiate(playerPrefab, playerPosition);
        playerUnit =  player.GetComponent<Unit>();

        GameObject enemy = Instantiate(enemyPrefab, enemyPosition);
        enemyUnit = enemy.GetComponent<Unit>();

        dialogueText.text = "A fearsome " + enemyUnit.name + "approaches!";
    }
    
}

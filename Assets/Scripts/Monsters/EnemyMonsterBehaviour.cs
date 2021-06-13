using System;
using System.Collections.Generic;
using Channels;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class EnemyMonsterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private MonsterActionChannel enemyQueueActionChannel;

        [SerializeField]
        private VoidChannel enemyDoActionChannel;

        [SerializeField]
        private BodyPartChannel enemyBodyChange;

        [SerializeField] private IntChannel partSelectInfoSend;

        [SerializeField] private VoidChannel partSelectUI;

        [SerializeField] private VoidChannel restartBattle;
        
        private readonly Queue<MONSTER_ACTIONS> actionList = new Queue<MONSTER_ACTIONS>();

        [SerializeField]
        private BodyPartBaseSO[] partArray = new BodyPartBaseSO[2];
        
        [SerializeField] private VoidChannel nextMonster;

        [SerializeField] private List<BodyPartBaseSO> enemyTwo = new List<BodyPartBaseSO>();
        [SerializeField] private BodyPartBaseSO[] enemyTwoParts = new BodyPartBaseSO[2];

        [SerializeField] private List<BodyPartBaseSO> enemyBoss = new List<BodyPartBaseSO>();
        [SerializeField] private BodyPartBaseSO[] enemyThreeParts = new BodyPartBaseSO[2];

        private List<List<BodyPartBaseSO>> enemies = new List<List<BodyPartBaseSO>>();
        private List<BodyPartBaseSO[]> parts = new List<BodyPartBaseSO[]>();

        private int counter = 0;
        private void GenerateAction()
        {
            MONSTER_ACTIONS newAction = (MONSTER_ACTIONS)Random.Range(0, (int) MONSTER_ACTIONS.STALL);
            actionList.Enqueue(newAction);
            enemyQueueActionChannel.RaiseEvent(newAction);
        }

        public void DoAction()
        {
            //enemyDoActionChannel.RaiseEvent();
            actionList.Dequeue();
            if (actionList.Count <= 2)
            {
                GenerateAction();
            }

        }

        private void OnEnable()
        {
            nextMonster.OnEventRaised += NextMonster;
            enemyDoActionChannel.OnEventRaised += DoAction;
            partSelectUI.OnEventRaised += SendPartInfo;
        }

        private void OnDisable()
        {
            nextMonster.OnEventRaised -= NextMonster;
            enemyDoActionChannel.OnEventRaised -= DoAction;
            partSelectUI.OnEventRaised -= SendPartInfo;
        }

        private void SendPartInfo()
        {
            Debug.Log("Sending part info!");
            partSelectInfoSend.RaiseEvent(counter);
        }

        private void NextMonster()
        {
            List<BodyPartBaseSO> currentEnemy = enemies[counter];
            for(int i = 0; i < currentEnemy.Count; ++i)
            {
                BodyPartChangeEvent newPart = new BodyPartChangeEvent();
                newPart.location = (PART_LOCATIONS)i;
                newPart.newPart = currentEnemy[i];
                Transform child = transform.GetChild(i);
                Vector3 newPos = Vector3.zero;
                newPos.x = newPart.newPart.xLocOffset;
                newPos.y = newPart.newPart.yLocOffset;
                child.position = newPos;

                Vector3 newScale = Vector3.one;
                newScale.x = newPart.newPart.xScaleOffset;
                newScale.y = newPart.newPart.yScaleOffset;
                child.localScale = newScale;
                enemyBodyChange.RaiseEvent(newPart);
            }
            
            ++counter;
            restartBattle.RaiseEvent();
        }


        private void Start()
        {
            GenerateAction();
            foreach (var monsterAction in actionList)
            {
                enemyQueueActionChannel.RaiseEvent(monsterAction);
            }
            
            enemies.Add(enemyTwo);
            enemies.Add(enemyBoss);
            parts.Add(enemyTwoParts);
            parts.Add(enemyThreeParts);
        }
        
    }
}
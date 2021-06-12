using System.Collections.Generic;
using Channels;
using UnityEngine;

namespace Monsters
{
    public class EnemyMonsterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private MonsterActionChannel enemyQueueActionChannel;

        [SerializeField]
        private VoidChannel enemyDoActionChannel;
        
        [SerializeField]
        private List<MONSTER_ACTIONS> actionList = new List<MONSTER_ACTIONS>();

        public void GenerateAction()
        {
            MONSTER_ACTIONS newAction = (MONSTER_ACTIONS)Random.Range(0, (int) MONSTER_ACTIONS.STALL);
            actionList.Add(newAction);
            enemyQueueActionChannel.RaiseEvent(newAction);
        }

        public void DoAction()
        {
            enemyDoActionChannel.RaiseEvent();
        }

        private void Start()
        {
            foreach (var monsterAction in actionList)
            {
                enemyQueueActionChannel.RaiseEvent(monsterAction);
            }
        }
        
    }
}
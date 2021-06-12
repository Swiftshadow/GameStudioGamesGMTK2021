using BodyParts;
using Channels;
using UnityEngine;

namespace TestScripts
{
    public class TestActionQueueScript : MonoBehaviour
    {
        public MonsterActionChannel playerActionChannel;

        public void AddAction(int action)
        {
            MONSTER_ACTIONS newAction = (MONSTER_ACTIONS) action;
            playerActionChannel.RaiseEvent(newAction);
        }
    }
}
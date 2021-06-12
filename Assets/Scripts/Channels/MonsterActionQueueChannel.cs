using System;
using System.Collections.Generic;
using Monsters;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New Monster Action Queue Channel", menuName = "Monster/Monster Action Queue Channel")]
    public class MonsterActionQueueChannel : BaseChannel
    {
        public Action<Queue<MONSTER_ACTIONS>> OnEventRaised;
        
        public void RaiseEvent(Queue<MONSTER_ACTIONS> value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
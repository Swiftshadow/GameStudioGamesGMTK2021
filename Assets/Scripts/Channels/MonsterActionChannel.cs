using System;
using Monsters;
using UnityEngine;

namespace Channels
{ 
    [CreateAssetMenu(fileName = "New Monster Action Channel", menuName = "Monster/Monster Action Channel")]
    public class MonsterActionChannel : BaseChannel
    {
        public Action<MONSTER_ACTIONS> OnEventRaised;

        public void RaiseEvent(MONSTER_ACTIONS value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
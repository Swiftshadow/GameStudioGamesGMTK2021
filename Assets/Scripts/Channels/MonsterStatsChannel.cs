using System;

using Monsters;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New Monster Stats Channel", menuName = "Monster/Monster Stats Channel")]
    public class MonsterStatsChannel : BaseChannel
    {
        public Action<MonsterStats> OnEventRaised;
        
        public void RaiseEvent(MonsterStats value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
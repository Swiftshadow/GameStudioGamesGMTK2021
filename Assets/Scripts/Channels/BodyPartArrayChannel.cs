using System;
using Monsters;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New Body Part Array Channel", menuName = "Body/Body Part Array Channel")]
    public class BodyPartArrayChannel : BaseChannel
    {
        public Action<BodyPartBaseSO[]> OnEventRaised;

        public void RaiseEvent(BodyPartBaseSO[] value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
using System;
using BodyParts;
using UnityEngine;

namespace Channels
{
    public struct BodyPartChangeEvent
    {
        public BodyPartBaseSO newPart;
        public PART_LOCATIONS location;
    }
    [CreateAssetMenu(fileName = "New Body Part Channel", menuName = "Body/Body Part Channel")]
    public class BodyPartChannel : BaseChannel
    {
        public Action<BodyPartChangeEvent> OnEventRaised;

        public void RaiseEvent(BodyPartChangeEvent value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
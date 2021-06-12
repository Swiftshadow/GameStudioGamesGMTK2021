using System;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New Int Channel", menuName = "Primitive/Int Channel")]
    public class IntChannel : BaseChannel
    {
        public Action<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
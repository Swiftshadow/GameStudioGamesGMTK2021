using System;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New Void Channel", menuName = "Primitive/Void Channel")]
    public class VoidChannel : BaseChannel
    {
        public Action OnEventRaised;
        
        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
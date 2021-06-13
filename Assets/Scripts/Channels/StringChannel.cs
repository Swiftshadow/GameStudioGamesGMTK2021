using System;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "New String Channel", menuName = "Primitive/String Channel")]
    public class StringChannel : BaseChannel
    {
        public Action<string> OnEventRaised;

        public void RaiseEvent(string value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
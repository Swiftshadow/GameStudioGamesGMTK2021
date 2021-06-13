using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Channels
{
    [CreateAssetMenu(fileName = "New String Channel", menuName = "Primitive/String Channel")]
    public class WinLoseChannel : BaseChannel
    {
        public Action<bool> OnEventWin;
        public Action<bool> OnEventLose;

        public void WinEvent(bool value)
        {
            OnEventWin?.Invoke(value);
               
            
        }

        public void LoseEvent(bool value)
        {
            OnEventLose?.Invoke(value);
            
        }

    }

}

using Channels;
using UnityEngine;

namespace TestScripts
{
    public class TestActionDoScript : MonoBehaviour
    {
        public VoidChannel doActionChannel;

        public void DoMonsterAction()
        {
            doActionChannel.RaiseEvent();
        }
    }
}
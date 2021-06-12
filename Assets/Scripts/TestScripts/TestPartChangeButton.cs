using BodyParts;
using Channels;
using UnityEngine;

namespace TestScripts
{
    public class TestPartChangeButton : MonoBehaviour
    {
        public BodyPartChannel partChangedChannelEvent;

        public BodyPartBaseSO newPart;
        
        public void ChangePart()
        {
            BodyPartChangeEvent changedEvent = new BodyPartChangeEvent();
            changedEvent.location = PART_LOCATIONS.TORSO;
            changedEvent.newPart = newPart;
            
            partChangedChannelEvent.RaiseEvent(changedEvent);
        }
    }
}
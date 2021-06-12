using Monsters;
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
            changedEvent.location = PART_LOCATIONS.BODY;
            changedEvent.newPart = newPart;
            
            partChangedChannelEvent.RaiseEvent(changedEvent);
        }
    }
}
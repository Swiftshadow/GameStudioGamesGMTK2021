using Channels;
using Monsters;
using UnityEngine;

namespace UI
{
    public class PartSelectButon : MonoBehaviour
    {
        [SerializeField] private BodyPartChannel changePlayerPart;
        
        [SerializeField] private IntChannel partChannel;

        [SerializeField] private VoidChannel nextEnemy;

        public BodyPartBaseSO[] unlockableParts;
        private int passedIndex;
        private void OnEnable()
        {
            partChannel.OnEventRaised += SetIndex;
        }

        private void OnDisable()
        {
            partChannel.OnEventRaised -= SetIndex;
        }

        private void SetIndex(int obj)
        {
            Debug.Log("Setting index!");
            passedIndex = obj;
        }

        public void SelectPart()
        {
            Debug.Log("Button Part Select");
            BodyPartChangeEvent change = new BodyPartChangeEvent();
            int indexToGrab = transform.GetChild(0)
                .GetComponent<PartSelectButtonPictures>().indexToGrab - 1;
            change.newPart = unlockableParts[passedIndex + indexToGrab];
            Debug.Log(passedIndex + indexToGrab);
            switch (passedIndex + indexToGrab)
            {
                case 0:
                    change.location = PART_LOCATIONS.EXTRA_1;
                    break;
                case 1:
                    change.location = PART_LOCATIONS.TAIL;
                    break;
                case 2:
                    change.location = PART_LOCATIONS.EXTRA_3;
                    break;
                case 3:
                    change.location = PART_LOCATIONS.EXTRA_2;
                    break;
            }
            
            nextEnemy.RaiseEvent();
            changePlayerPart.RaiseEvent(change);
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
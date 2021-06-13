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
        
        [SerializeField] private int partIndex;
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
            partIndex += obj;
        }

        public void SelectPart()
        {
            BodyPartChangeEvent change = new BodyPartChangeEvent();
            change.newPart = unlockableParts[partIndex];
            switch (partIndex)
            {
                case 0:
                    change.location = PART_LOCATIONS.EXTRA_1;
                    break;
                case 1:
                    change.location = PART_LOCATIONS.TAIL;
                    break;
                default:
                    change.location = PART_LOCATIONS.EXTRA_2;
                    break;
            }
            
            nextEnemy.RaiseEvent();
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
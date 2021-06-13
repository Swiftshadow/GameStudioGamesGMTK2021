using System;
using Channels;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PartSelectButtonPictures : MonoBehaviour
    {
        public int indexToGrab;

        [SerializeField] private IntChannel partChannel;

        private BodyPartBaseSO[] partsList;
        
        private void OnEnable()
        {
            partChannel.OnEventRaised += ShowIcon;
            partsList = transform.parent.GetComponent<PartSelectButon>()
                .unlockableParts;
        }

        private void OnDisable()
        {
            partChannel.OnEventRaised -= ShowIcon;
        }

        private void ShowIcon(int obj)
        {
            Debug.Log("Showing icon!");
            TextMeshProUGUI tmp = gameObject.GetComponent<TextMeshProUGUI>();
            tmp.text = $"<sprite={obj + indexToGrab}>";
            ++indexToGrab;
        }
    }
}
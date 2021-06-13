using System;
using Channels;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PartSelectButtonPictures : MonoBehaviour
    {
        [SerializeField] private int indexToGrab;

        [SerializeField] private IntChannel partChannel;

        private void OnEnable()
        {
            partChannel.OnEventRaised += ShowIcon;
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
        }
    }
}
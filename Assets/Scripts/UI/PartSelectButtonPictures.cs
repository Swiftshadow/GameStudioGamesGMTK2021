using System;
using Channels;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PartSelectButtonPictures : MonoBehaviour
    {
        [SerializeField] private int indexToGrab;

        [SerializeField] private BodyPartArrayChannel partChannel;

        private void OnEnable()
        {
            partChannel.OnEventRaised += ShowIcon;
        }

        private void OnDisable()
        {
            partChannel.OnEventRaised -= ShowIcon;
        }

        private void ShowIcon(BodyPartBaseSO[] obj)
        {
            Debug.Log("Showing icon!");
            TextMeshPro tmp = gameObject.GetComponent<TextMeshPro>();
            TMP_SpriteAsset spriteAsset = new TMP_SpriteAsset();
            spriteAsset.spriteSheet = obj[indexToGrab].sprite.texture;
            tmp.spriteAsset = spriteAsset;
            tmp.text = "<sprite index = 0>";
        }
    }
}
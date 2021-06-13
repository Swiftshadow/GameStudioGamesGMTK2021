using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Channels;
using UI;

public class HoverBehaviour : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    private BodyPartBaseSO[] partsList;

    [SerializeField] private IntChannel partChannel;

    private int partIndex;
    //public List<string> all;
    // Start is called before the first frame update

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
        partIndex = obj;
    }

    void Start()
    {
        //all = new List<string>();
        partsList = transform.GetChild(0).GetChild(0).gameObject
            .GetComponent<PartSelectButon>().unlockableParts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void One()
    {
        dialogueText.text = partsList[partIndex].description;
    }

    public void Two()
    {
        dialogueText.text = partsList[partIndex + 1].description;
    }

    public void ExitTheHover()
    {
        dialogueText.text = "Default";
    }
}

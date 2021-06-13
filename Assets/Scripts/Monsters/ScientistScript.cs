using System;
using System.Collections;
using System.Collections.Generic;
using Channels;
using UnityEngine;

public class ScientistScript : MonoBehaviour
{

    [SerializeField] private VoidChannel doScientistAction;

    [SerializeField] private IntChannel damageReceived;
    
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        doScientistAction.OnEventRaised += Bounce;
    }

    private void OnDisable()
    {
        doScientistAction.OnEventRaised -= Bounce;
    }

    private void Bounce()
    {
        anim.SetTrigger("Buff");
    }
}

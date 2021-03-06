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
        damageReceived.OnEventRaised += Damaged;
    }

    private void OnDisable()
    {
        doScientistAction.OnEventRaised -= Bounce;
        damageReceived.OnEventRaised += Damaged;
    }

    private void Damaged(int obj)
    {
        anim.SetTrigger("Damaged");
    }

    private void Bounce()
    {
        anim.SetTrigger("Buff");
    }
}

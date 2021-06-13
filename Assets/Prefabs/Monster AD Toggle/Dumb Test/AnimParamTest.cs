using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimParamTest : MonoBehaviour
{

    Animator animTest;

    // Start is called before the first frame update
    void Start()
    {
        animTest = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animTest.SetTrigger("ToggleOn");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animTest.SetTrigger("ToggleOff");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBehaviour : MonoBehaviour
{
    [System.Obsolete]
    void Start()
    {
        Application.LoadLevelAdditive(4);
    }
}

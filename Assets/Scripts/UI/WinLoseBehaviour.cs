using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadBattle()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

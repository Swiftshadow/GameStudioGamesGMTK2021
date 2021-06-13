using Channels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseBehaviour : MonoBehaviour
{
    public VoidChannel dieEvent;
    public VoidChannel winEvent;

    private void OnEnable()
    {
        dieEvent.OnEventRaised += LoadLose;
        winEvent.OnEventRaised += LoadWin;
    }

    void LoadLose()
    {
        SceneManager.LoadScene(5, LoadSceneMode.Single);
    }
    void LoadWin()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Single);
    }

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

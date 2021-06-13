using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class HoverBehaviour : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    //public List<string> all;
    // Start is called before the first frame update
    void Start()
    {
        //all = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void One()
    {
        dialogueText.text = "Optopn 1";
    }

    public void Two()
    {
        dialogueText.text = "OPption 2";
    }

    public void ExitTheHover()
    {
        dialogueText.text = "Default";
    }
}

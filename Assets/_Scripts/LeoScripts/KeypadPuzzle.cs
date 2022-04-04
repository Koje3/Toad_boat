using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadPuzzle : MonoBehaviour {
    public TextMeshProUGUI screenText;
    public GameObject objectToActivate;

    public string correctCombination;
    [SerializeField] private string presentCombination = "";

    public bool puzzleIsFinished = false;
    void Start() {
        presentCombination = "";
        screenText.text = "0000";
    }
    private void Update()
    {
        if (puzzleIsFinished == true)
        {
            CodeComplete();
        }
    }
    public void ButtonWasPressed(int keyNumber) {
        screenText.color = Color.black;
        presentCombination += keyNumber;
        screenText.text = presentCombination;

        if (presentCombination.Length == correctCombination.Length)
        {
            if (presentCombination == correctCombination)
            {
                CodeComplete();                
            }
            else
            {
                screenText.color = Color.red;
                ResetKeypad();
            }
        }        
    }
    void ResetKeypad()
    {
        Start();
    }

    void CodeComplete()
    {
        screenText.color = Color.green;
        puzzleIsFinished = true;
        screenText.text = "OPEN";
        ActivateObject();
    }

    void ActivateObject()
    {
        objectToActivate.GetComponent<Animator>().Play("open");
        objectToActivate.GetComponent<AudioSource>().enabled = true;
    }
}

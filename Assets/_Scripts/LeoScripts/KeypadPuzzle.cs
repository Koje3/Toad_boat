using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadPuzzle : MonoBehaviour {
    public TextMeshProUGUI screenText;

    public string correctCombination;
    [SerializeField] private string presentCombination = "";

    public bool puzzleIsFinished = false;
    void Start() {
        presentCombination = "";
        screenText.text = "0000";
    }
    public void ButtonWasPressed(int keyNumber) {
        screenText.color = Color.black;
        presentCombination += keyNumber;
        screenText.text = presentCombination;

        if (presentCombination.Length == correctCombination.Length) {
            if (presentCombination == correctCombination) {
                print("correct combo!");
                screenText.color = Color.green;
                puzzleIsFinished = true;
            } else {
                print("bad combo!");
                screenText.color = Color.red;
                ResetKeypad();
            }
        }

        void ResetKeypad() {
            Start();
        }
    }
}

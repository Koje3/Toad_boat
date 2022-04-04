using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour {
    public int keyValue;
    [SerializeField] private KeypadPuzzle keypadCode;

    private void Start() {
        keypadCode = GetComponentInParent<KeypadPuzzle>();
    }
    public void ButtonPressed() {
        if (keypadCode.puzzleIsFinished == false) {
            keypadCode.ButtonWasPressed(keyValue);
        }
    }
}

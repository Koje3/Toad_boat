using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CoffeePuzzle : MonoBehaviour
{
    private void Start()
    {
        StartPuzzle(FindObjectOfType<MainGameManager>());
    }

    public static void StartPuzzle(MainGameManager gameManager)
    {
        gameManager.GetComponent<Volume>().profile.components[3].active = true;
    }
}

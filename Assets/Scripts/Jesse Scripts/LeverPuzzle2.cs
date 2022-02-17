using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class LeverPuzzle2 : MonoBehaviour
{
    public Material leverIncorrect;
    public Material leverCorrect;

    public List<LeverInfo> levers;


    // Start is called before the first frame update
    void Start()
    {
        foreach (LeverInfo item in levers)
        {            
            item.lever.onLeverChange.AddListener(LeverValueChanged);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public void StartPuzzle()
    {

    }

    public bool CheckAreLeversSolved()
    {
        foreach (LeverInfo item in levers)
        {
            if (item.currentLeverValue != item.solvedValue)
            {
                Debug.Log("One or more levers are incorrect position");
                Debug.Log( item.lever.gameObject.transform.parent.name + " is incorrect position");

                return false;
            }
        }

        Debug.Log("All levers are in correct position!");

        return true;
    }


    public void LeverValueChanged(float leverValue)
    {


            foreach (LeverInfo item in levers)
            {
                item.currentLeverValue = item.lever.LeverPercentage;

                if (item.currentLeverValue == item.solvedValue)
                {
                    if (item.indicator != null)
                        item.indicator.GetComponent<Renderer>().material = leverCorrect;

                    if (item.leverCorrectAction != null)
                        item.leverCorrectAction.Invoke();

                }
                else
                {
                    if (item.indicator != null)
                        item.indicator.GetComponent<Renderer>().material = leverIncorrect;

                    if (item.leverInCorrectAction != null)
                        item.leverInCorrectAction.Invoke();
                }
            }


    }

    public void CheckIsPuzzleSolved()
    {
        if (CheckAreLeversSolved() == true)
        {
            Debug.Log("Puzzle is solved!");
        }
    }

}

[Serializable]
public class LeverInfo
{
    [Header("Select object with 'Lever' -script component")]
    [Space (2)]
    public BNG.Lever lever;
    [Space (5)]
    public float solvedValue;
    [Space(5)]
    public float currentLeverValue;
    [Space(5)]
    public GameObject indicator;


    [Header("When lever is on correct position what happens?")]
    public UnityEvent leverCorrectAction;

    [Header("When lever is on incorrect position what happens?")]
    public UnityEvent leverInCorrectAction;

}

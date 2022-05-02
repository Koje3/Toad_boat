using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ValvePuzzle : MonoBehaviour
{
    [Header("Buffer Value +/-")]
    public float buffer = 10f;

    //public BNG.Button button;

    public List<ValveInfo> valves;
    public Material valveFalse;
    public Material valveCorrect;

    [Header("Debugging")]
    public uint solvedAmount;
    private bool[] solved;
    public bool isActive = false;
    public bool isFixed = false;
    

    void Start()
    {    
        foreach (var item in valves)
        {
            // Start listening to value changes and initialize puzzle
            item.steeringWheel.onAngleChange.AddListener(WheelValueChanged);
            item.indicator = item.steeringWheel.transform.Find("Indicator").GetComponent<Renderer>();
            item.indicator.material = valveCorrect;
            item.valveScript = item.steeringWheel.GetComponent<ValveInteractable>();
        }

        solved = new bool[valves.Count];
    }

    public void StartPuzzle()
    {
        isActive = true;
        isFixed = false;

        ValveInfo item;

        for (int i = 0; i < valves.Count; i++)
        {
            item = valves[i];

            item.indicator.material = valveFalse;

            if (item.wheelInCorrectAction != null)
                item.wheelInCorrectAction.Invoke();

            item.valveScript.BreakValve();

            solved[i] = false;
        }

        solvedAmount = 0;

        Debug.Log("Valve Puzzle Started");
    }

    public void CheckIsPuzzleSolved()
    {
        if (CheckAreValvesSolved() == true)
        {
            isActive = false;

            isFixed = true;

            Debug.Log("Valve Puzzle Solved");

            //button.buttonActive = true;

            // puzzleObject.FixCrisis(crisisTypeToSolve);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            ResetPuzzle();

        if(solvedAmount >= valves.Count)
            CheckIsPuzzleSolved();
    }

    public bool CheckAreValvesSolved()
    {
        foreach (ValveInfo item in valves)
        {

            if(item.currentValue < item.solvedValue - buffer || item.currentValue > item.solvedValue + buffer) // buffer puuttuu
            {
                Debug.Log("One or more Valve are in incorrect position/s");
                Debug.Log( item.steeringWheel.gameObject.transform.name + " is incorrect position");
       
                return false;
            }  
        }

         Debug.Log("All Valves are in correct position!");

         return true;
    }

    public void WheelValueChanged(float wheelValue)
    {
        ValveInfo item;

        for (int i = 0; i < valves.Count; i++)
        {
            item = valves[i];

            item.currentValue = item.steeringWheel.Angle;

            if (isActive)
            {
                if (item.currentValue > item.solvedValue - buffer && item.currentValue < item.solvedValue + buffer)
                {
                    item.indicator.material = valveCorrect;

                    solved[i] = true;

                    if (item.wheelCorrectAction != null)
                        item.wheelCorrectAction.Invoke();

                    item.valveScript.FixValve();
                }
                else
                {
                    item.indicator.material = valveFalse;

                    solved[i] = false;

                    if (item.wheelInCorrectAction != null)
                        item.wheelInCorrectAction.Invoke();

                    item.valveScript.BreakValve();
                }

                // Map value to 0.25 - 1 range and scale the steam with it
                item.valveScript.ScaleEffectsOnValue(Mathf.Abs(Mathf.Abs(item.currentValue) / Mathf.Abs(item.solvedValue) - 1.00f) * .75f + .25f);
            }
            // TODO Make all the valves break after tempered after fix
            // If user is messing around with valve -> start the puzzle
            else if (!isFixed && (Mathf.Abs(item.currentValue) / Mathf.Abs(item.solvedValue)) > 0.25f)
            {
                Debug.Log("Valve messed with before start");
                
                StartPuzzle();
            }
            // If user is messing around with valve -> start the puzzle
            else if (isFixed && (Mathf.Abs(item.currentValue) / Mathf.Abs(item.solvedValue)) < 0.75f)
            {
                Debug.Log("Valve messed with after fix");

                ResetPuzzle();

                StartPuzzle();

                return;
            }
        }

        // Update solved amount after changes to array holding states (bool[] solved)
        UpdateSolvedAmount();
    }

    private void UpdateSolvedAmount()
    {
        solvedAmount = 0;
        for (int i = 0; i < solved.Length; i++)
        {
            if (solved[i])
                solvedAmount++;
        }
    }

    public void ResetPuzzle()
    {
        foreach (var item in valves)
        {
            //item.solvedValue = UnityEngine.Random.Range(item.steeringWheel.MinAngle,item.steeringWheel.MaxAngle); // valitsee uuden random arvon

            item.valveScript.ResetValveRotation();
        }

        Debug.Log("Valve Puzzle Reset");
    }
}



[Serializable]
public class ValveInfo
{
    [Tooltip("GameObject to listen")]
    public BNG.SteeringWheel steeringWheel;
    [HideInInspector]
    public ValveInteractable valveScript;
    [HideInInspector]
    public Renderer indicator;
    [Space(2)]
    [Header("Current Value")]
    public float currentValue;
    [Header("Solved Value")]
    public float solvedValue;

    [Header("When wheel is on correct position what happens?")]
    public UnityEvent wheelCorrectAction;

    [Header("When wheel is on incorrect position what happens?")]
    public UnityEvent wheelInCorrectAction;
}
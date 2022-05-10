using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EngineValvePuzzle : MonoBehaviour
{
    [Header("Buffer Value +/-")]
    public float buffer = 10f;
    public List<ValveInfo> valves;
    public Material valveIndicatorFalse;
    public Material valveIndicatorCorrect;

    [SerializeField]
    private PuzzleComponentManager puzzleObject;
    [SerializeField]
    private CrisisSubType crisisTypeToSolve;
    public UnityEvent onStartEarly;

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
            item.steeringWheel.onAngleChange.AddListener(ValveAngleChanged);
            item.indicator = item.steeringWheel.transform.Find("Indicator").GetComponent<Renderer>();
            item.indicator.material = valveIndicatorCorrect;
            item.valveScript = item.steeringWheel.GetComponent<ValveInteractable>();
        }

        solved = new bool[valves.Count];
    }

    void Update()
    {
        if (!isFixed && solvedAmount >= valves.Count)
            CheckIsPuzzleSolved();
    }

    public void StartPuzzle()
    {
        isActive = true;
        isFixed = false;

        solvedAmount = 0;

        ValveAngleChanged(0);

        Debug.Log("Valve Puzzle Started");
    }

    public void CheckIsPuzzleSolved()
    {
        if (CheckAreValvesSolved() == true)
        {
            PuzzleSolved();
        }
    }

    private void PuzzleSolved()
    {
        isActive = false;

        isFixed = true;

        puzzleObject.FixCrisis(crisisTypeToSolve);

        Debug.Log("Valve Puzzle Solved");
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

    public void ValveAngleChanged(float wheelValue)
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
                    item.indicator.material = valveIndicatorCorrect;

                    solved[i] = true;

                    if (item.wheelCorrectAction != null)
                        item.wheelCorrectAction.Invoke();

                    item.valveScript.FixValve();
                }
                else
                {
                    item.indicator.material = valveIndicatorFalse;

                    solved[i] = false;

                    if (item.wheelInCorrectAction != null)
                        item.wheelInCorrectAction.Invoke();

                    item.valveScript.BreakValve();
                }

                // Map value to 0.25 - 1 range and scale the steam with it
                item.valveScript.ScaleEffectsOnValue(item.currentValue, item.solvedValue, 0.75f);
            }
            // TODO Make all the valves break after tempered after fix
            // If user is messing around with valve -> start the puzzle
            else if (!isFixed && (Mathf.Abs(item.currentValue) / Mathf.Abs(item.solvedValue)) > 0.25f)
            {
                puzzleObject.StartCrisis(puzzleObject.GetCrisisTypeWithSubType(crisisTypeToSolve));

                onStartEarly.Invoke();

                Debug.Log("Valve Puzzle started early");
            }
            // If user is messing around with valve after fix -> start the puzzle
            else if (isFixed && !item.valveScript.wasTamperedAfterFix && (Mathf.Abs(item.currentValue) / Mathf.Abs(item.solvedValue)) < 0.75f)
            {
                StartCoroutine(TamperedAfterFixEffect(item));

                Debug.Log("Valve was tampered after fix");
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

    private IEnumerator TamperedAfterFixEffect(ValveInfo valve)
    {
        valve.valveScript.BreakValve();

        valve.indicator.material = valveIndicatorFalse;

        LevelManager.instance.ChangeShipSpeed(0);

        float duration = 10, time = 0;

        while(time < duration)
        {
            valve.valveScript.ScaleEffectsOnValue(time, duration, 1f);

            time += Time.deltaTime;

            yield return null;
        }

        valve.valveScript.FixValve();

        LevelManager.instance.ChangeShipSpeed(8);

        valve.valveScript.wasTamperedAfterFix = true;
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
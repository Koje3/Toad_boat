using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using BNG;

public class LeverPuzzle2 : BNG.GrabbableEvents
{
    public Material indicatorMaterialIncorrect;
    public Material indicatorMaterialCorrect;

    public PuzzleComponentManager puzzleComponentManager;
    public CrisisSubType crisisTypeToSolve;

    public bool puzzleOpen = false;

    //Audio
    public AudioClip leverCorrectSound;
    public float leverSoundVol;

    public List<LeverInfo> levers;

    void Start()
    {
        foreach (LeverInfo item in levers)
        {
            item.lever.onLeverChange.AddListener(LeverValueChanged);
            item.indicator = item.lever.gameObject.transform.parent.Find("LeverIndicator").gameObject;
            item.solvedValue = UnityEngine.Random.Range(0f, 100.0f);
        }

    }

    public void StartPuzzle()
    {
        puzzleOpen = true;
    }

    public void CheckIsPuzzleSolved()
    {
        if (CheckAreLeversSolved() == true)
        {
            Debug.Log("Puzzle is solved!");

            puzzleOpen = false;

            puzzleComponentManager.FixCrisis(crisisTypeToSolve);
        }
    }

    public bool CheckAreLeversSolved()
    {
        foreach (LeverInfo item in levers)
        {
            if (item.currentLeverValue < item.solvedValue - 1 || item.currentLeverValue > item.solvedValue + 1)
            {
                Debug.Log("One or more levers are incorrect position");
                Debug.Log(item.lever.gameObject.transform.parent.name + " is incorrect position");

                return false;
            }
        }

        Debug.Log("All levers are in correct position!");

        return true;
    }


    public void LeverValueChanged(float leverValue)
    {
        if (puzzleOpen == true)
        {
            foreach (LeverInfo item in levers)
            {
                item.currentLeverValue = item.lever.LeverPercentage;

                if (item.currentLeverValue > item.solvedValue - 1 && item.currentLeverValue < item.solvedValue + 1)
                {
                    if (item.indicator != null)
                        item.indicator.GetComponent<Renderer>().material = indicatorMaterialCorrect;

                    if (item.lever.GetComponentInParent<LeverFeedbackTimer>().timerEndEventTriggered == false)
                    {
                        item.lever.GetComponentInParent<LeverFeedbackTimer>().timerEndEventTriggered = true;

                        item.lever.GetComponentInParent<LeverFeedbackTimer>().timerIsOn = true; //activate current lever's timer

                        if (leverCorrectSound != null)
                        {
                            item.lever.GetComponent<AudioSource>().PlayOneShot(leverCorrectSound, leverSoundVol);

                            BNG.InputBridge.Instance.VibrateController(0.2f, 0.5f, 0.2f, item.lever.GetComponent<Grabbable>().LastGrabbersHand);                           
                        }

                        if (item.leverCorrectAction != null)
                        {                            
                            item.leverCorrectAction.Invoke();                            
                        }
                    }
                }
                else
                {                    
                    if (item.indicator != null)
                        item.indicator.GetComponent<Renderer>().material = indicatorMaterialIncorrect;

                    if (item.lever.GetComponentInParent<LeverFeedbackTimer>().timeRemaining == 0f)
                    {
                        item.lever.GetComponentInParent<LeverFeedbackTimer>().ResetTime();
                        item.lever.GetComponentInParent<LeverFeedbackTimer>().timerEndEventTriggered = false;
                    }                    

                    if (item.leverInCorrectAction != null)
                        item.leverInCorrectAction.Invoke();
                }
            }
        }

    }

    public void ResetPuzzle()
    {

        foreach (LeverInfo item in levers)
        {
            item.lever.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }



}

[Serializable]
public class LeverInfo
{
    [Header("Select object with 'Lever' -script component")]
    [Space(2)]
    public BNG.Lever lever;
    [Space(5)]
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum PuzzleComponent
{
    None,
    Engine,
    Radar,
    Rudder,
    Hull,
}

public enum CrisisSubType
{
    None,
    Overheat,
    Broken,
    BatteryEmpty,
    Failure,
    Explosion,
}


public class EventManager : MonoBehaviour
{
    public static event Action<PuzzleComponent, CrisisSubType, bool> onCrisisStateChange;
 

    [SerializeField]
    public List<Crises> crises;


    void Start()
    {
        foreach (Crises item in crises)
        {
            item.SetVarsiablesFalse();
            Debug.Log("Set false ");


        }
    }

    private void Update()
    {

    }

    //LevelManager updates current Tick every frame
    public void Tick(float tick)
    {
        
        foreach (Crises item in crises)
        {
            item.Tick(tick);
        }
       
    }



    //Shout crisis to start!

    public void SetCrisisState(PuzzleComponent newPuzzleComponent, CrisisSubType newCrisisSubType, bool isCrisisFixed)
    {
        onCrisisStateChange(newPuzzleComponent, newCrisisSubType, isCrisisFixed);

        foreach (Crises item in crises)
        {
            if (item.puzzleComponent == newPuzzleComponent && item.crisisSubType == newCrisisSubType)
            {
                item.isCrisisFixed = isCrisisFixed;
            }
        }

        if (IsPuzzleComponentFixed(newPuzzleComponent))
        {
            Debug.Log(newPuzzleComponent + " is fixed!");
        }
    }



    public bool GetCrisisState(PuzzleComponent newPuzzleComponent, CrisisSubType newCrisisSubType)
    {

        foreach (Crises item in crises)
        {
            if (item.puzzleComponent == newPuzzleComponent && item.crisisSubType == newCrisisSubType)
            {
                return item.isCrisisFixed;
            }
        }

        Debug.LogError("Tried to find crisis that doesn't exit" + newPuzzleComponent + " " + newCrisisSubType);
        return false;
    }



    public bool IsPuzzleComponentFixed(PuzzleComponent checkPuzzleComponent)
    {

        foreach (Crises item in crises)
        {
            if (item.puzzleComponent == checkPuzzleComponent)
            {
                if (item.isCrisisFixed == false)
                {
                    return false;
                }
            }
        }

        return true;
    }



}


/*

[Serializable]
public class EngineCrisis
{
    [Header("Select Engine State")]
    public EngineState engineCrisis;

    [Space(2)]

    [Header("Does ship's speed stop?")]
    public bool shipSpeedStop;

    [Space(2)]

    public bool isCrisisStarted;

    public float startPuzzleTick;
    public float failPuzzleTick;
    public void Tick(float tick)
    {
        if (!isCrisisStarted)
        {
            if (tick > startPuzzleTick)
            {
                LevelManager.instance.currentPiece.changeEngineCrisisState(engineCrisis);
                isCrisisStarted = true;
            }
        }
        else
        {
            if (failPuzzleTick > 0 && tick > failPuzzleTick)
            {
                // enginecrises is failing
            }
        }

    }

}
*/

[Serializable]
public class Crises
{
    [Header("Select Puzzle Component")]
    public PuzzleComponent puzzleComponent;

    [Header("Select crisis type")]
    public CrisisSubType crisisSubType;

    [Space(2)]

    public bool stopShipSpeed;
    public bool isCrisisStarted;
    [Range(0f, 1f)]
    public float startPuzzleTick;
    [Range(0f, 1f)]
    public float failPuzzleTick;

    public bool isCrisisFixed;
    public bool isCrisisEnded;

    public void SetVarsiablesFalse()
    {
        isCrisisFixed = false;
        isCrisisEnded = false;
        isCrisisStarted = false;
    }

    public void Tick(float tick)
    {
        if (!isCrisisStarted)
        {
            if (tick > startPuzzleTick)
            {
                Debug.Log(puzzleComponent + " " + crisisSubType + "Crisis started");

                isCrisisStarted = true;               
                LevelManager.instance.currentPiece.SetCrisisState(puzzleComponent, crisisSubType, false);

            }
        }
        else
        {
            if (failPuzzleTick > 0 && tick > failPuzzleTick && !isCrisisEnded)
            {
                isCrisisEnded = true;

                if (isCrisisFixed)
                {
                    Debug.Log(puzzleComponent + " " + crisisSubType + " crisis PASSED");
                }
                else 
                {                    
                    Debug.Log(puzzleComponent + " " + crisisSubType + " crisis FAILED");
                }               
            }
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

//This script should be put in every level piece, even when there are no specified events happening in that piece

public enum PuzzleComponent
{
    None,
    Engine,
    Radar,
    Rudder,
    Hull,
    Environment,
    Player
}

public enum CrisisSubType
{
    None,
    Overheat,
    Broken,
    BatteryEmpty,
    Failure,
    Explosion,
    Obstacle,
    MultipleObstacles,
    Tired
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
            item.SetVarsiablesStart();
            Debug.Log("Set false ");
        }
    }

    void Update()
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

                if (isCrisisFixed == true)
                {
                    item.stopShipSpeed = false;
                }               
            }
        }

        if (IsPuzzleComponentFixed(newPuzzleComponent))
        {
            Debug.Log(newPuzzleComponent + " is fixed!");

            if (CheckShouldShipMove())
            {
                LevelManager.instance.ChangeShipSpeed(8, 0.5f);
            }

        }
    }

    bool CheckShouldShipMove()
    {
        foreach (Crises item in crises)
        {
            if (item.stopShipSpeed == true && item.isCrisisStarted == true)
            {               
               return false;
            }
        }
        return true;
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

    public bool gameOverAfterFail = false;

    public bool isCrisisFixed;
    public bool isCrisisEnded;

    public UnityEvent crisisFailsEvents;

    public void SetVarsiablesStart()
    {
        isCrisisFixed = true;
        isCrisisEnded = false;
        isCrisisStarted = false;
    }

    public void Tick(float tick)
    {
        if (!isCrisisStarted)
        {
            //start crisis at certain tick
            if (tick > startPuzzleTick && tick <= 1)
            {

                if (stopShipSpeed == true)
                {
                    LevelManager.instance.ChangeShipSpeed(0, 1f);
                }

                isCrisisStarted = true;               
                LevelManager.instance.currentPiece.SetCrisisState(puzzleComponent, crisisSubType, false);

                Debug.Log(puzzleComponent + " " + crisisSubType + "Crisis started");
            }
        }
        else
        {
            //Check if crisis fails or passes at certain tick if it is larger than 0
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

                    if (gameOverAfterFail == true)
                    {
                        crisisFailsEvents.Invoke();
                        MainGameManager.instance.GameOver();
                    }
                }               
            }
        }
    }


}


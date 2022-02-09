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

    public Dictionary<PuzzleComponent, CrisisSubType> crisesDictionary = new Dictionary<PuzzleComponent, CrisisSubType>();
 

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



    public void CheckProgress(PuzzleComponent type, CrisisSubType subType, bool isSolved)
    {
        Debug.Log("Check progress");

        if (crisesDictionary.Count > 0)
        {
            
            if (crisesDictionary.ContainsKey(type) && crisesDictionary.ContainsValue(subType))
            {
                if (!isSolved)
                crisesDictionary.Remove(type, out subType);
            }
            else
            {
                Debug.Log(type + " " + subType +" is not in the dictionary");
            }

           
        }

        Debug.Log("There are " + crisesDictionary.Count + " crises to be solved");

        foreach (var item in crisesDictionary)
        {
            Debug.Log(item);
        }
        
    }



    //Shout crisis to start!

    public void SetCrisisState(PuzzleComponent newShipComponent, CrisisSubType newCrisisSubType, bool isCrisisFixed)
    {

        onCrisisStateChange(newShipComponent, newCrisisSubType, isCrisisFixed);


        foreach (Crises item in crises)
        {
            if (item.PuzzleComponent == newShipComponent && item.crisisSubType == newCrisisSubType)
            {
                item.isCrisisFixed = isCrisisFixed;
            }
        }
    }

    public bool GetCrisisState(PuzzleComponent newShipComponent, CrisisSubType newCrisisSubType)
    {

        foreach (Crises item in crises)
        {
            if (item.PuzzleComponent == newShipComponent && item.crisisSubType == newCrisisSubType)
            {
                return item.isCrisisFixed;
            }
        }

        Debug.LogError("Tried to find crisis that doesn't exit" + newShipComponent + " " + newCrisisSubType);
        return false;
    }

    public bool IsShipComponentFixed(PuzzleComponent checkCrisisType)
    {

        foreach (Crises item in crises)
        {
            if (item.PuzzleComponent == checkCrisisType)
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
    public PuzzleComponent PuzzleComponent;

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
    public bool isCrisisFailed;

    public void SetVarsiablesFalse()
    {
        isCrisisFixed = false;
        isCrisisFailed = false;
        isCrisisStarted = false;
    }

    public void Tick(float tick)
    {
        if (!isCrisisStarted)
        {
            if (tick > startPuzzleTick)
            {
                Debug.Log("Puzzle started");
                isCrisisStarted = true;
                
                LevelManager.instance.currentPiece.SetCrisisState(PuzzleComponent, crisisSubType, false);

            }
        }
        else
        {
            if (failPuzzleTick > 0 && tick > failPuzzleTick && !isCrisisFailed)
            {
                isCrisisFailed = true;
                Debug.Log("Puzzle FAILED");
                
            }
        }

    }
}


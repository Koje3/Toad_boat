using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CrisisType
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
    public static event Action<CrisisType, CrisisSubType> onCrisisStart;

    public Dictionary<CrisisType, CrisisSubType> crisesDictionary;
 

    [SerializeField]
    public List<Crises> Crises;


    void Start()
    {
        foreach (Crises item in Crises)
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
        
        foreach (Crises item in Crises)
        {
            item.Tick(tick);
        }
       
    }



    public void CheckProgress(CrisisType type, CrisisSubType subType, bool isSolved)
    {
        Debug.Log("Check progress");

        if (crisesDictionary.Count > 0)
        {
            
            if (crisesDictionary.ContainsKey(type) && crisesDictionary.ContainsValue(subType) && isSolved)
            {
                crisesDictionary.Remove(type, out subType);
            }
            else
            {
                Debug.Log(type + " " + subType +" is not solved");
            }

            Debug.Log("There are " + crisesDictionary.Count + " crises to be solved");
        }

    }



    //Shout crisis to start!

    public void StartCrisis(CrisisType crisisType, CrisisSubType crisisSubType)
    {
        onCrisisStart(crisisType, crisisSubType);

        foreach (Crises item in Crises)
        {
            crisesDictionary.Add(item.crisisType, item.crisisSubType);
        }
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
    [Header("Select crisis MAIN type")]
    public CrisisType crisisType;

    [Header("Select crisis SUB type")]
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
                isCrisisFixed = false;
                LevelManager.instance.currentPiece.StartCrisis(crisisType, crisisSubType);

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


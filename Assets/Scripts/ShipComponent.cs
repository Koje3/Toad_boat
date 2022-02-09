using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShipComponent : MonoBehaviour
{

    /*
    /Create dictionary for possible engine problems
    public Dictionary<CrisisSubType, bool> engineProblemStates = new Dictionary<CrisisSubType, bool>()
        {
            { CrisisSubType.Overheat, false },
            { CrisisSubType.BatteryEmpty, false },
            { CrisisSubType.Failure, false },
        };

    */

    public PuzzleComponent puzzleComponent;


    void Start()
    {
        //subscribe to EventManager's static action and use it to execute GetCrisisStateChange function
        EventManager.onCrisisStateChange += GetCrisisStateChange;

    }




    public void GetCrisisStateChange(PuzzleComponent crisis, CrisisSubType newCrisisType, bool isCrisisFixed)
    {
        if (crisis == puzzleComponent)
        {

            // Change vfxState = CrisisSubType
        }

    }

    public void FixCrisis(CrisisSubType crisisSubType)
    {

        LevelManager.instance.currentPiece.SetCrisisState(puzzleComponent, crisisSubType, true);
        
    }

    public void GetCrisisStates(CrisisSubType crisisSubType)
    {
        if (LevelManager.instance.currentPiece.GetCrisisState(puzzleComponent, crisisSubType))
        {

            Debug.Log(crisisSubType + " is fixed");
        }
        else
        {

            Debug.Log(crisisSubType + " is not fixed");
        }
    }

    /*
    public void CheckEngineProblems()
    {
        foreach (KeyValuePair<CrisisSubType, bool> pair in engineProblemStates)
        {
            if (LevelManager.instance.currentPiece != null)
            {
                LevelManager.instance.currentPiece.CheckProgress(PuzzleComponent.Engine, pair.Key, pair.Value);

                //Debug.Log("Engine problem: " + pair.Key + " status: " + pair.Value);
            }
        }

    }

    */
}


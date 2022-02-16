using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;



public class ShipComponent : MonoBehaviour
{

    public PuzzleComponent puzzleComponent;

    public List<CrisisTypes> crisisTypes;


    void Start()
    {
        //subscribe to EventManager's static action and use it to execute GetCrisisStateChange function
        EventManager.onCrisisStateChange += GetCrisisStateChange;

    }



    //Get notice from current EventManager that crisis state has changed
    public void GetCrisisStateChange(PuzzleComponent crisisComponent, CrisisSubType newCrisisType, bool isCrisisFixed)
    {
        //Check that crisis is related to this gameobject
        if (crisisComponent == puzzleComponent)
        {

            //Check that there are actions assigned for this crisistype
            foreach (CrisisTypes crisisType in crisisTypes)
            {
                if (crisisType.crisisType == newCrisisType)
                {
                    // Start or Stop actions related to this crisis
                    if (isCrisisFixed == false)
                    {
                        StartCrisis(crisisType);
                    }
                    else
                    {
                        StopCrisis(crisisType);
                    }
                }
            }

        }

    }

    //Send Event manager info that crisis is fixed
    public void FixCrisis(CrisisSubType crisisSubType)
    {

        LevelManager.instance.currentPiece.SetCrisisState(puzzleComponent, crisisSubType, true);
        
    }


    // Ask which crises are set in current piece and which are fixed
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


    // Activate crisis actions
    public void StartCrisis(CrisisTypes startCrisisType)
    {
        startCrisisType.onCrisisStartEvent.Invoke();
    }

    // Stop crisis actions
    public void StopCrisis(CrisisTypes stopCrisisType)
    {
        stopCrisisType.onCrisisStopEvent.Invoke();
        
    }

    

}

[Serializable]
public class CrisisTypes
{
    [Header("Select crisis type")]
    public CrisisSubType crisisType;

    public UnityEvent onCrisisStartEvent;
    public UnityEvent onCrisisStopEvent;
}



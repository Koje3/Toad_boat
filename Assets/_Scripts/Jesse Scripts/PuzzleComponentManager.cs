using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;



public class PuzzleComponentManager : MonoBehaviour
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
                        StartCoroutine(StopCrisis(crisisType));
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

    /// <returns>First CrisisType with given CrisisSubType.</returns>
    public CrisisTypes GetCrisisTypeWithSubType(CrisisSubType crisisSubType)
    {
        for (int i = 0; i < crisisTypes.Count; i++)
        {
            if(crisisTypes[i].crisisType == crisisSubType)
            {
                return crisisTypes[i];
            }
        }

        return null;
    }

    // Stop crisis actions
    public IEnumerator StopCrisis(CrisisTypes stopCrisisType)
    {
        yield return new WaitForSeconds(stopCrisisType.delayStopCrisisSeconds);

        stopCrisisType.onCrisisStopEvent.Invoke();       
    }
    

}

[Serializable]
public class CrisisTypes
{
    [Header("Select crisis type")]
    public CrisisSubType crisisType;

    public UnityEvent onCrisisStartEvent;

    public float delayStopCrisisSeconds = 0;
    public UnityEvent onCrisisStopEvent;
}



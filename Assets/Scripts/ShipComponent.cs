using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShipComponent : MonoBehaviour
{

    public PuzzleComponent puzzleComponent;

    public List<CrisisSubType> crisisSubTypes;


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
            // vfxstateChange activates objects, particle effects, buttons, levers etc...

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

}


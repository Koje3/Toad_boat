using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Engine : MonoBehaviour
{

    //Create dictionary for possible engine problems
    public Dictionary<CrisisSubType, bool> engineProblemStates = new Dictionary<CrisisSubType, bool>()
        {
            { CrisisSubType.Overheat, false },
            { CrisisSubType.BatteryEmpty, false },
            { CrisisSubType.Failure, false },
        };


    void Start()
    {
        //subscribe to EventManager's static action and use it to execute setEngineState function
        EventManager.onCrisisStart += setEngineProblem;

    }




    private void setEngineProblem(CrisisType crisis, CrisisSubType newEngineproblem)
    {
        if (crisis == CrisisType.Engine)
        {
            if (engineProblemStates.ContainsKey(newEngineproblem))
            {
                engineProblemStates[newEngineproblem] = true;
            }
        }

        CheckEngineProblems();
    }

    public void CheckEngineProblems()
    {
        foreach (KeyValuePair<CrisisSubType, bool> pair in engineProblemStates)
        {
            if (LevelManager.instance.currentPiece != null)
            {
                LevelManager.instance.currentPiece.CheckProgress(CrisisType.Engine, pair.Key, pair.Value);

                //Debug.Log("Engine problem: " + pair.Key + " status: " + pair.Value);
            }
        }

    }


}


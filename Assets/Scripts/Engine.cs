using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EngineState
{
    OK,
    OverHeat,
    BatteryEmpty,
    Failure,
}

public class Engine : MonoBehaviour
{
    public EngineState engineState;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onEngineStateChanged += ChangeEngineState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ChangeEngineState(EngineState newState)
    {
        engineState = newState;

        if (engineState == EngineState.OK)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (engineState == EngineState.OverHeat)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (engineState == EngineState.BatteryEmpty)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        LevelManager.instance.currentPiece.GetComponent<Crisis>().engineCrisis = newState;
        LevelManager.instance.currentPiece.GetComponent<EventManager>().CheckProgress();
    }
}


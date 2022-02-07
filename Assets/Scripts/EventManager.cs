using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CrisisType
{
    none,
    Engine,
    Rudder,
}

public class EventManager : MonoBehaviour
{

    public float tick;

    [Space (2)]

    public float startPuzzleTick;
    public float checkPuzzleCompletionTick;

    [SerializeField]
    public Crisis[] crisis;

    void Start()
    {
        tick = 0f;
    }

    private void Update()
    {
        Debug.Log(tick);

        StartPuzzle();
    }

    private void StartPuzzle()
    {
        if (tick > startPuzzleTick && tick != 0)
        {
            foreach (Crisis item in crisis)
            {
                if (item.crisisType == CrisisType.Engine)
                {
                    LevelManager.instance.StartCrisis(item.engineCrisis, item.crisisType);

                }

            }
        }
    }

    public void CheckProgress()
    {
        foreach (Crisis item in crisis)
        {
            if (item.engineCrisis == EngineState.OK)
            {

            }
            

        }
    }

}

[Serializable]
public class Crisis
{
    [Header("Select Crisis")]
    public CrisisType crisisType;

    [Space(2)]

    [Header("Select Engine State")]
    public EngineState engineCrisis;

    [Space(2)]

    [Header("Does ship's speed stop?")]
    public bool shipSpeedStop;
}


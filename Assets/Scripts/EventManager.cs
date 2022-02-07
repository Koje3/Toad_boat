using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CrisisTypes
{
    none,
    EngineOverheat,
    RudderBroken,
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
                LevelManager.instance.StartCrisis(item.crisisType);
            }
        }
    }

}

[Serializable]
public class Crisis
{
    public CrisisTypes crisisType;
    public bool shipSpeedStop;
   
}

public class Puzzle2 : EventManager
{

}

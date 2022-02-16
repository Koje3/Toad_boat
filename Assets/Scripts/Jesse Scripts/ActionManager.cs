using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ActionManager : MonoBehaviour
{

    public List<CrisisTypes> crisisTypes;


    public void StartCrisis()
    {
        crisisTypes[1].onCrisisStartEvent.Invoke();
    }

    public void StopCrisis()
    {
        crisisTypes[1].onCrisisStopEvent.Invoke();
    }



}

/*
[Serializable]
public class CrisisTypes
{
    [Header("Select crisis type")]
    public CrisisSubType crisisType;

    public UnityEvent onCrisisStartEvent;
    public UnityEvent onCrisisStopEvent;
}
*/




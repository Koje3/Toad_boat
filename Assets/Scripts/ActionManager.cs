using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{
    public List<crisisObjectsAndActions> crisisObjects ;

    void Start()
    {
        
    }


}

[Serializable]
public class crisisObjectsAndActions
{
    [Header("Select crisis type")]
    public CrisisSubType crisisType;

    [Header("Add Effects")]
    public List<GameObject> effects;
    public bool activateEffects;

    [Space(4)]

    [Header("Add objects")]
    public List<GameObject> objects;
    public bool activateObjects;

    [Space(4)]

    [Header("Add buttons, levers, etc.")]
    public List<GameObject> solverObjects;
    public bool activateSolverObjectsFunctions;
}

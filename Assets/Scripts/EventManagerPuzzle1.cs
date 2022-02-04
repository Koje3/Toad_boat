using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManagerPuzzle1 : MonoBehaviour
{
    public static EventManagerPuzzle1 instance;

    public event Action puzzle1Starts;


    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Ship")
        {
            Destroy(GameObject.Find("Ship"));
        }
    }
    */


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    public static event Action puzzleStarts;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryFunctions : MonoBehaviour
{
    public float dropTime;

    // Start is called before the first frame update
    void Start()
    {
        //refresh last drop time at startup
        GetComponent<BNG.Grabbable>().LastDropTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        dropTime = Time.time - GetComponent<BNG.Grabbable>().LastDropTime;
    }

public void MessageToConsole()

{
Debug.Log("Hello! Can You Hear me?");

}

}

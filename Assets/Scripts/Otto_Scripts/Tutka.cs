using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutka : MonoBehaviour
{

//public GameObject antenni;
public GameObject radar;
public GameObject plate;



  
    void Start()
    {
        plate.transform.SetParent(radar.transform);
        CloseLid();

    }

    
    public void StartPuzzle()
    {
    radar.transform.localPosition = new Vector3(0,0,0);

    }


    public void CloseLid()
    {
    radar.transform.localPosition = new Vector3(0,-1,0);
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        StartPuzzle();
    }
}

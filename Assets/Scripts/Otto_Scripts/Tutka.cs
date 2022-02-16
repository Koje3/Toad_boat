using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutka : MonoBehaviour
{


Transform toimii;

public bool PuzzleRadarSolved = false;

Vector3 radarDownPosition = new Vector3(0,-1,0);
Vector3 radarUpPosition = new Vector3(0,0,0);

public float speed = 2f;

 
    void Start()
    {
      
    toimii = this.gameObject.transform.GetChild(2);
    CloseLid();
      
             
    }

    
    void Update()
    {


        if(Input.GetKeyDown(KeyCode.J))
        {
        StartPuzzle();
        }
    }


    public void StartPuzzle()
    {
     // toimii.transform.localPosition = Vector3.Lerp(radarUpPosition, radarUpPosition, speed * Time.deltaTime);
     toimii.transform.localPosition = radarUpPosition;

    }


    public void CloseLid()
    {

    toimii.transform.localPosition = radarDownPosition;
  //  toimii.transform.localPosition = Vector3.Lerp(radarUpPosition, radarDownPosition, speed * Time.deltaTime);

    }

    public void Solved()
    {
    PuzzleRadarSolved = true;
    Debug.Log("Radar Solved");
    CloseLid();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutka : MonoBehaviour
{

//public GameObject antenni;
public GameObject mastooni;
public GameObject plate;
public bool PuzzleRadarSolved = false;


Vector3 radarDownPosition = new Vector3(0,-1,0);
Vector3 radarUpPosition;

float speed = 0.2f;

 
    void Start()
    {
        plate.transform.SetParent(mastooni.transform);
       Vector3 radarUpPosition = mastooni.transform.localPosition;
       mastooni.transform.localPosition = radarDownPosition;
       
    }

    
    public void StartPuzzle()
    {
      //  mastooni.transform.localPosition = Vector3.Lerp(transform.localPosition, radarUpPosition, speed * Time.deltaTime);
      mastooni.transform.localPosition = radarUpPosition;

    }


    public void CloseLid()
    {

    mastooni.transform.localPosition = radarDownPosition;
  //  mastooni.transform.localPosition = Vector3.Lerp(mastooni.transform.localPosition, radarDownPosition, speed * Time.deltaTime);

    }

    public void Solved()
    {
    PuzzleRadarSolved = true;
    Debug.Log("Radar Solved");
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        StartPuzzle();
    }
}

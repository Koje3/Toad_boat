using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public enum LightPuzzleStates
{
NotActive,
Active,
Done

}

public class LightPuzzleMain : MonoBehaviour

{

public LightPuzzleStates lightPuzzleStates;
public BNG.Button button;
public List<LightPuzzleInfo> lightInfo;  



// StartLightPuzzle
// CheckArePuzzlesSolved();


void Update() 
{
if(lightPuzzleStates == LightPuzzleStates.Active) 
    StartLightPuzzle();
}


void StartLightPuzzle()
{
        lightPuzzleStates = LightPuzzleStates.Active;

        foreach (LightPuzzleInfo item in lightInfo)
        {
        item.ligthPuzzleUnit.GetComponentInChildren<BNG.SnapZone>().enabled = true;
        }


        //Spawn Batteries koodia tähän

}



    void Start()
    {


        lightPuzzleStates = LightPuzzleStates.NotActive;
        button = this.gameObject.transform.GetComponentInChildren<BNG.Button>();

        foreach (LightPuzzleInfo item in lightInfo)
        {
            
            item.isThisoneSolved = item.ligthPuzzleUnit.gameObject.transform.GetComponent<LightPuzzleUnit>().isSolved;
            item.ligthPuzzleUnit.GetComponentInChildren<BNG.SnapZone>().enabled = false;

        }

    }


public void CheckArePuzzlesSolved()
{

      foreach (LightPuzzleInfo item in lightInfo)
        {
            
            item.isThisoneSolved = item.ligthPuzzleUnit.gameObject.transform.GetComponent<LightPuzzleUnit>().isSolved;
         
        }

        if(IsLightOk() == true)
            {

            Debug.Log("PuzzleSolved");
            lightPuzzleStates = LightPuzzleStates.Done;
            button.buttonActive = true;
            // puzzleObject.FixCrisis(crisisTypeToSolve);
            }
            else
            button.buttonActive = false;
             Debug.Log("Puzzle Not Solved");

          


}

public bool IsLightOk()
{

 foreach(LightPuzzleInfo item in lightInfo)
    {
     
    if(item.isThisoneSolved == false)
        {  
        return false;
        }
    }

    return true;

}



}


[Serializable]
public class LightPuzzleInfo
{

[Header("Light PuzzleElent")]
public GameObject ligthPuzzleUnit;


[Header("is Solved")]

public bool isThisoneSolved;

}
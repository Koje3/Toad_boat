using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightPuzzleUnit : MonoBehaviour
{

public GameObject valo;

public bool isSolved = false;

    void Start()
    {
        
    valo = this.gameObject.transform.GetChild(2).gameObject;

    }



    public void TrunLightOn()
    {

    valo.SetActive(true);
    isSolved = true;

    }

}

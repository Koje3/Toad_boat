using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onCrisisStart += CrisisStart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CrisisStart(CrisisTypes crisis)
    {
        if (crisis == CrisisTypes.EngineOverheat)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

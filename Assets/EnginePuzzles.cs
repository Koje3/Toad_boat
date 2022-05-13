using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePuzzles : MonoBehaviour
{

    public bool engineBroken;
    public float helpInterval = 30;
    private int helpIndex;

    // Start is called before the first frame update
    void Start()
    {
        engineBroken = false;
        helpIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEnginePuzzles()
    {
        StartCoroutine(SamHelpVoiceLines());
    }

    IEnumerator SamHelpVoiceLines()
    {
        while (engineBroken == true)
        {
            yield return new WaitForSeconds(helpInterval);

            if (SamController.instance.samBehaviorQueue.Count <= 0)
            {
                helpIndex++;

                SamController.instance.AddSamBehaviorToQueue("CrisisEngineHelp" + helpIndex);
            }


            if (helpIndex > 5)
            {
                break;
            }
        }
    }

    public void AddSamBehavior(string samBehavior)
    {
        SamController.instance.AddSamBehaviorTopOfQueue(samBehavior, true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePuzzles : MonoBehaviour
{

    public bool engineBroken;
    public string samHelpIdtextBase = "CrisisEngineHelp";
    public float samHelpIntervalSeconds = 30;
    public int helpIdtextCount = 3;
    public LevelManager levelManager;

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
        engineBroken = true;

        StartCoroutine(SamHelpVoiceLines());

        // Slow down to give time for engine puzzle
        levelManager.ChangeShipSpeed(4);
    }

    IEnumerator SamHelpVoiceLines()
    {
        while (engineBroken == true)
        {
            yield return new WaitForSeconds(samHelpIntervalSeconds);

            if (SamController.instance.samBehaviorQueue.Count <= 0)
            {
                helpIndex++;

                SamController.instance.AddSamBehaviorToQueue(samHelpIdtextBase + helpIndex);
            }


            if (helpIndex > helpIdtextCount)
            {
                break;
            }
        }
    }

    public void AddSamBehavior(string samBehavior)
    {
        SamController.instance.AddSamBehaviorTopOfQueue(samBehavior, true);
    }

    public void EngineFixed()
    {
        engineBroken = false;

        // Speed back up to normal
        levelManager.ChangeShipSpeed(8);
    }

}

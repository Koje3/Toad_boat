using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePuzzles : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject explosionEffectPrefab;


    public PuzzleComponentManager puzzleComponentManager;
    public CrisisSubType crisisTypeToSolve;

    private GameObject obstacle;
    private bool obstaclePuzzleOpen;

    private float obstacleSpawnZ;

    [Header("MULTIPLE OBSTACLE CONFIG")]
    public int multipleObstacleCount = 3;
    public float obstacleSpacing;
    private bool multipleObstaclePuzzleOpen;


    [Header("SAM HELP")]
    public string puzzleStartIdtext = "CrisisLazer1";
    public string samHelpIdtextBase = "CrisisLazerHelp";
    public string puzzleEndIdtext = "LazerComplete";
    public float samHelpIntervalSeconds = 30;
    public int helpIdtextCount = 5;
    private int helpIndex;

    void Start()
    {
        obstaclePuzzleOpen = false;
        multipleObstaclePuzzleOpen = false;
    }


    public void StartObstaclePuzzle()
    {
        if (obstaclePuzzleOpen == false && multipleObstaclePuzzleOpen == false)
        {
            StartCoroutine(SamHelpVoiceLines());
            SamController.instance.AddSamBehaviorTopOfQueue(puzzleStartIdtext, true);
        }

        obstaclePuzzleOpen = true;

        foreach (Crises crisis in LevelManager.instance.currentPiece.crises)
        {
            if (crisis.crisisSubType == CrisisSubType.Obstacle)
            {
                obstacleSpawnZ = LevelManager.instance.currentPieceLenght * (crisis.failPuzzleTick - crisis.startPuzzleTick);
                obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(-20, 20), -3, obstacleSpawnZ), Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f)));            

                break;
            }
        }     
    }

    public void StartMultipleObstaclePuzzle()
    {
        if (obstaclePuzzleOpen == false && multipleObstaclePuzzleOpen == false)
        {
            StartCoroutine(SamHelpVoiceLines());
            SamController.instance.AddSamBehaviorTopOfQueue(puzzleStartIdtext, true);
        }

        multipleObstaclePuzzleOpen = true;

        foreach (Crises crisis in LevelManager.instance.currentPiece.crises)
        {
            if (crisis.crisisSubType == CrisisSubType.MultipleObstacles)
            {
                obstacleSpawnZ = LevelManager.instance.currentPieceLenght * (crisis.failPuzzleTick - crisis.startPuzzleTick);

                for (int i = 0; i < multipleObstacleCount; i++)
                {
                    obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(-20, 20), -3, obstacleSpawnZ - (obstacleSpacing * i * Random.Range(1f, 2f))), Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f)));
                }

                break;
            }
        }
    }

    public void CheckWhatRaycastHit(RaycastHit hit)
    {
        if (obstaclePuzzleOpen == true)
        {
            Debug.Log("Raycast hit " + hit.transform.name);

            if (hit.transform.root.tag == "Obstacle")
            {
                Destroy(hit.transform.root.gameObject);

                GameObject explosionEffect = Instantiate(explosionEffectPrefab, hit.point, Quaternion.identity);
               
                Destroy(explosionEffect, 5);               

                StartCoroutine(CheckIfAllObstaclesAreDestroyed());
            }
        }

        GameObject[] obstaclesRemaining = GameObject.FindGameObjectsWithTag("Obstacle");
        Debug.Log(obstaclesRemaining.Length + " obstacles still remain");
    }

    private IEnumerator CheckIfAllObstaclesAreDestroyed()
    {
        yield return new WaitForSeconds(1);

        GameObject[] obstaclesRemaining = GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstaclesRemaining.Length == 0)
        {
            Debug.Log("All obstacles destroyed");

            EndPuzzle();
        }

        Debug.Log(obstaclesRemaining.Length + " obstacles still remain");
    }


    IEnumerator SamHelpVoiceLines()
    {
        if (obstaclePuzzleOpen == false && multipleObstaclePuzzleOpen == false)
        {
            while (obstaclePuzzleOpen == true || multipleObstaclePuzzleOpen == true)
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

    }

    public void AddSamBehavior(string samBehavior)
    {
        SamController.instance.AddSamBehaviorTopOfQueue(samBehavior, true);
    }


    public void EndPuzzle()
    {
        if (obstaclePuzzleOpen == true)
        {
            puzzleComponentManager.FixCrisis(CrisisSubType.Obstacle);
        }

        if (multipleObstaclePuzzleOpen == true)
        {
            puzzleComponentManager.FixCrisis(CrisisSubType.MultipleObstacles);
        }

        SamController.instance.AddSamBehaviorTopOfQueue(puzzleEndIdtext, true);

        obstaclePuzzleOpen = false;
        multipleObstaclePuzzleOpen = false;        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRocks : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject explosionEffectPrefab;


    public PuzzleComponentManager puzzleObject;
    public CrisisSubType crisisTypeToSolve;

    private GameObject obstacle;
    private bool puzzleOpen;

    // Start is called before the first frame update
    void Start()
    {
        puzzleOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacle != null)
        {
            ScrollObstacles();
        }
    }


    public void StartPuzzle()
    {
        puzzleOpen = true;

        foreach (Crises crisis in LevelManager.instance.currentPiece.crises)
        {
            if (crisis.crisisSubType == CrisisSubType.Rocks)
            {
                float rocksSpawnZ = LevelManager.instance.currentPieceLenght * crisis.failPuzzleTick;
                obstacle = Instantiate(obstaclePrefab, new Vector3(0, 0, rocksSpawnZ - 50f), Quaternion.identity);
            }
        }

        
    }

    public void CheckWhatRaycastHit(RaycastHit hit)
    {
        if (puzzleOpen == true)
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

    public void EndPuzzle()
    {
        puzzleOpen = false;

        puzzleObject.FixCrisis(crisisTypeToSolve);
    }


    public void ScrollObstacles()
    {
        obstacle.transform.Translate(Vector3.back * LevelManager.instance.speedDelta);
       
    }
}

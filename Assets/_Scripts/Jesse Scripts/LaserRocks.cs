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

    public int obstacleCount;
    public float obstacleSpacing;

    public float obstacleSpawnZ;

    void Start()
    {
        puzzleOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartPuzzle()
    {
        puzzleOpen = true;

        // scrollObstacles = true;

        foreach (Crises crisis in LevelManager.instance.currentPiece.crises)
        {
            if (crisis.crisisSubType == CrisisSubType.Obstacle)
            {
                obstacleSpawnZ = LevelManager.instance.currentPieceLenght * (crisis.failPuzzleTick - crisis.startPuzzleTick);
                obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(-20, 20), -3, obstacleSpawnZ), Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f)));            

                break;
            }

            if (crisis.crisisSubType == CrisisSubType.MultipleObstacles)
            {
                obstacleSpawnZ = LevelManager.instance.currentPieceLenght * (crisis.failPuzzleTick - crisis.startPuzzleTick);

                for (int i = 0; i < obstacleCount; i++)
                {
                    obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(-20, 20), -3, obstacleSpawnZ - (obstacleSpacing * i * Random.Range(1f, 2f))), Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f)));
                }

                break;
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


}

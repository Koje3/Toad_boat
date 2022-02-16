using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRocks : MonoBehaviour
{
    public GameObject rockPrefab;
    private GameObject rock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rock != null)
        {
            ScrollRocks();
        }
    }


    public void StartPuzzle()
    {
        foreach (Crises crisis in LevelManager.instance.currentPiece.crises)
        {
            if (crisis.crisisSubType == CrisisSubType.Rocks)
            {
                float rocksSpawnZ = LevelManager.instance.currentPieceLenght * crisis.failPuzzleTick;
                rock = Instantiate(rockPrefab, new Vector3(0, 0, rocksSpawnZ), Quaternion.identity);
            }
        }
        
    }

    public void EndPuzzle()
    {
        rock.transform.GetChild(0).gameObject.SetActive(false);
        rock.transform.GetChild(1).gameObject.SetActive(true);
        Destroy(rock, 5);
    }


    public void ScrollRocks()
    {
        rock.transform.Translate(Vector3.back * LevelManager.instance.speedDelta);
    }
}

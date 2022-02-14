using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

   // public event Action<CrisisType, CrisisSubType> onCrisisStart;

    public GameObject[] level1;
    public GameObject stream1;
    private int levelNumber;


    public GameObject emptyObject;
    private GameObject levelObjectParent;
    private float pieceLenghtSum;

    public float shipSpeed;
    public float speedDelta;

    [SerializeField] private int currentPieceNumber;
    [SerializeField] public EventManager currentPiece;
    [SerializeField] public float currentPieceLenght { get; private set; } 
    [SerializeField] public float currentPieceTravelled { get; private set; }
    [SerializeField] public float levelTravelled { get; private set; }
    [SerializeField] public float currentPieceDistance;
    [SerializeField] private bool levelCleared;

    private void Awake()
    {
        instance = this;       
    }

    void Start()
    {
        currentPieceNumber = 0;
        levelTravelled = 0;
        levelCleared = false;
        MakeLevel(levelNumber);
              
    }


    void Update()
    {
        ScrollEnvironment();
        CalculateCurrentPieceMovement();
        GetCurrentPieceInfo();

        if (currentPiece != null)
            currentPiece.Tick(currentPieceDistance);

    }

    void MakeLevel(int levelNumber)
    {
        levelObjectParent = Instantiate(emptyObject, new Vector3(0, 0, 0), Quaternion.identity);
        pieceLenghtSum = 0;

        for (int i = 0; i < level1.Length; i++)
        {
            float pieceLenght = level1[i].transform.localScale.z;

            GameObject levelPiece = Instantiate(level1[i], new Vector3(0, 0, pieceLenghtSum), Quaternion.identity);
            levelPiece.transform.parent = levelObjectParent.transform;
            level1[i] = levelPiece;
            pieceLenghtSum += pieceLenght;
        }

        GameObject stream = Instantiate(stream1, new Vector3(0, 0, -5), Quaternion.identity);
        stream.transform.parent = levelObjectParent.transform;

    }

    void ScrollEnvironment()
    {
        speedDelta = shipSpeed * Time.deltaTime;

        levelObjectParent.transform.Translate(Vector3.back * speedDelta);
        levelTravelled += speedDelta;

    }


    public void GetCurrentPieceInfo()
    {
        if (levelCleared == false)
        {
            if (currentPiece == null)
            {
                currentPiece = level1[currentPieceNumber].GetComponent<EventManager>();
                if (currentPiece == null)
                {
                    Debug.Log("Couldn't get event manager (currentPieceNumber: " + currentPieceNumber + ")");
                }
                currentPieceLenght = currentPiece.transform.localScale.z;
            }

            else if (currentPieceTravelled > currentPieceLenght)
            {
                currentPieceNumber += 1;
                currentPieceTravelled = 0;
                currentPieceDistance = 0;

                if (currentPieceNumber < level1.Length)
                {
                    // New piece!
                    currentPiece = level1[currentPieceNumber].GetComponent<EventManager>();
                    currentPieceLenght = currentPiece.transform.localScale.z;
                }
                else
                {
                    // Level Cleared!
                    currentPiece = null;
                    levelCleared = true;
                }
            }
        }
                

    }

    public void CalculateCurrentPieceMovement()
    {
        currentPieceTravelled += speedDelta;

        currentPieceDistance = currentPieceTravelled / currentPieceLenght;

    }

    /*
    public void StartCrisis(CrisisType crisisType, CrisisSubType crisisSubType)
    {
        onCrisisStart(crisisType, crisisSubType);
    }
    */
}


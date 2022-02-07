using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public event Action<CrisisType> onCrisisStart;
    public event Action<EngineState> onEngineStateChanged;

    public GameObject[] level1;
    private int levelNumber;


    public GameObject emptyObject;
    private GameObject levelObjectParent;
    private float pieceLenghtSum;

    public float shipSpeed;
    [SerializeField] private float speedDelta;

    [SerializeField] private int currentPieceNumber;
    [SerializeField] public GameObject currentPiece;
    [SerializeField] private float currentPieceLenght;
    [SerializeField] private float currentPieceTravelled;
    [SerializeField] private float levelTravelled;
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
        GetCurrentPieceInfo();
    }

    void MakeLevel(int levelNumber)
    {
        levelObjectParent = Instantiate(emptyObject, new Vector3(0, 0, 0), Quaternion.identity);
        pieceLenghtSum = 0;

        foreach (GameObject piece in level1)
        {
            float pieceLenght = piece.transform.localScale.z;

            GameObject levelPiece = Instantiate(piece, new Vector3(0, 0, pieceLenghtSum), Quaternion.identity);
            levelPiece.transform.parent = levelObjectParent.transform;

            pieceLenghtSum += pieceLenght;
        }           
    }

    void ScrollEnvironment()
    {
        speedDelta = shipSpeed * Time.deltaTime;

        levelObjectParent.transform.Translate(Vector3.back * speedDelta);
        levelTravelled += speedDelta;

    }


    public void GetCurrentPieceInfo()
    {
        if (currentPieceNumber < level1.Length)
        {
            currentPiece = level1[currentPieceNumber];
            currentPieceLenght = currentPiece.transform.localScale.z;
            currentPieceTravelled += speedDelta;

            currentPieceDistance = Mathf.Round(currentPieceTravelled / currentPieceLenght * 100f) * 0.01f;

            if (currentPieceTravelled > currentPieceLenght)
            {
                currentPieceNumber += 1;
                currentPieceTravelled = 0;
            }
            
        }
        else if (currentPieceNumber >= level1.Length)
        {
            currentPiece = null;
            levelCleared = true;
        }

        if (currentPiece != null)
        {
            Debug.Log("piece found");

            if (currentPiece.GetComponent<EventManager>() != null)
            {
                Debug.Log("eventmanager found");

                EventManager currentEventManager = currentPiece.GetComponent<EventManager>();

                Debug.Log(currentEventManager);

                currentPiece.GetComponent<EventManager>().tick = currentPieceDistance;
            }

        }

    }

    public void StartCrisis(EngineState engineState, CrisisType crisisType)
    {
        onCrisisStart(crisisType);
        onEngineStateChanged(engineState);
    }

}

public class CrisisManager
{


    
}

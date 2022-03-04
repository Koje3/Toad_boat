using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject[] level1;
    public GameObject stream1;
    private int levelNumber;


    public GameObject emptyObject;
    private GameObject levelObjectParent;
    private float pieceLenghtSum;

    public float shipSpeed;
    public float shipGoalSpeed = 8;
    public float speedDelta;
    public float deaccelerationSpeed = 0.1f;
    public float accelerationSpeed = 0.1f;

    [SerializeField] private int currentPieceNumber;
    [SerializeField] public EventManager currentPiece;
    [SerializeField] public float currentPieceLenght { get; private set; } 
    [SerializeField] public float currentPieceTravelled { get; private set; }
    [SerializeField] public float levelTravelled { get; private set; }
    [SerializeField] public float currentPieceDistance;
    [SerializeField] private bool levelCleared;

    private bool gameOver;
    private float screenFade = 0f;
    private float screenFadeGoal = 1f;
    private float screenFadeSpeed = 0.1f;

    public Volume volume { get; private set; }
    [ColorUsageAttribute(true, true)]
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        instance = this;

        volume = GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) == false)
        {
            Debug.LogError("ERROR: couldn't get ColorAdjustments from the Volume");
        }
    }

    void Start()
    {
        gameOver = false;

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

        MatchShipGoalSpeed();
        GameOverSequence();

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

    public void ChangeShipSpeed(float newShipSpeed)
    {
        shipGoalSpeed = newShipSpeed;
    }

    //Update shipspeed if the goalshipspeed changes
    void MatchShipGoalSpeed()
    {
        if (shipSpeed > shipGoalSpeed)
        {
            shipSpeed -= deaccelerationSpeed * Time.deltaTime;

            if (shipSpeed <= shipGoalSpeed)
            {
                shipSpeed = shipGoalSpeed;
            }
        }
        else if (shipSpeed < shipGoalSpeed)
        {
            shipSpeed += accelerationSpeed * Time.deltaTime;

            if (shipSpeed >= shipGoalSpeed)
            {
                shipSpeed = shipGoalSpeed;
            }
        }
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

    public void GameOver()
    {
        gameOver = true;        
    }

    void GameOverSequence()
    {    
        if (gameOver == true)
        {
            if (screenFade < screenFadeGoal)
            {
                screenFade += screenFadeSpeed * Time.deltaTime;
                colorAdjustments.postExposure.value = Mathf.Lerp(0f, -15f, screenFade);
            }
            else
            {
                SceneManager.LoadScene(levelNumber);
                gameOver = false;
            }
        }        
    }

}


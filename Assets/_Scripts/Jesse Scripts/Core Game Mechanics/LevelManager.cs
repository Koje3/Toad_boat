using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    //Level building related
    public GameObject[] levelPieces;
    public GameObject[] levelBackPieces;
    public GameObject riverStreamPrefab;
    public int levelNumber = 1;
    public GameObject emptyObject;
    private GameObject levelObjectParent;
    public float pieceLenghtSum;


    //Ship speed related variables
    public float shipSpeed;
    public float shipStartSpeed = 8;
    public float shipGoalSpeed;
    public float speedDelta;
    public float deaccelerationSpeed = 0.1f;
    public float accelerationSpeed = 0.1f;

    //Level info
     public int currentPieceNumber = 0;
     public EventManager currentPiece;
     public float currentPieceLenght { get; private set; } 
     public float currentPieceTravelled { get; private set; }
     public float levelTravelled = 0f;
     public float currentPieceDistance;
    [SerializeField] private bool levelCleared;


    //Level loading and saving related
    public int currentLevel;
    public int loadedPieceNumber;
    public float currentLevelTravelled;
    public bool continueGame;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        continueGame = false;
        shipGoalSpeed = shipStartSpeed;


        levelCleared = false;
        MakeLevel(levelNumber);

        //If there is a SaveFile, loadgame and check if game continues from last spot
        if (CarterGames.Assets.SaveManager.SaveManager.HasSaveFile())
        {
            CarterGames.Assets.SaveManager.SaveLoadController.LoadGame();

            if (continueGame == true)
                ContinueGame();
        }

        CarterGames.Assets.SaveManager.SaveLoadController.SaveGame();
    }


    void Update()
    {
        ScrollEnvironment();
        CalculateCurrentPieceMovement();
        GetCurrentPieceInfo();

        if (currentPiece != null)
            currentPiece.Tick(currentPieceDistance);

        MatchShipGoalSpeed();

    }

    void MakeLevel(int levelNumber)
    {
        levelObjectParent = Instantiate(emptyObject, new Vector3(0, 0, 0), Quaternion.identity);
        pieceLenghtSum = 0f;

        for (int i = 0; i < levelPieces.Length; i++)
        {
            float pieceLenght = levelPieces[i].transform.localScale.z;

            GameObject levelPiece = Instantiate(levelPieces[i], new Vector3(0, 0, pieceLenghtSum), Quaternion.identity);
            levelPiece.transform.parent = levelObjectParent.transform;
            levelPieces[i] = levelPiece;
            pieceLenghtSum += pieceLenght;
        }

        float backPieceLenghtSum = 0f;

        for (int i = 0; i < levelBackPieces.Length; i++)
        {
            float pieceLenght = levelBackPieces[i].transform.localScale.z;
            backPieceLenghtSum += pieceLenght;

            GameObject levelPiece = Instantiate(levelBackPieces[i], new Vector3(0, 0, -backPieceLenghtSum), Quaternion.identity);
            levelPiece.transform.parent = levelObjectParent.transform;
            levelBackPieces[i] = levelPiece;
            
        }

        GameObject stream = Instantiate(riverStreamPrefab, new Vector3(0, 0, -5), Quaternion.identity);
        stream.transform.parent = levelObjectParent.transform;

    }

    //Load last game and transform the level to the last piece the ship was
    void ContinueGame()
    {
        pieceLenghtSum = 0f;

        for (int i = 0; i < loadedPieceNumber; i++)
        {
            float pieceLenght = levelPieces[i].transform.localScale.z;
            pieceLenghtSum += pieceLenght;
        }

        levelTravelled = pieceLenghtSum;
        currentPieceNumber = loadedPieceNumber;
        levelObjectParent.transform.Translate(Vector3.back * levelTravelled);

        shipSpeed = shipGoalSpeed;
    }


    void ScrollEnvironment()
    {
        speedDelta = shipSpeed * Time.deltaTime;

        levelObjectParent.transform.Translate(Vector3.back * speedDelta);
        levelTravelled += speedDelta;

    }

    public void ChangeShipSpeed(float newShipSpeed, float newDeacclerationSpeed = 0.5f, float newAccelerationSpeed = 0.5f)
    {
        shipGoalSpeed = newShipSpeed;
        deaccelerationSpeed = newDeacclerationSpeed;
        accelerationSpeed = newAccelerationSpeed;
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
                currentPiece = levelPieces[currentPieceNumber].GetComponent<EventManager>();
                if (currentPiece == null)
                {
                    Debug.Log("Couldn't get event manager (currentPieceNumber: " + currentPieceNumber + ")");
                }
                currentPieceLenght = currentPiece.transform.localScale.z;
            }

            else if (currentPieceTravelled > currentPieceLenght)
            {
                //If piece changes, Save the game

                currentPieceNumber += 1;
                currentPieceTravelled = 0;
                currentPieceDistance = 0;

                continueGame = true;
                CarterGames.Assets.SaveManager.SaveLoadController.SaveGame();

                if (currentPieceNumber < levelPieces.Length)
                {
                    // New piece!
                    currentPiece = levelPieces[currentPieceNumber].GetComponent<EventManager>();
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

}


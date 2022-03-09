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

    public GameObject[] levelPieces;
    public GameObject[] levelBackPieces;
    public GameObject riverStreamPrefab;
    public int levelNumber = 1;


    public GameObject emptyObject;
    private GameObject levelObjectParent;
    public float pieceLenghtSum;

    public float shipSpeed;
    public float shipStartSpeed = 8;
    public float shipGoalSpeed;
    public float speedDelta;
    public float deaccelerationSpeed = 0.1f;
    public float accelerationSpeed = 0.1f;

     public int currentPieceNumber = 0;
     public EventManager currentPiece;
     public float currentPieceLenght { get; private set; } 
     public float currentPieceTravelled { get; private set; }
     public float levelTravelled = 0f;
     public float currentPieceDistance;
    [SerializeField] private bool levelCleared;
    public bool restart;

    public int loadedLevelNumber;
    public int loadedPieceNumber;
    public float loadedLevelTravelled;

    private bool gameOver;
    private float screenFade;
    private float screenFadeGoal;
    private float screenFadeSpeed;

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
        restart = false;

        shipGoalSpeed = shipStartSpeed;

        //screen fade to black variables set to starting state
        screenFadeGoal = 0f;
        screenFade = 1f;
        screenFadeSpeed = 0.4f;
        colorAdjustments.postExposure.value = -15f;

        levelCleared = false;
        MakeLevel(levelNumber);

        ContinueAfterRestart();

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
        ScreenFadeTowardsFadeGoal();

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
    void ContinueAfterRestart()
    {
        CarterGames.Assets.SaveManager.GameManager.instance.LoadGame();
        pieceLenghtSum = 0f;

        if (restart)
        {
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
                currentPiece = levelPieces[currentPieceNumber].GetComponent<EventManager>();
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

                restart = true;
                CarterGames.Assets.SaveManager.GameManager.instance.SaveGame();

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

    public void GameOver()
    {
        gameOver = true;
        screenFadeGoal = 1;
        screenFadeSpeed = 0.2f;
    }

    void GameOverSequence()
    {    
        if (gameOver == true)
        {
            if (screenFade >= screenFadeGoal)
            {
                gameOver = false;

                CarterGames.Assets.SaveManager.GameManager.instance.LoadGame();
                SceneManager.LoadScene(loadedLevelNumber);                
            }
        }        
    }

    void GameStartSequence()
    {

    }

    //Lerps screenFade value towards ScreenFadeGoal if it changes
    void ScreenFadeTowardsFadeGoal()
    {        
        if (screenFade > screenFadeGoal)
        {
            screenFade -= screenFadeSpeed * Time.deltaTime;

            if (screenFade <= screenFadeGoal)
            {
                screenFade = screenFadeGoal;
            }

            colorAdjustments.postExposure.value = Mathf.Lerp(0f, -15f, screenFade);
        }
        else if (screenFade < screenFadeGoal)
        {
            screenFade += screenFadeSpeed * Time.deltaTime;

            if (screenFade >= screenFadeGoal)
            {
                screenFade = screenFadeGoal;
            }

            colorAdjustments.postExposure.value = Mathf.Lerp(0f, -15f, screenFade);
        }
    }

}


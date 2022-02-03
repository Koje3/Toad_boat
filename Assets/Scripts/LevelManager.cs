using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject[] level1;
    private int levelNumber;


    public GameObject emptyObject;
    private GameObject levelObjectParent;
    private float pieceLenghtSum;

    public float shipSpeed;

    public int currentPieceNumber;
    public GameObject currentPiece;
    public float currentPieceLenght;
    public float distanceLeftOnPiece;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentPieceNumber = 0;
        MakeLevel(levelNumber);
    }


    void Update()
    {
        ScrollEnvironment();
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
        levelObjectParent.transform.Translate(Vector3.back * shipSpeed * Time.deltaTime);
        distanceLeftOnPiece -= shipSpeed * Time.deltaTime;
    }
    
}

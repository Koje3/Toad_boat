using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public Volume volume { get; private set; }
    [ColorUsageAttribute(true, true)]
    private ColorAdjustments colorAdjustments;

    [SerializeField]
    private GameObject xrRig;
    [SerializeField]
    private BNG.ScreenFader screenFader;

    private bool gameOver;
    private float postExposure;
    private float postExposureGoal;
    private float postExposureSpeed;

    public float gameOverDelay = 1f;



    private void Awake()
    {
        instance = this;

        volume = GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) == false)
        {
            Debug.LogError("ERROR: couldn't get ColorAdjustments from the Volume");
        }
    }

    private void Start()
    {
        //Find XR Rig in the scene
        xrRig = GameObject.Find("XR Rig Advanced");
        if (xrRig == null)
        {
            Debug.LogError("ERROR: couldn't find XR Rig");
        }
        else
        {
            screenFader = xrRig.GetComponentInChildren<BNG.ScreenFader>();
        }

        gameOver = false;

        GameStartSequence();
    }

    private void Update()
    {
        PostExposureTowardsGoal();
    }

    void GameStartSequence()
    {
        /*
        postExposureGoal = 0f;
        postExposure = 1f;
        postExposureSpeed = 0.4f;
        colorAdjustments.postExposure.value = 0f;
        */

        ScreenFadeOut();
    }


    public void GameOver()
    {
        /*
        postExposureGoal = 1;
        postExposureSpeed = 0.2f;
        */
        
        if (gameOver == false)
        {
            StartCoroutine(GameOverSequence());
        }

    }

    public IEnumerator GameOverSequence()
    {
        gameOver = true;
        ScreenFadeIn();

        yield return new WaitForSeconds(gameOverDelay);

        CarterGames.Assets.SaveManager.SaveLoadController.LoadGame();
        SceneManager.LoadScene(LevelManager.instance.currentLevel);

    }

    public void ScreenFadeIn()
    {
        screenFader.DoFadeIn();
    }

    public void ScreenFadeOut()
    {
        screenFader.DoFadeOut();
    }


    //Lerps post exposure value towards postExposureGoal if it changes
    void PostExposureTowardsGoal()
    {
        if (postExposure > postExposureGoal)
        {
            postExposure -= postExposureSpeed * Time.deltaTime;

            if (postExposure <= postExposureGoal)
            {
                postExposure = postExposureGoal;
            }

            colorAdjustments.postExposure.value = Mathf.Lerp(0f, -15f, postExposure);
        }
        else if (postExposure < postExposureGoal)
        {
            postExposure += postExposureSpeed * Time.deltaTime;

            if (postExposure >= postExposureGoal)
            {
                postExposure = postExposureGoal;
            }

            colorAdjustments.postExposure.value = Mathf.Lerp(0f, -15f, postExposure);
        }
    }
}



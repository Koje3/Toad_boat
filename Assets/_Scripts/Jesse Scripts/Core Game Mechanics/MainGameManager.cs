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
    public float showSaveIconTime = 2f;

    [SerializeField]
    private float timer;
    public float timerMaxTime = 5f;

    public GameObject saveIconPrefab;
    private GameObject saveIconObject;
    private string saveIconName = "SaveIcon";
    private bool showSaveIcon = false;



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

        showSaveIcon = false;
        InitializeSaveIcon();
        GameStartSequence();

    }

    private void Update()
    {
        Timer();
        PostExposureTowardsGoal();
        SaveIcon();

        if (xrRig.transform.position.y < -5f)
        {
            GameOver();
        }
    }

    public float Timer()
    {
        timer += Time.deltaTime;

        if (timer > timerMaxTime)
            timer = 0;

        return timer;
    }

    void GameStartSequence()
    {
        gameOver = false;
        StartCoroutine(ScreenFadeOut(1));
    }


    public void GameOver()
    {
        /*
        postExposureGoal = 1;
        postExposureSpeed = 0.2f;
        */
        
        if (gameOver == false)
        {
            gameOver = true;
            StartCoroutine(GameOverSequence());
        }

    }

    public IEnumerator GameOverSequence()
    {
        gameOver = true;
        StartCoroutine(ScreenFadeIn(0));

        yield return new WaitForSeconds(gameOverDelay);

        CarterGames.Assets.SaveManager.SaveLoadController.LoadGame();
        SceneManager.LoadScene(LevelManager.instance.currentLevel);

    }

    //Fade screen from transparent to solid
    public IEnumerator ScreenFadeIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        screenFader.DoFadeIn();
    }

    //Fade screen from solid to transparent
    public IEnumerator ScreenFadeOut(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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


    public void ShowSaveIcon()
    {
        showSaveIcon = true;
        timer = 0;
    }

    public void SaveIcon()
    {
        if (saveIconObject != null && showSaveIcon == true)
        {
            Debug.Log("Show save icon");
            
            if (timer < showSaveIconTime)
            {
                saveIconObject.SetActive(true);
                saveIconObject.transform.Rotate(0, 0, 40 * Time.deltaTime);
            }
            else
            {
                showSaveIcon = false;
                saveIconObject.SetActive(false);
            }
        }
    }

    private void InitializeSaveIcon()
    {
        // Create a Canvas that will be placed directly over the camera
        if (saveIconPrefab != null)
        {
            saveIconObject = Instantiate(saveIconPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            saveIconObject.transform.parent = xrRig.transform.Find("PlayerController/CameraRig/TrackingSpace/CenterEyeAnchor").gameObject.transform;
            saveIconObject.transform.localPosition = new Vector3(0.53f, -0.47f, 0.5f);
            saveIconObject.transform.localEulerAngles = Vector3.zero;
            saveIconObject.transform.name = saveIconName;

            saveIconObject.SetActive(false);

        }
        else
        {
            Debug.Log("Theres no saveIconPrefab set");
        }
    }
}



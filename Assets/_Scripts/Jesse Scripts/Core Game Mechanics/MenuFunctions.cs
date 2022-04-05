using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [Header("Menu canvases")]
    public GameObject startCanvas;
    public GameObject playCanvas;
    public GameObject options;
    public GameObject controls;
    public GameObject credits;

    public float sceneLoadDelay = 3;

    private int startLevelNumber;
    

    void Start()
    {
        playCanvas.SetActive(false);
        options.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(false);

        //If there's save file show continue button
        if (playCanvas != null)
        {
            GameObject continueButton = playCanvas.transform.Find("ContinueButton").gameObject;
            if (continueButton != null)
            {
                if (CarterGames.Assets.SaveManager.SaveManager.HasSaveFile())
                    continueButton.SetActive(true);
                else
                    continueButton.SetActive(false);

            }
        }

    }


    void Update()
    {
        
    }

    public void StartNewGame()
    {
        startLevelNumber = 1;
        CarterGames.Assets.SaveManager.SaveManager.ResetSaveFile();

        StartCoroutine(LoadScene());
    }

    public void ContinueGame()
    {
        var loadData = CarterGames.Assets.SaveManager.SaveManager.LoadGame();

        if (loadData.levelNumber > 0)
            startLevelNumber = loadData.levelNumber;
        else
            startLevelNumber = 1;

        StartCoroutine(LoadScene());
    }

    public void DeleteSaveFile()
    {
        CarterGames.Assets.SaveManager.SaveManager.DeleteSaveFile();
    }

    private IEnumerator LoadScene()
    {
        MainGameManager.instance.ScreenFadeIn();
        yield return new WaitForSeconds(sceneLoadDelay);

        SceneManager.LoadScene(startLevelNumber);
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }
}

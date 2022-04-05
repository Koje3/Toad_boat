using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void StartNewGame()
    {
        CarterGames.Assets.SaveManager.SaveManager.ResetSaveFile();
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        int startLevelNumber = 1;
        var loadData = CarterGames.Assets.SaveManager.SaveManager.LoadGame();


        if (loadData.levelNumber < 1)
            startLevelNumber = loadData.levelNumber;

        SceneManager.LoadScene(startLevelNumber);
    }
}

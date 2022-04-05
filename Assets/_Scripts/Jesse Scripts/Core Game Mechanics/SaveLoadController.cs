using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace CarterGames.Assets.SaveManager
{

    public static class SaveLoadController
    {
        //This is non-monobeaviour Save/Load actuator script. This script doesn't have to and cannot be in gameobject
        //Call the functions writing for example: CarterGames.Assets.SaveManager.SaveLoadController.LoadGame();

        public static void SaveGame()
        {
            // For you this would be ' SaveData ' not ' ExampleSaveData '
            var saveData = new SaveData();

            // Passing in values to be saved
            saveData.levelNumber = LevelManager.instance.levelNumber;
            saveData.pieceNumber = LevelManager.instance.currentPieceNumber;
            saveData.levelTravelled = LevelManager.instance.levelTravelled;
            saveData.restart = LevelManager.instance.restart;


            SaveManager.SaveGame(saveData);
        }

        public static void LoadGame()
        {
            var loadData = SaveManager.LoadGame();


            LevelManager.instance.currentLevel = loadData.levelNumber;
            LevelManager.instance.loadedPieceNumber = loadData.pieceNumber;
            LevelManager.instance.currentLevelTravelled = loadData.levelTravelled;
            LevelManager.instance.restart = loadData.restart;

        }
    }

    public class CustomClass
    {
        [SerializeField] public int shield = 5;
    }


}



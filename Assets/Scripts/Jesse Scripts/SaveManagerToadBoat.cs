using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CarterGames.Assets.SaveManager
{
    public class SaveManagerToadBoat : MonoBehaviour
    {
        public static SaveManagerToadBoat instance;


        private void Awake()
        {
            instance = this;

        }

        public void SaveGame()
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

        public void LoadGame()
        {
            var loadData = SaveManager.LoadGame();


            LevelManager.instance.loadedLevelNumber = loadData.levelNumber;
            LevelManager.instance.loadedPieceNumber = loadData.pieceNumber;
            LevelManager.instance.loadedLevelTravelled = loadData.levelTravelled;
            LevelManager.instance.restart = loadData.restart;

        }
    }

        public class CustomClass
    {
        [SerializeField] public int shield = 5;
    }

}
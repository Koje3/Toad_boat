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
        // Input values
        [Header("Input Fields")]
        public Text playerName;
        public Text playerHealth;
        public Text playerPosition0;
        public Text playerPosition1;
        public Text playerPosition2;
        public Text playerShield;
        public Image playerSprite;

        // An instance of the custom class used in the example
        private CustomClass customClassInt = new CustomClass();

        // output values
        [Header("Output Text Components")]
        public Text displayPlayerName;
        public Text displayPlayerHealth;
        public Text displayPlayerPosition;
        public Text displayPlayerShield;
        public Image displayPlayerSprite;


        public void SaveGame()
        {
            // For you this would be ' SaveData ' not ' ExampleSaveData '
            var saveData = new SaveData();

            // Passing in values to be saved
            saveData.examplePlayerName = playerName.text;
            saveData.examplePlayerHealth = float.Parse(playerHealth.text);

            // Creating a SaveVector3 and setting the Vector3 up before saving it.
            var exampleVec = new Vector3(float.Parse(playerPosition0.text), float.Parse(playerPosition1.text), float.Parse(playerPosition2.text));
            saveData.examplePlayerPosition = exampleVec;

            saveData.examplePlayerSprite = playerSprite.sprite;

            customClassInt.shield = int.Parse(playerShield.text);
            saveData.exampleCustomClass = customClassInt;


            SaveManager.SaveGame(saveData);
        }

        public void LoadGame()
        {
            var loadData = SaveManager.LoadGame();

            displayPlayerName.text = loadData.examplePlayerName;
            displayPlayerHealth.text = loadData.examplePlayerHealth.ToString();
            displayPlayerPosition.text = loadData.examplePlayerPosition.ToString();
            displayPlayerShield.text = loadData.exampleCustomClass.shield.ToString();
            displayPlayerSprite.sprite = loadData.examplePlayerSprite;
        }
    }

        public class CustomClass
    {
        [SerializeField] public int shield = 5;
    }

}
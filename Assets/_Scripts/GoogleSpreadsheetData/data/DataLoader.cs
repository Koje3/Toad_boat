using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataLoader : MonoBehaviour
{
    

    void Awake()
    {
        // Try loading the data TODO: Move this to your game initialization. You only need to do this once.
        try
        {
            GameData.Load();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}

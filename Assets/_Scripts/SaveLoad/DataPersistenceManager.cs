using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private static GameData gameData;
    
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than once Data Persistence Manager in the scene");
        }
        instance = this;
    }

    public static void NewGame()
    {
        gameData = new GameData();
    }

    public static void LoadGame()
    {
        if (gameData == null)
        {
            Debug.Log("No save data found. Initialising to default values");
            NewGame();
        }
    }

    public static void SaveGame()
    {

    }
}
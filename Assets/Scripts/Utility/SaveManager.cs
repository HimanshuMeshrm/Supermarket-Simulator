using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string saveFilePath = Application.persistentDataPath + "/gameSave.json";
    private static GameSave currentGameSave;

    public static void SaveGame(GameSave gameSave)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(gameSave);
            File.WriteAllText(saveFilePath, jsonData);
            currentGameSave = gameSave;
            Debug.Log("Game saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }
    public static void SaveGame()
    {
        if (currentGameSave == null)
        {
            Debug.LogError("No game data to save.");
            return;
        }
        try
        {
            string jsonData = JsonUtility.ToJson(currentGameSave);
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log("Game saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }

    public static GameSave LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string jsonData = File.ReadAllText(saveFilePath);
                currentGameSave = JsonUtility.FromJson<GameSave>(jsonData);
                Debug.Log("Game loaded successfully.");
                return currentGameSave;
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading game: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }

    public static GameSave GetCurrentGameSave()
    {
        if(currentGameSave == null)
        {
            currentGameSave = new GameSave();
        }
        return currentGameSave;
    }
}

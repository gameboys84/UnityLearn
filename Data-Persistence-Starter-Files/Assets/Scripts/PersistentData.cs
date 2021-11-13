using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class RuntimeData
{
    public int curLevel; // between scenes
    public int curScore; // between scenes

    public RuntimeData()
    {
        Clear();
    }
    
    public void Clear()
    {
        curLevel = 1;
        curScore = 0;
    }
}

public class PersistData
{
    public int highLevel;   // highLevel in history
    public int highScore;   // highScore in history
    public int totalScore;  // totalScore between sessions

    public PersistData()
    {
        Clear();
    }

    public void Clear()
    {
        highLevel = 1;
        highScore = 0;
        totalScore = 0;
    }
}

public class PersistentData : MonoBehaviour
{
    private const string Key_PersistData = "PersistData";
    // private const string Key_RuntimeData = "RuntimeData";
    private static PersistentData _instance;
    
    public static PersistentData Instance => _instance;
    public PersistData data;
    public RuntimeData runtimeData;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (_instance == null)
        {
            _instance = GetComponent<PersistentData>();
            runtimeData = new RuntimeData();
        }
        else
        {
            Destroy(this);
            return;
        }

        var strData = PlayerPrefs.GetString(Key_PersistData, string.Empty);
        if (string.IsNullOrEmpty(strData))
        {
            data = new PersistData();
        }
        else
        {
            data = JsonUtility.FromJson<PersistData>(strData);
        }

        
    }

    public void SaveData()
    {
        var strData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key_PersistData, strData);
    }

    public int AddScore(int value)
    {
        runtimeData.curScore += value;
        data.totalScore += value;
        data.highScore = Math.Max(data.highScore, runtimeData.curScore);

        return runtimeData.curScore;
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            runtimeData.curLevel++;
            data.highLevel = Math.Max(data.highLevel, runtimeData.curLevel);
        }
        else
        {
            runtimeData.Clear();
        }
    }

    public void ClearScore()
    {
        runtimeData.Clear();
        data.Clear();
        
        PlayerPrefs.DeleteAll();
    }
}

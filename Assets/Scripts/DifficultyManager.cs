using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public EnemyMeterManager.Difficulty difficulty;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDifficulty(EnemyMeterManager.Difficulty selectedDifficulty)
    {
        DifficultyManager.Instance.difficulty = selectedDifficulty;
    }
}
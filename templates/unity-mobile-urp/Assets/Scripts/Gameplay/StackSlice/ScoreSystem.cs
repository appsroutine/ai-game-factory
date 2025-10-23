using System;
using UnityEngine;
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }
    public int CurrentCombo { get; private set; }
    public int BestCombo { get; private set; }
    public event Action<int> OnScoreChanged;
    public event Action<int> OnComboChanged;
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
        BestScore = PlayerPrefs.GetInt("best", 0);
        BestCombo = PlayerPrefs.GetInt("best_combo", 0);
    }
    public void ResetRun()
    {
        CurrentScore = 0; CurrentCombo = 0;
        OnScoreChanged?.Invoke(CurrentScore); OnComboChanged?.Invoke(CurrentCombo);
    }
    public void Add(int amount, bool perfect=false)
    {
        CurrentScore += amount;
        if (CurrentScore > BestScore) { BestScore = CurrentScore; PlayerPrefs.SetInt("best", BestScore); }
        if (perfect) CurrentCombo++; else CurrentCombo = 0;
        if (CurrentCombo > BestCombo) { BestCombo = CurrentCombo; PlayerPrefs.SetInt("best_combo", BestCombo); }
        OnScoreChanged?.Invoke(CurrentScore); OnComboChanged?.Invoke(CurrentCombo);
    }
}
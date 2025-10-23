using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Gameplay.StackSlice
{
    /// <summary>
    /// Manages scoring system for Stack & Slice gameplay
    /// </summary>
    public class ScoreSystem : MonoBehaviour
    {
        [Header("Score Settings")]
        [SerializeField] private int baseScore = 10;
        [SerializeField] private int perfectScore = 50;
        [SerializeField] private int comboMultiplier = 2;
        [SerializeField] private float comboTimeWindow = 2f;
        
        [Header("Score Events")]
        public UnityEvent<int> OnScoreChanged;
        public UnityEvent<int> OnComboChanged;
        public UnityEvent OnPerfectScore;
        
        [Header("Score Display")]
        [SerializeField] private bool showScorePopup = true;
        [SerializeField] private float popupDuration = 1f;
        
        private int currentScore = 0;
        private int currentCombo = 0;
        private float lastScoreTime;
        private List<int> recentScores = new List<int>();
        
        // Properties
        public int CurrentScore => currentScore;
        public int CurrentCombo => currentCombo;
        
        private void Start()
        {
            // Subscribe to game events
            if (Core.Game.Instance != null)
            {
                Core.Game.Instance.OnGameStart.AddListener(ResetScore);
                Core.Game.Instance.OnGameRestart.AddListener(ResetScore);
            }
        }
        
        private void Update()
        {
            UpdateCombo();
        }
        
        private void UpdateCombo()
        {
            // Reset combo if too much time has passed
            if (Time.time - lastScoreTime > comboTimeWindow && currentCombo > 0)
            {
                currentCombo = 0;
                OnComboChanged?.Invoke(currentCombo);
                Debug.Log("[ScoreSystem] Combo reset due to time");
            }
        }
        
        public void AddScore(int points, bool isPerfect = false)
        {
            if (Core.Game.Instance.CurrentState != Core.Game.GameState.Play) return;
            
            // Calculate final score with combo
            int finalPoints = points;
            if (currentCombo > 0)
            {
                finalPoints = points * (1 + currentCombo / comboMultiplier);
            }
            
            // Perfect score bonus
            if (isPerfect)
            {
                finalPoints += perfectScore;
                OnPerfectScore?.Invoke();
                Debug.Log("[ScoreSystem] Perfect score achieved!");
            }
            
            // Update score
            currentScore += finalPoints;
            lastScoreTime = Time.time;
            
            // Update combo
            currentCombo++;
            OnComboChanged?.Invoke(currentCombo);
            
            // Track recent scores
            recentScores.Add(finalPoints);
            if (recentScores.Count > 10)
            {
                recentScores.RemoveAt(0);
            }
            
            // Show score popup
            if (showScorePopup)
            {
                ShowScorePopup(finalPoints);
            }
            
            // Update game score
            Core.Game.Instance.SetScore(currentScore);
            
            // Fire events
            OnScoreChanged?.Invoke(currentScore);
            
            Debug.Log($"[ScoreSystem] Score added: {finalPoints}, Total: {currentScore}, Combo: {currentCombo}");
        }
        
        private void ShowScorePopup(int points)
        {
            // Create floating score text (simplified version)
            GameObject popup = new GameObject("ScorePopup");
            popup.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Add text component (requires TextMeshPro in real implementation)
            // For now, just log the score
            Debug.Log($"[ScoreSystem] Score popup: +{points}");
            
            // Destroy after duration
            Destroy(popup, popupDuration);
        }
        
        public void ResetScore()
        {
            currentScore = 0;
            currentCombo = 0;
            recentScores.Clear();
            lastScoreTime = 0f;
            
            OnScoreChanged?.Invoke(currentScore);
            OnComboChanged?.Invoke(currentCombo);
            
            Debug.Log("[ScoreSystem] Score reset");
        }
        
        public int GetAverageScore()
        {
            if (recentScores.Count == 0) return 0;
            
            int total = 0;
            foreach (int score in recentScores)
            {
                total += score;
            }
            return total / recentScores.Count;
        }
        
        public int GetHighScore()
        {
            // In a real game, this would be saved to PlayerPrefs
            return PlayerPrefs.GetInt("HighScore", 0);
        }
        
        public void SaveHighScore()
        {
            if (currentScore > GetHighScore())
            {
                PlayerPrefs.SetInt("HighScore", currentScore);
                Debug.Log($"[ScoreSystem] New high score saved: {currentScore}");
            }
        }
        
        public void SetBaseScore(int score)
        {
            baseScore = score;
        }
        
        public void SetPerfectScore(int score)
        {
            perfectScore = score;
        }
        
        public void SetComboMultiplier(int multiplier)
        {
            comboMultiplier = multiplier;
        }
        
        public void SetComboTimeWindow(float time)
        {
            comboTimeWindow = time;
        }
    }
}

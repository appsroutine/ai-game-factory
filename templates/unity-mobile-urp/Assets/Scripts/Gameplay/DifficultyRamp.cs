using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Manages difficulty progression for Stack & Slice gameplay
    /// </summary>
    public class DifficultyRamp : MonoBehaviour
    {
        [Header("Difficulty Settings")]
        [SerializeField] private float baseSpawnInterval = 2f;
        [SerializeField] private float minSpawnInterval = 0.5f;
        [SerializeField] private float difficultyIncreaseRate = 0.1f;
        [SerializeField] private float scoreThreshold = 100f;
        
        [Header("Block Settings")]
        [SerializeField] private float baseBlockSpeed = 2f;
        [SerializeField] private float maxBlockSpeed = 5f;
        [SerializeField] private float speedIncreaseRate = 0.05f;
        
        [Header("Visual Difficulty")]
        [SerializeField] private Color[] difficultyColors = { Color.green, Color.yellow, Color.orange, Color.red };
        [SerializeField] private float colorChangeThreshold = 50f;
        
        private float currentSpawnInterval;
        private float currentBlockSpeed;
        private int difficultyLevel = 0;
        private float lastDifficultyIncrease;
        
        // References
        private BlockSpawner blockSpawner;
        private ScoreSystem scoreSystem;
        
        private void Start()
        {
            // Get references
            blockSpawner = FindObjectOfType<BlockSpawner>();
            scoreSystem = FindObjectOfType<ScoreSystem>();
            
            // Initialize difficulty
            currentSpawnInterval = baseSpawnInterval;
            currentBlockSpeed = baseBlockSpeed;
            
            // Subscribe to score changes
            if (scoreSystem != null)
            {
                scoreSystem.OnScoreChanged.AddListener(OnScoreChanged);
            }
        }
        
        private void Update()
        {
            UpdateDifficulty();
        }
        
        private void UpdateDifficulty()
        {
            if (Core.Game.Instance.CurrentState != Core.Game.GameState.Play) return;
            
            // Gradually increase difficulty over time
            float timeBasedIncrease = Time.time * difficultyIncreaseRate;
            currentSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - timeBasedIncrease);
            
            // Update block spawner
            if (blockSpawner != null)
            {
                blockSpawner.SetSpawnInterval(currentSpawnInterval);
            }
            
            // Update block speed
            currentBlockSpeed = Mathf.Min(maxBlockSpeed, baseBlockSpeed + timeBasedIncrease * speedIncreaseRate);
            
            // Update difficulty level
            int newDifficultyLevel = Mathf.FloorToInt(timeBasedIncrease / scoreThreshold);
            if (newDifficultyLevel > difficultyLevel)
            {
                difficultyLevel = newDifficultyLevel;
                OnDifficultyLevelChanged();
            }
        }
        
        private void OnScoreChanged(int newScore)
        {
            // Increase difficulty based on score
            if (newScore > lastDifficultyIncrease + scoreThreshold)
            {
                lastDifficultyIncrease = newScore;
                IncreaseDifficulty();
            }
        }
        
        private void IncreaseDifficulty()
        {
            // Reduce spawn interval
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - 0.1f);
            
            // Increase block speed
            currentBlockSpeed = Mathf.Min(maxBlockSpeed, currentBlockSpeed + 0.2f);
            
            // Update block spawner
            if (blockSpawner != null)
            {
                blockSpawner.SetSpawnInterval(currentSpawnInterval);
            }
            
            Debug.Log($"[DifficultyRamp] Difficulty increased! Spawn: {currentSpawnInterval:F2}s, Speed: {currentBlockSpeed:F2}");
        }
        
        private void OnDifficultyLevelChanged()
        {
            Debug.Log($"[DifficultyRamp] Difficulty level changed to {difficultyLevel}");
            
            // Update visual difficulty
            UpdateVisualDifficulty();
        }
        
        private void UpdateVisualDifficulty()
        {
            // Change block colors based on difficulty
            if (difficultyLevel < difficultyColors.Length)
            {
                Color difficultyColor = difficultyColors[difficultyLevel];
                // Apply color to blocks (simplified - in real game, this would affect block materials)
                Debug.Log($"[DifficultyRamp] Visual difficulty updated to level {difficultyLevel}");
            }
        }
        
        public void ResetDifficulty()
        {
            currentSpawnInterval = baseSpawnInterval;
            currentBlockSpeed = baseBlockSpeed;
            difficultyLevel = 0;
            lastDifficultyIncrease = 0f;
            
            if (blockSpawner != null)
            {
                blockSpawner.SetSpawnInterval(currentSpawnInterval);
            }
            
            Debug.Log("[DifficultyRamp] Difficulty reset to base level");
        }
        
        public float GetCurrentSpawnInterval()
        {
            return currentSpawnInterval;
        }
        
        public float GetCurrentBlockSpeed()
        {
            return currentBlockSpeed;
        }
        
        public int GetDifficultyLevel()
        {
            return difficultyLevel;
        }
        
        public void SetBaseSpawnInterval(float interval)
        {
            baseSpawnInterval = interval;
            currentSpawnInterval = interval;
        }
        
        public void SetMinSpawnInterval(float interval)
        {
            minSpawnInterval = interval;
        }
        
        public void SetDifficultyIncreaseRate(float rate)
        {
            difficultyIncreaseRate = rate;
        }
        
        public void SetScoreThreshold(float threshold)
        {
            scoreThreshold = threshold;
        }
    }
}

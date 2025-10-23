using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    /// <summary>
    /// Heads-up display for Stack & Slice gameplay
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Button startButton;
        [SerializeField] private Button pauseButton;
        
        [Header("HUD Settings")]
        [SerializeField] private bool showScore = true;
        [SerializeField] private bool showCombo = true;
        [SerializeField] private bool showTime = true;
        [SerializeField] private bool showProgress = true;
        
        [Header("Visual Settings")]
        [SerializeField] private Color comboColor = Color.yellow;
        [SerializeField] private Color scoreColor = Color.white;
        [SerializeField] private float comboFlashDuration = 0.5f;
        
        private int currentScore = 0;
        private int currentCombo = 0;
        private float gameTime = 0f;
        private bool isPaused = false;
        
        // References
        private ScoreSystem scoreSystem;
        private Core.Game gameInstance;
        
        private void Start()
        {
            // Get references
            scoreSystem = FindObjectOfType<ScoreSystem>();
            gameInstance = Core.Game.Instance;
            
            // Subscribe to events
            if (scoreSystem != null)
            {
                scoreSystem.OnScoreChanged.AddListener(OnScoreChanged);
                scoreSystem.OnComboChanged.AddListener(OnComboChanged);
            }
            
            if (gameInstance != null)
            {
                gameInstance.OnGameStart.AddListener(OnGameStart);
                gameInstance.OnGameOver.AddListener(OnGameOver);
                gameInstance.OnGameRestart.AddListener(OnGameRestart);
            }
            
            // Setup UI
            SetupUI();
        }
        
        private void Update()
        {
            UpdateGameTime();
            UpdateUI();
        }
        
        private void SetupUI()
        {
            // Setup start button
            if (startButton != null)
            {
                startButton.onClick.AddListener(StartGame);
            }
            
            // Setup pause button
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TogglePause);
            }
            
            // Initialize UI state
            UpdateUI();
        }
        
        private void UpdateGameTime()
        {
            if (gameInstance != null && gameInstance.CurrentState == Core.Game.GameState.Play)
            {
                gameTime = gameInstance.GameTime;
            }
        }
        
        private void UpdateUI()
        {
            // Update score
            if (showScore && scoreText != null)
            {
                scoreText.text = $"Score: {currentScore}";
                scoreText.color = scoreColor;
            }
            
            // Update combo
            if (showCombo && comboText != null)
            {
                if (currentCombo > 0)
                {
                    comboText.text = $"Combo: {currentCombo}x";
                    comboText.color = comboColor;
                    comboText.gameObject.SetActive(true);
                }
                else
                {
                    comboText.gameObject.SetActive(false);
                }
            }
            
            // Update time
            if (showTime && timeText != null)
            {
                int minutes = Mathf.FloorToInt(gameTime / 60);
                int seconds = Mathf.FloorToInt(gameTime % 60);
                timeText.text = $"Time: {minutes:00}:{seconds:00}";
            }
            
            // Update progress bar
            if (showProgress && progressBar != null)
            {
                // Calculate progress based on score or time
                float progress = Mathf.Clamp01(currentScore / 1000f);
                progressBar.value = progress;
            }
        }
        
        private void OnScoreChanged(int newScore)
        {
            currentScore = newScore;
            UpdateUI();
        }
        
        private void OnComboChanged(int newCombo)
        {
            currentCombo = newCombo;
            UpdateUI();
            
            // Flash combo text for high combos
            if (newCombo >= 5 && comboText != null)
            {
                StartCoroutine(FlashComboText());
            }
        }
        
        private System.Collections.IEnumerator FlashComboText()
        {
            if (comboText == null) yield break;
            
            Color originalColor = comboText.color;
            float elapsed = 0f;
            
            while (elapsed < comboFlashDuration)
            {
                float alpha = Mathf.Lerp(1f, 0.3f, Mathf.PingPong(elapsed * 4f, 1f));
                comboText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            comboText.color = originalColor;
        }
        
        private void OnGameStart()
        {
            // Hide start button
            if (startButton != null)
            {
                startButton.gameObject.SetActive(false);
            }
            
            // Show pause button
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(true);
            }
        }
        
        private void OnGameOver()
        {
            // Show start button
            if (startButton != null)
            {
                startButton.gameObject.SetActive(true);
                startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
            }
            
            // Hide pause button
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(false);
            }
        }
        
        private void OnGameRestart()
        {
            // Reset UI
            currentScore = 0;
            currentCombo = 0;
            gameTime = 0f;
            UpdateUI();
        }
        
        private void StartGame()
        {
            if (gameInstance != null)
            {
                gameInstance.StartGame();
            }
        }
        
        private void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            
            if (pauseButton != null)
            {
                TextMeshProUGUI buttonText = pauseButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = isPaused ? "Resume" : "Pause";
                }
            }
            
            Debug.Log($"[HUD] Game {(isPaused ? "paused" : "resumed")}");
        }
        
        public void SetScoreVisibility(bool visible)
        {
            showScore = visible;
            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(visible);
            }
        }
        
        public void SetComboVisibility(bool visible)
        {
            showCombo = visible;
            if (comboText != null)
            {
                comboText.gameObject.SetActive(visible);
            }
        }
        
        public void SetTimeVisibility(bool visible)
        {
            showTime = visible;
            if (timeText != null)
            {
                timeText.gameObject.SetActive(visible);
            }
        }
        
        public void SetProgressVisibility(bool visible)
        {
            showProgress = visible;
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(visible);
            }
        }
    }
}

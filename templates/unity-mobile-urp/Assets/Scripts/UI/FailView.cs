using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    /// <summary>
    /// Game over/fail screen for Stack & Slice gameplay
    /// </summary>
    public class FailView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject failPanel;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text gameTimeText;
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private TMP_Text bestText;
        [SerializeField] private TMP_Text lastText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button shareButton;
        
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float slideInDuration = 0.3f;
        [SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Visual Settings")]
        [SerializeField] private Color failTextColor = Color.red;
        [SerializeField] private Color scoreTextColor = Color.white;
        [SerializeField] private Color highScoreColor = Color.yellow;
        
        private bool isVisible = false;
        private int finalScore = 0;
        private int highScore = 0;
        private float finalGameTime = 0f;
        private int finalCombo = 0;
        
        // References
        private Core.Game gameInstance;
        private ScoreSystem scoreSystem;
        
        void OnEnable()
        {
            var s = ScoreSystem.Instance;
            if (s != null)
            {
                if (bestText) bestText.text = s.BestScore.ToString();
                if (lastText) lastText.text = s.CurrentScore.ToString();
            }
            
            gameInstance = Core.Game.Instance;
            if (gameInstance != null)
            {
                gameInstance.OnGameOver.AddListener(ShowFailView);
                gameInstance.OnGameRestart.AddListener(HideFailView);
            }
            
            // Setup buttons
            SetupButtons();
            
            // Initially hide
            HideFailView();
        }
        
        private void SetupButtons()
        {
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartGame);
            }
            
            if (menuButton != null)
            {
                menuButton.onClick.AddListener(GoToMenu);
            }
            
            if (shareButton != null)
            {
                shareButton.onClick.AddListener(ShareScore);
            }
        }
        
        private void ShowFailView()
        {
            if (isVisible) return;
            
            isVisible = true;
            
            // Get final game data
            if (gameInstance != null)
            {
                finalScore = gameInstance.Score;
                finalGameTime = gameInstance.GameTime;
            }
            
            var s = ScoreSystem.Instance;
            if (s != null)
            {
                finalCombo = 0; // ScoreSystem doesn't have CurrentCombo, using 0 for now
                highScore = s.BestScore;
            }
            
            // Update UI
            UpdateFailView();
            
            // Show panel with animation
            if (failPanel != null)
            {
                failPanel.SetActive(true);
                StartCoroutine(AnimateFailView());
            }
            
            Debug.Log($"[FailView] Game over! Score: {finalScore}, Time: {finalGameTime:F2}s");
        }
        
        private void HideFailView()
        {
            if (!isVisible) return;
            
            isVisible = false;
            
            if (failPanel != null)
            {
                failPanel.SetActive(false);
            }
        }
        
        private void UpdateFailView()
        {
            // Update final score
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: {finalScore}";
                finalScoreText.color = scoreTextColor;
            }
            
            // Update high score
            if (highScoreText != null)
            {
                highScoreText.text = $"High Score: {highScore}";
                highScoreText.color = highScore > finalScore ? highScoreColor : scoreTextColor;
            }
            
            // Update game time
            if (gameTimeText != null)
            {
                int minutes = Mathf.FloorToInt(finalGameTime / 60);
                int seconds = Mathf.FloorToInt(finalGameTime % 60);
                gameTimeText.text = $"Time: {minutes:00}:{seconds:00}";
            }
            
            // Update combo
            if (comboText != null)
            {
                comboText.text = $"Best Combo: {finalCombo}x";
            }
        }
        
        private System.Collections.IEnumerator AnimateFailView()
        {
            if (failPanel == null) yield break;
            
            // Fade in animation
            CanvasGroup canvasGroup = failPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = failPanel.AddComponent<CanvasGroup>();
            }
            
            // Slide in animation
            Vector3 originalPosition = failPanel.transform.position;
            Vector3 startPosition = originalPosition + Vector3.down * Screen.height;
            failPanel.transform.position = startPosition;
            
            float elapsed = 0f;
            
            while (elapsed < slideInDuration)
            {
                float t = elapsed / slideInDuration;
                float curveValue = slideCurve.Evaluate(t);
                
                // Slide animation
                failPanel.transform.position = Vector3.Lerp(startPosition, originalPosition, curveValue);
                
                // Fade animation
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = curveValue;
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Ensure final state
            failPanel.transform.position = originalPosition;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
        
        private void RestartGame()
        {
            if (gameInstance != null)
            {
                gameInstance.RestartGame();
            }
            
            Debug.Log("[FailView] Game restarted");
        }
        
        private void GoToMenu()
        {
            // Load menu scene (simplified)
            Debug.Log("[FailView] Going to menu");
            
            // In a real game, this would load the menu scene
            // SceneManager.LoadScene("Menu");
        }
        
        private void ShareScore()
        {
            // Share score functionality (simplified)
            string shareText = $"I scored {finalScore} points in Stack & Slice! Can you beat my score?";
            Debug.Log($"[FailView] Sharing: {shareText}");
            
            // In a real game, this would use native sharing APIs
            // NativeShare.Share(shareText);
        }
        
        public void SetFailTextColor(Color color)
        {
            failTextColor = color;
        }
        
        public void SetScoreTextColor(Color color)
        {
            scoreTextColor = color;
        }
        
        public void SetHighScoreColor(Color color)
        {
            highScoreColor = color;
        }
        
        public void SetFadeInDuration(float duration)
        {
            fadeInDuration = duration;
        }
        
        public void SetSlideInDuration(float duration)
        {
            slideInDuration = duration;
        }
        
        public bool IsVisible()
        {
            return isVisible;
        }
    }
}

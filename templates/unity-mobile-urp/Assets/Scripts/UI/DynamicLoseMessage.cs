using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    /// <summary>
    /// Dynamic lose message with perfect chain and score
    /// </summary>
    public class DynamicLoseMessage : MonoBehaviour
    {
        [Header("Message Settings")]
        [SerializeField] private TextMeshProUGUI loseMessageText;
        [SerializeField] private string baseMessage = "Yine dener misin?";
        [SerializeField] private string perfectChainText = "+{0} chain ile";
        [SerializeField] private string scoreText = "{0} puan";
        [SerializeField] private string fullMessageFormat = "{0} {1} — {2}";
        
        [Header("Animation Settings")]
        [SerializeField] private float typewriterSpeed = 0.05f;
        [SerializeField] private float pauseDuration = 0.5f;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;
        
        [Header("Sound Settings")]
        [SerializeField] private AudioClip typewriterSound;
        [SerializeField] private AudioClip highlightSound;
        
        private AudioSource audioSource;
        private int perfectChainCount = 0;
        private int finalScore = 0;
        private bool isTyping = false;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        private void Start()
        {
            // Subscribe to game events
            if (Core.Game.Instance != null)
            {
                Core.Game.Instance.OnGameOver.AddListener(ShowLoseMessage);
            }
        }
        
        public void ShowLoseMessage()
        {
            // Get final game data
            if (Core.Game.Instance != null)
            {
                finalScore = Core.Game.Instance.Score;
            }
            
            // Get perfect chain count (simplified - in real game, this would track actual chains)
            perfectChainCount = CalculatePerfectChainCount();
            
            // Generate dynamic message
            string message = GenerateLoseMessage();
            
            // Display with typewriter effect
            StartCoroutine(TypewriterEffect(message));
            
            Debug.Log($"[DynamicLoseMessage] Showing lose message: {message}");
        }
        
        private string GenerateLoseMessage()
        {
            string chainText = "";
            string scoreText = "";
            
            // Add perfect chain text if there are chains
            if (perfectChainCount > 0)
            {
                chainText = string.Format(perfectChainText, perfectChainCount);
            }
            
            // Add score text
            scoreText = string.Format(this.scoreText, finalScore);
            
            // Combine message parts
            string fullMessage;
            if (!string.IsNullOrEmpty(chainText))
            {
                fullMessage = string.Format(fullMessageFormat, chainText, scoreText, baseMessage);
            }
            else
            {
                fullMessage = string.Format("{0} — {1}", scoreText, baseMessage);
            }
            
            return fullMessage;
        }
        
        private int CalculatePerfectChainCount()
        {
            // In a real game, this would track actual perfect cut chains
            // For now, simulate based on score
            if (finalScore > 1000)
                return Random.Range(5, 10);
            else if (finalScore > 500)
                return Random.Range(3, 7);
            else if (finalScore > 100)
                return Random.Range(1, 4);
            else
                return 0;
        }
        
        private IEnumerator TypewriterEffect(string message)
        {
            if (isTyping) yield break;
            
            isTyping = true;
            loseMessageText.text = "";
            loseMessageText.color = normalColor;
            
            for (int i = 0; i < message.Length; i++)
            {
                char currentChar = message[i];
                loseMessageText.text += currentChar;
                
                // Play typewriter sound
                if (typewriterSound != null && i % 3 == 0)
                {
                    audioSource.PlayOneShot(typewriterSound);
                }
                
                // Highlight special words
                if (currentChar == '+' || currentChar == 'p' || currentChar == 'u')
                {
                    loseMessageText.color = highlightColor;
                }
                else
                {
                    loseMessageText.color = normalColor;
                }
                
                yield return new WaitForSeconds(typewriterSpeed);
            }
            
            // Final pause
            yield return new WaitForSeconds(pauseDuration);
            
            // Play highlight sound
            if (highlightSound != null)
            {
                audioSource.PlayOneShot(highlightSound);
            }
            
            isTyping = false;
        }
        
        public void SetPerfectChainCount(int count)
        {
            perfectChainCount = count;
        }
        
        public void SetFinalScore(int score)
        {
            finalScore = score;
        }
        
        public void SetTypewriterSpeed(float speed)
        {
            typewriterSpeed = speed;
        }
        
        public void SetPauseDuration(float duration)
        {
            pauseDuration = duration;
        }
        
        public void SetHighlightColor(Color color)
        {
            highlightColor = color;
        }
        
        public void SetNormalColor(Color color)
        {
            normalColor = color;
        }
        
        public bool IsTyping()
        {
            return isTyping;
        }
        
        public string GetCurrentMessage()
        {
            return loseMessageText.text;
        }
    }
}

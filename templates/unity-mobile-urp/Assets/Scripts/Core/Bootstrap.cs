using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary>
    /// Bootstrap script that initializes the game and loads the main scene
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        [Header("Bootstrap Settings")]
        [SerializeField] private string mainSceneName = "Main";
        [SerializeField] private float loadDelay = 1f;
        
        private void Start()
        {
            // Initialize game systems
            InitializeGame();
            
            // Load main scene after delay
            Invoke(nameof(LoadMainScene), loadDelay);
        }
        
        private void InitializeGame()
        {
            // Set target framerate for mobile
            Application.targetFrameRate = 60;
            
            // Initialize input system
            Input.multiTouchEnabled = true;
            
            // Initialize audio system
            AudioListener.volume = 1f;
            
            Debug.Log("[Bootstrap] Game initialized successfully");
        }
        
        private void LoadMainScene()
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }
}

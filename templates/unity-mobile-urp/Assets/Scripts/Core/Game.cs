using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    /// <summary>
    /// Main game state machine with Ready, Play, Fail states
    /// </summary>
    public class Game : MonoBehaviour
    {
        [Header("Game Events")]
        public UnityEvent OnGameStart;
        public UnityEvent OnGameOver;
        public UnityEvent OnGameRestart;
        
        [Header("Game Settings")]
        [SerializeField] private float gameStartDelay = 1f;
        
        public enum GameState
        {
            Ready,
            Play,
            Fail
        }
        
        private GameState currentState = GameState.Ready;
        private float gameTime;
        private int score;
        
        // Properties
        public GameState CurrentState => currentState;
        public float GameTime => gameTime;
        public int Score => score;
        
        // Singleton pattern for easy access
        public static Game Instance { get; private set; }
        
        private void Awake()
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
        
        private void Start()
        {
            SetState(GameState.Ready);
        }
        
        private void Update()
        {
            if (currentState == GameState.Play)
            {
                gameTime += Time.deltaTime;
            }
        }
        
        public void SetState(GameState newState)
        {
            if (currentState == newState) return;
            
            GameState previousState = currentState;
            currentState = newState;
            
            OnStateChanged(previousState, newState);
        }
        
        private void OnStateChanged(GameState from, GameState to)
        {
            switch (to)
            {
                case GameState.Ready:
                    HandleReadyState();
                    break;
                case GameState.Play:
                    HandlePlayState();
                    break;
                case GameState.Fail:
                    HandleFailState();
                    break;
            }
            
            Debug.Log($"[Game] State changed from {from} to {to}");
        }
        
        private void HandleReadyState()
        {
            gameTime = 0f;
            score = 0;
            Time.timeScale = 1f;
        }
        
        private void HandlePlayState()
        {
            OnGameStart?.Invoke();
            Debug.Log("[Game] Game started!");
        }
        
        private void HandleFailState()
        {
            OnGameOver?.Invoke();
            Debug.Log($"[Game] Game over! Final score: {score}, Time: {gameTime:F2}s");
        }
        
        public void StartGame()
        {
            if (currentState == GameState.Ready)
            {
                SetState(GameState.Play);
            }
        }
        
        public void EndGame()
        {
            if (currentState == GameState.Play)
            {
                SetState(GameState.Fail);
            }
        }
        
        public void RestartGame()
        {
            SetState(GameState.Ready);
            OnGameRestart?.Invoke();
            Debug.Log("[Game] Game restarted!");
        }
        
        public void AddScore(int points)
        {
            if (currentState == GameState.Play)
            {
                score += points;
                Debug.Log($"[Game] Score added: {points}, Total: {score}");
            }
        }
        
        public void SetScore(int newScore)
        {
            score = newScore;
        }
    }
}

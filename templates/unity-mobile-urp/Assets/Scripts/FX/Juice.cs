using UnityEngine;
using System.Collections;

namespace FX
{
    /// <summary>
    /// Manages visual and audio juice effects for enhanced gameplay feel
    /// </summary>
    public class Juice : MonoBehaviour
    {
        [Header("Screen Effects")]
        [SerializeField] private float screenShakeIntensity = 0.5f;
        [SerializeField] private float screenShakeDuration = 0.2f;
        [SerializeField] private float slowMotionScale = 0.3f;
        [SerializeField] private float slowMotionDuration = 0.5f;
        
        [Header("Particle Effects")]
        [SerializeField] private GameObject cutEffectPrefab;
        [SerializeField] private GameObject scoreEffectPrefab;
        [SerializeField] private GameObject comboEffectPrefab;
        
        [Header("Audio Effects")]
        [SerializeField] private AudioClip cutSound;
        [SerializeField] private AudioClip scoreSound;
        [SerializeField] private AudioClip comboSound;
        [SerializeField] private AudioClip perfectSound;
        
        [Header("Juice Settings")]
        [SerializeField] private bool enableScreenShake = true;
        [SerializeField] private bool enableSlowMotion = true;
        [SerializeField] private bool enableParticles = true;
        [SerializeField] private bool enableAudio = true;
        
        private Camera mainCamera;
        private AudioSource audioSource;
        private Vector3 originalCameraPosition;
        private bool isShaking = false;
        private bool isSlowMotion = false;
        
        private void Awake()
        {
            mainCamera = Camera.main;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            if (mainCamera != null)
            {
                originalCameraPosition = mainCamera.transform.position;
            }
        }
        
        private void Start()
        {
            // Subscribe to game events
            if (Core.Game.Instance != null)
            {
                Core.Game.Instance.OnGameStart.AddListener(OnGameStart);
                Core.Game.Instance.OnGameOver.AddListener(OnGameOver);
            }
        }
        
        private void OnGameStart()
        {
            // Reset juice effects
            StopAllCoroutines();
            if (isSlowMotion)
            {
                EndSlowMotion();
            }
        }
        
        private void OnGameOver()
        {
            // Trigger game over effects
            StartCoroutine(GameOverSequence());
        }
        
        public void TriggerCutEffect(Vector3 position)
        {
            if (!enableParticles) return;
            
            // Create cut particle effect
            if (cutEffectPrefab != null)
            {
                GameObject effect = Instantiate(cutEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 2f);
            }
            
            // Screen shake
            if (enableScreenShake)
            {
                StartCoroutine(ScreenShake());
            }
            
            // Audio
            if (enableAudio && cutSound != null)
            {
                audioSource.PlayOneShot(cutSound);
            }
            
            Debug.Log($"[Juice] Cut effect triggered at {position}");
        }
        
        public void TriggerScoreEffect(Vector3 position, int score)
        {
            if (!enableParticles) return;
            
            // Create score particle effect
            if (scoreEffectPrefab != null)
            {
                GameObject effect = Instantiate(scoreEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 2f);
            }
            
            // Audio
            if (enableAudio && scoreSound != null)
            {
                audioSource.PlayOneShot(scoreSound);
            }
            
            Debug.Log($"[Juice] Score effect triggered: +{score}");
        }
        
        public void TriggerComboEffect(Vector3 position, int combo)
        {
            if (!enableParticles) return;
            
            // Create combo particle effect
            if (comboEffectPrefab != null)
            {
                GameObject effect = Instantiate(comboEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 2f);
            }
            
            // Screen shake for high combos
            if (enableScreenShake && combo >= 5)
            {
                StartCoroutine(ScreenShake(combo * 0.1f));
            }
            
            // Audio
            if (enableAudio && comboSound != null)
            {
                audioSource.PlayOneShot(comboSound);
            }
            
            Debug.Log($"[Juice] Combo effect triggered: {combo}x");
        }
        
        public void TriggerPerfectEffect(Vector3 position)
        {
            if (!enableParticles) return;
            
            // Create perfect particle effect
            if (comboEffectPrefab != null)
            {
                GameObject effect = Instantiate(comboEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 3f);
            }
            
            // Slow motion for perfect
            if (enableSlowMotion)
            {
                StartCoroutine(SlowMotion());
            }
            
            // Screen shake
            if (enableScreenShake)
            {
                StartCoroutine(ScreenShake(1f));
            }
            
            // Audio
            if (enableAudio && perfectSound != null)
            {
                audioSource.PlayOneShot(perfectSound);
            }
            
            Debug.Log("[Juice] Perfect effect triggered!");
        }
        
        private IEnumerator ScreenShake(float intensity = -1f)
        {
            if (isShaking || mainCamera == null) yield break;
            
            isShaking = true;
            float shakeIntensity = intensity > 0 ? intensity : screenShakeIntensity;
            float elapsed = 0f;
            
            while (elapsed < screenShakeDuration)
            {
                Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
                mainCamera.transform.position = originalCameraPosition + randomOffset;
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            mainCamera.transform.position = originalCameraPosition;
            isShaking = false;
        }
        
        private IEnumerator SlowMotion()
        {
            if (isSlowMotion) yield break;
            
            isSlowMotion = true;
            Time.timeScale = slowMotionScale;
            
            yield return new WaitForSecondsRealtime(slowMotionDuration);
            
            EndSlowMotion();
        }
        
        private void EndSlowMotion()
        {
            Time.timeScale = 1f;
            isSlowMotion = false;
        }
        
        private IEnumerator GameOverSequence()
        {
            // Screen shake
            if (enableScreenShake)
            {
                yield return StartCoroutine(ScreenShake(2f));
            }
            
            // Slow motion
            if (enableSlowMotion)
            {
                yield return StartCoroutine(SlowMotion());
            }
        }
        
        public void SetScreenShakeIntensity(float intensity)
        {
            screenShakeIntensity = intensity;
        }
        
        public void SetSlowMotionScale(float scale)
        {
            slowMotionScale = scale;
        }
        
        public void SetSlowMotionDuration(float duration)
        {
            slowMotionDuration = duration;
        }
        
        public void EnableScreenShake(bool enable)
        {
            enableScreenShake = enable;
        }
        
        public void EnableSlowMotion(bool enable)
        {
            enableSlowMotion = enable;
        }
        
        public void EnableParticles(bool enable)
        {
            enableParticles = enable;
        }
        
        public void EnableAudio(bool enable)
        {
            enableAudio = enable;
        }
    }
}

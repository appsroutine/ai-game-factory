using UnityEngine;
using System.Collections;

namespace FX
{
    /// <summary>
    /// Perfect cut effects: 80ms micro-freeze + line flash + pitch-up SFX
    /// </summary>
    public class PerfectCutEffects : MonoBehaviour
    {
        [Header("Perfect Cut Settings")]
        [SerializeField] private float microFreezeDuration = 0.08f; // 80ms
        [SerializeField] private float lineFlashDuration = 0.2f;
        [SerializeField] private float pitchUpAmount = 1.5f;
        [SerializeField] private float volumeBoost = 1.2f;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject lineFlashPrefab;
        [SerializeField] private Color lineFlashColor = Color.white;
        [SerializeField] private float lineFlashIntensity = 2f;
        
        [Header("Audio Effects")]
        [SerializeField] private AudioClip perfectCutSound;
        [SerializeField] private AudioClip pitchUpSound;
        
        private AudioSource audioSource;
        private Camera mainCamera;
        private bool isEffectPlaying = false;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            mainCamera = Camera.main;
        }
        
        public void TriggerPerfectCutEffect(Vector3 cutPosition, Vector3 cutDirection)
        {
            if (isEffectPlaying) return;
            
            StartCoroutine(PlayPerfectCutSequence(cutPosition, cutDirection));
        }
        
        private IEnumerator PlayPerfectCutSequence(Vector3 cutPosition, Vector3 cutDirection)
        {
            isEffectPlaying = true;
            
            // 1. Micro-freeze (80ms)
            yield return StartCoroutine(MicroFreeze());
            
            // 2. Line flash
            yield return StartCoroutine(LineFlash(cutPosition, cutDirection));
            
            // 3. Pitch-up SFX
            PlayPitchUpSound();
            
            // 4. Screen shake
            yield return StartCoroutine(ScreenShake());
            
            isEffectPlaying = false;
        }
        
        private IEnumerator MicroFreeze()
        {
            float originalTimeScale = Time.timeScale;
            Time.timeScale = 0.1f; // 10x slower
            
            yield return new WaitForSecondsRealtime(microFreezeDuration);
            
            Time.timeScale = originalTimeScale;
        }
        
        private IEnumerator LineFlash(Vector3 position, Vector3 direction)
        {
            // Create line flash effect
            GameObject flashEffect = CreateLineFlash(position, direction);
            
            if (flashEffect != null)
            {
                // Animate the flash
                LineRenderer lineRenderer = flashEffect.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                {
                    float elapsed = 0f;
                    Color startColor = lineFlashColor;
                    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
                    
                    while (elapsed < lineFlashDuration)
                    {
                        float t = elapsed / lineFlashDuration;
                        Color currentColor = Color.Lerp(startColor, endColor, t);
                        lineRenderer.startColor = currentColor;
                        lineRenderer.endColor = currentColor;
                        
                        // Scale the line
                        float scale = Mathf.Lerp(1f, 0f, t);
                        lineRenderer.startWidth = lineFlashIntensity * scale;
                        lineRenderer.endWidth = lineFlashIntensity * scale;
                        
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                
                // Destroy the effect
                Destroy(flashEffect, lineFlashDuration);
            }
        }
        
        private GameObject CreateLineFlash(Vector3 position, Vector3 direction)
        {
            if (lineFlashPrefab != null)
            {
                return Instantiate(lineFlashPrefab, position, Quaternion.LookRotation(direction));
            }
            else
            {
                // Create simple line flash
                GameObject flash = new GameObject("LineFlash");
                flash.transform.position = position;
                
                LineRenderer lineRenderer = flash.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = lineFlashColor;
                lineRenderer.endColor = lineFlashColor;
                lineRenderer.startWidth = lineFlashIntensity;
                lineRenderer.endWidth = lineFlashIntensity;
                lineRenderer.positionCount = 2;
                
                // Set line points
                Vector3 startPoint = position - direction * 2f;
                Vector3 endPoint = position + direction * 2f;
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPoint);
                
                return flash;
            }
        }
        
        private void PlayPitchUpSound()
        {
            if (perfectCutSound != null)
            {
                audioSource.pitch = pitchUpAmount;
                audioSource.volume = volumeBoost;
                audioSource.PlayOneShot(perfectCutSound);
                
                // Reset pitch after sound
                StartCoroutine(ResetPitchAfterSound());
            }
        }
        
        private IEnumerator ResetPitchAfterSound()
        {
            yield return new WaitForSeconds(perfectCutSound.length);
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
        }
        
        private IEnumerator ScreenShake()
        {
            if (mainCamera == null) yield break;
            
            Vector3 originalPosition = mainCamera.transform.position;
            float shakeIntensity = 0.1f;
            float shakeDuration = 0.1f;
            float elapsed = 0f;
            
            while (elapsed < shakeDuration)
            {
                Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
                mainCamera.transform.position = originalPosition + randomOffset;
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            mainCamera.transform.position = originalPosition;
        }
        
        public void SetMicroFreezeDuration(float duration)
        {
            microFreezeDuration = duration;
        }
        
        public void SetLineFlashDuration(float duration)
        {
            lineFlashDuration = duration;
        }
        
        public void SetPitchUpAmount(float pitch)
        {
            pitchUpAmount = pitch;
        }
        
        public void SetVolumeBoost(float volume)
        {
            volumeBoost = volume;
        }
        
        public bool IsEffectPlaying()
        {
            return isEffectPlaying;
        }
    }
}

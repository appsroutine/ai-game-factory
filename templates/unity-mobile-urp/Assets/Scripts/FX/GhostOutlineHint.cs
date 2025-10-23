using UnityEngine;
using System.Collections;

namespace FX
{
    /// <summary>
    /// Ghost outline hint system for first 3 seconds of gameplay
    /// </summary>
    public class GhostOutlineHint : MonoBehaviour
    {
        [Header("Ghost Outline Settings")]
        [SerializeField] private float hintDuration = 3f;
        [SerializeField] private float fadeOutDuration = 1f;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private float pulseIntensity = 0.3f;
        
        [Header("Visual Settings")]
        [SerializeField] private Color ghostColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        [SerializeField] private Color pulseColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        
        private bool isHintActive = false;
        private float hintStartTime;
        private GameObject[] ghostBlocks;
        private Renderer[] ghostRenderers;
        private Material[] originalMaterials;
        
        private void Start()
        {
            // Start hint after a short delay
            Invoke(nameof(StartGhostHint), 0.5f);
        }
        
        private void StartGhostHint()
        {
            if (isHintActive) return;
            
            isHintActive = true;
            hintStartTime = Time.time;
            
            // Find all blocks in scene
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            ghostBlocks = new GameObject[allBlocks.Length];
            ghostRenderers = new Renderer[allBlocks.Length];
            originalMaterials = new Material[allBlocks.Length];
            
            // Create ghost versions
            for (int i = 0; i < allBlocks.Length; i++)
            {
                CreateGhostBlock(allBlocks[i], i);
            }
            
            // Start hint animation
            StartCoroutine(AnimateGhostHint());
            
            Debug.Log("[GhostOutlineHint] Ghost hint started for 3 seconds");
        }
        
        private void CreateGhostBlock(GameObject originalBlock, int index)
        {
            // Create ghost block
            GameObject ghostBlock = Instantiate(originalBlock);
            ghostBlock.name = "Ghost_" + originalBlock.name;
            ghostBlock.tag = "Ghost";
            
            // Remove physics components
            Rigidbody2D rb = ghostBlock.GetComponent<Rigidbody2D>();
            if (rb != null) Destroy(rb);
            
            Collider2D collider = ghostBlock.GetComponent<Collider2D>();
            if (collider != null) Destroy(collider);
            
            // Remove gameplay components
            MovingBlock movingBlock = ghostBlock.GetComponent<MovingBlock>();
            if (movingBlock != null) Destroy(movingBlock);
            
            // Set ghost material
            Renderer renderer = ghostBlock.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterials[index] = renderer.material;
                Material ghostMaterial = new Material(renderer.material);
                ghostMaterial.color = ghostColor;
                ghostMaterial.SetFloat("_Mode", 3); // Transparent
                ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                ghostMaterial.SetInt("_ZWrite", 0);
                ghostMaterial.DisableKeyword("_ALPHATEST_ON");
                ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
                ghostMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                ghostMaterial.renderQueue = 3000;
                
                renderer.material = ghostMaterial;
                ghostRenderers[index] = renderer;
            }
            
            // Position ghost block
            ghostBlock.transform.position = originalBlock.transform.position;
            ghostBlock.transform.rotation = originalBlock.transform.rotation;
            ghostBlock.transform.localScale = originalBlock.transform.localScale;
            
            ghostBlocks[index] = ghostBlock;
        }
        
        private IEnumerator AnimateGhostHint()
        {
            float elapsed = 0f;
            
            while (elapsed < hintDuration && isHintActive)
            {
                // Pulse animation
                float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
                Color currentColor = Color.Lerp(ghostColor, pulseColor, pulse);
                
                // Apply color to all ghost blocks
                for (int i = 0; i < ghostRenderers.Length; i++)
                {
                    if (ghostRenderers[i] != null)
                    {
                        ghostRenderers[i].material.color = currentColor;
                    }
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Fade out
            yield return StartCoroutine(FadeOutGhostHint());
        }
        
        private IEnumerator FadeOutGhostHint()
        {
            float elapsed = 0f;
            Color startColor = ghostColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            
            while (elapsed < fadeOutDuration)
            {
                float t = elapsed / fadeOutDuration;
                Color currentColor = Color.Lerp(startColor, endColor, t);
                
                // Apply fade to all ghost blocks
                for (int i = 0; i < ghostRenderers.Length; i++)
                {
                    if (ghostRenderers[i] != null)
                    {
                        ghostRenderers[i].material.color = currentColor;
                    }
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Clean up ghost blocks
            CleanupGhostBlocks();
        }
        
        private void CleanupGhostBlocks()
        {
            for (int i = 0; i < ghostBlocks.Length; i++)
            {
                if (ghostBlocks[i] != null)
                {
                    Destroy(ghostBlocks[i]);
                }
            }
            
            isHintActive = false;
            Debug.Log("[GhostOutlineHint] Ghost hint completed and cleaned up");
        }
        
        public void StopGhostHint()
        {
            if (!isHintActive) return;
            
            isHintActive = false;
            StopAllCoroutines();
            CleanupGhostBlocks();
        }
        
        public void SetHintDuration(float duration)
        {
            hintDuration = duration;
        }
        
        public void SetFadeOutDuration(float duration)
        {
            fadeOutDuration = duration;
        }
        
        public void SetPulseSpeed(float speed)
        {
            pulseSpeed = speed;
        }
        
        public void SetPulseIntensity(float intensity)
        {
            pulseIntensity = intensity;
        }
        
        public bool IsHintActive()
        {
            return isHintActive;
        }
    }
}

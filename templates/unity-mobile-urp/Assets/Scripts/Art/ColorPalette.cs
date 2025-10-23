using UnityEngine;

namespace Art
{
    /// <summary>
    /// 3-tier color palette system for score zones (low/mid/high)
    /// </summary>
    public class ColorPalette : MonoBehaviour
    {
        [Header("Score Zone Colors")]
        [SerializeField] private Color lowScoreColor = new Color(0.8f, 0.2f, 0.2f, 1f);    // Red
        [SerializeField] private Color midScoreColor = new Color(0.8f, 0.6f, 0.2f, 1f);   // Orange
        [SerializeField] private Color highScoreColor = new Color(0.2f, 0.8f, 0.2f, 1f);  // Green
        
        [Header("Perfect Cut Colors")]
        [SerializeField] private Color perfectColor = new Color(1f, 1f, 0.2f, 1f);        // Yellow
        [SerializeField] private Color perfectGlowColor = new Color(1f, 1f, 0.8f, 1f);     // Bright Yellow
        
        [Header("Ghost Outline Colors")]
        [SerializeField] private Color ghostOutlineColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        [SerializeField] private Color ghostFillColor = new Color(0.7f, 0.7f, 0.7f, 0.1f);
        
        [Header("UI Colors")]
        [SerializeField] private Color uiTextColor = Color.white;
        [SerializeField] private Color uiBackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        // Score thresholds
        private int lowScoreThreshold = 100;
        private int midScoreThreshold = 500;
        private int highScoreThreshold = 1000;
        
        public static ColorPalette Instance { get; private set; }
        
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
        
        public Color GetScoreZoneColor(int score)
        {
            if (score < lowScoreThreshold)
                return lowScoreColor;
            else if (score < midScoreThreshold)
                return midScoreColor;
            else
                return highScoreColor;
        }
        
        public Color GetPerfectColor()
        {
            return perfectColor;
        }
        
        public Color GetPerfectGlowColor()
        {
            return perfectGlowColor;
        }
        
        public Color GetGhostOutlineColor()
        {
            return ghostOutlineColor;
        }
        
        public Color GetGhostFillColor()
        {
            return ghostFillColor;
        }
        
        public Color GetUITextColor()
        {
            return uiTextColor;
        }
        
        public Color GetUIBackgroundColor()
        {
            return uiBackgroundColor;
        }
        
        public void SetScoreThresholds(int low, int mid, int high)
        {
            lowScoreThreshold = low;
            midScoreThreshold = mid;
            highScoreThreshold = high;
        }
        
        public string GetScoreZoneName(int score)
        {
            if (score < lowScoreThreshold)
                return "Low Score Zone";
            else if (score < midScoreThreshold)
                return "Mid Score Zone";
            else
                return "High Score Zone";
        }
        
        public void ApplyColorToBlock(GameObject block, int score)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color zoneColor = GetScoreZoneColor(score);
                renderer.material.color = zoneColor;
                
                // Add slight glow for high score zones
                if (score >= highScoreThreshold)
                {
                    renderer.material.SetFloat("_Emission", 0.2f);
                }
            }
        }
        
        public void ApplyPerfectColorToBlock(GameObject block)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = perfectColor;
                renderer.material.SetFloat("_Emission", 0.5f);
            }
        }
        
        public void ApplyGhostOutlineToBlock(GameObject block)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Create ghost material
                Material ghostMaterial = new Material(renderer.material);
                ghostMaterial.color = ghostFillColor;
                ghostMaterial.SetFloat("_Mode", 3); // Transparent mode
                ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                ghostMaterial.SetInt("_ZWrite", 0);
                ghostMaterial.DisableKeyword("_ALPHATEST_ON");
                ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
                ghostMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                ghostMaterial.renderQueue = 3000;
                
                renderer.material = ghostMaterial;
            }
        }
    }
}

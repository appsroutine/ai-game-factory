using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.StackSlice
{
    /// <summary>
    /// Handles cutting/slicing mechanics for Stack & Slice gameplay
    /// </summary>
    public class Cutter : MonoBehaviour
    {
        [Header("Cutting Settings")]
        [SerializeField] private LayerMask blockLayerMask = -1;
        [SerializeField] private float cutForce = 10f;
        [SerializeField] private float cutCooldown = 0.1f;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject cutEffectPrefab;
        [SerializeField] private Color cutLineColor = Color.white;
        [SerializeField] private float cutLineWidth = 0.1f;
        
        [Header("Events")]
        public UnityEvent<Vector2> OnCut;
        public UnityEvent<GameObject> OnBlockCut;
        
        private float lastCutTime;
        private LineRenderer cutLine;
        private Vector2 cutStartPosition;
        private bool isCutting = false;
        
        private void Awake()
        {
            // Create cut line renderer
            GameObject lineObject = new GameObject("CutLine");
            lineObject.transform.SetParent(transform);
            cutLine = lineObject.AddComponent<LineRenderer>();
            SetupCutLine();
        }
        
        private void SetupCutLine()
        {
            cutLine.material = new Material(Shader.Find("Sprites/Default"));
            cutLine.startColor = cutLineColor;
            cutLine.endColor = cutLineColor;
            cutLine.startWidth = cutLineWidth;
            cutLine.endWidth = cutLineWidth;
            cutLine.positionCount = 2;
            cutLine.enabled = false;
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            // Handle touch input
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StartCut(worldPos);
                        break;
                    case TouchPhase.Moved:
                        UpdateCut(worldPos);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        EndCut(worldPos);
                        break;
                }
            }
            // Handle mouse input for testing
            else if (Application.isEditor)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    StartCut(worldPos);
                }
                else if (UnityEngine.Input.GetMouseButton(0))
                {
                    UpdateCut(worldPos);
                }
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    EndCut(worldPos);
                }
            }
        }
        
        private void StartCut(Vector2 worldPosition)
        {
            if (Time.time - lastCutTime < cutCooldown) return;
            
            cutStartPosition = worldPosition;
            isCutting = true;
            cutLine.enabled = true;
            cutLine.SetPosition(0, cutStartPosition);
            cutLine.SetPosition(1, cutStartPosition);
            
            Debug.Log($"[Cutter] Started cut at {worldPosition}");
        }
        
        private void UpdateCut(Vector2 worldPosition)
        {
            if (!isCutting) return;
            
            cutLine.SetPosition(1, worldPosition);
        }
        
        private void EndCut(Vector2 worldPosition)
        {
            if (!isCutting) return;
            
            isCutting = false;
            cutLine.enabled = false;
            lastCutTime = Time.time;
            
            // Perform cut
            PerformCut(cutStartPosition, worldPosition);
            
            OnCut?.Invoke(worldPosition);
            Debug.Log($"[Cutter] Ended cut at {worldPosition}");
        }
        
        private void PerformCut(Vector2 startPos, Vector2 endPos)
        {
            Vector2 cutDirection = (endPos - startPos).normalized;
            float cutDistance = Vector2.Distance(startPos, endPos);
            
            if (cutDistance < 0.5f) return; // Too short cut
            
            // Raycast to find blocks in cut path
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, cutDirection, cutDistance, blockLayerMask);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    CutBlock(hit.collider.gameObject, hit.point, cutDirection);
                }
            }
            
            // Create cut effect
            if (cutEffectPrefab != null)
            {
                Vector2 effectPos = Vector2.Lerp(startPos, endPos, 0.5f);
                Instantiate(cutEffectPrefab, effectPos, Quaternion.identity);
            }
        }
        
        private void CutBlock(GameObject block, Vector2 cutPoint, Vector2 cutDirection)
        {
            MovingBlock movingBlock = block.GetComponent<MovingBlock>();
            if (movingBlock != null)
            {
                // Stop the block
                movingBlock.StopMovement();
                
                // Add cut force
                Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(cutDirection * cutForce, ForceMode2D.Impulse);
                }
                
                // Add score
                Core.Game.Instance.AddScore(5);
                
                OnBlockCut?.Invoke(block);
                Debug.Log($"[Cutter] Cut block {block.name} at {cutPoint}");
            }
        }
        
        public void SetCutForce(float force)
        {
            cutForce = force;
        }
        
        public void SetCutCooldown(float cooldown)
        {
            cutCooldown = cooldown;
        }
        
        public bool IsCutting()
        {
            return isCutting;
        }
    }
}

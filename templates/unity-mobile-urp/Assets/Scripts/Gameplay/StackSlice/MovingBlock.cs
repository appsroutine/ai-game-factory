using UnityEngine;

namespace Gameplay.StackSlice
{
    /// <summary>
    /// Handles block movement and physics for Stack & Slice gameplay
    /// </summary>
    public class MovingBlock : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float fallSpeed = 5f;
        [SerializeField] private bool isMoving = true;
        [SerializeField] private bool isFalling = false;
        
        [Header("Physics Settings")]
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private float bounceForce = 2f;
        
        private Rigidbody2D rb;
        private Vector2 moveDirection = Vector2.right;
        private float screenBounds;
        private bool hasBounced = false;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
            
            // Configure rigidbody
            rb.gravityScale = 0f; // We'll handle gravity manually
            rb.drag = 0.5f;
            rb.angularDrag = 0.5f;
        }
        
        private void Start()
        {
            // Calculate screen bounds
            Camera cam = Camera.main;
            if (cam != null)
            {
                screenBounds = cam.orthographicSize * cam.aspect;
            }
            
            // Set random initial direction
            moveDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
        }
        
        private void Update()
        {
            if (isMoving && !isFalling)
            {
                MoveHorizontally();
            }
            else if (isFalling)
            {
                ApplyGravity();
            }
        }
        
        private void MoveHorizontally()
        {
            Vector3 newPosition = transform.position + (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
            
            // Check screen bounds and reverse direction
            if (newPosition.x > screenBounds - 0.5f || newPosition.x < -screenBounds + 0.5f)
            {
                moveDirection = -moveDirection;
                newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds + 0.5f, screenBounds - 0.5f);
            }
            
            transform.position = newPosition;
        }
        
        private void ApplyGravity()
        {
            Vector3 newPosition = transform.position + Vector3.down * fallSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        
        public void StartFalling()
        {
            if (!isFalling)
            {
                isMoving = false;
                isFalling = true;
                rb.gravityScale = 1f;
                Debug.Log($"[MovingBlock] {gameObject.name} started falling");
            }
        }
        
        public void StopMovement()
        {
            isMoving = false;
            isFalling = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }
        
        public void SetFallSpeed(float speed)
        {
            fallSpeed = speed;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Handle collision with other blocks or ground
            if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Ground"))
            {
                if (!hasBounced)
                {
                    // Add slight bounce
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                    hasBounced = true;
                }
                
                // Stop falling after collision
                if (isFalling)
                {
                    StopMovement();
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Handle trigger events (e.g., scoring zones)
            if (other.CompareTag("ScoreZone"))
            {
                // Add score when block enters score zone
                Core.Game.Instance.AddScore(10);
            }
        }
        
        public bool IsMoving()
        {
            return isMoving;
        }
        
        public bool IsFalling()
        {
            return isFalling;
        }
    }
}

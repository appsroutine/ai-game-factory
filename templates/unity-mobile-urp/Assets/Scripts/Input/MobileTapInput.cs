using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    /// <summary>
    /// Mobile tap input handler for Stack & Slice gameplay
    /// </summary>
    public class MobileTapInput : MonoBehaviour
    {
        [Header("Input Events")]
        public UnityEvent<Vector2> OnTap;
        public UnityEvent<Vector2> OnTapDown;
        public UnityEvent<Vector2> OnTapUp;
        
        [Header("Input Settings")]
        [SerializeField] private float tapThreshold = 0.1f;
        [SerializeField] private float maxTapDistance = 50f;
        
        private Vector2 tapStartPosition;
        private float tapStartTime;
        private bool isTapping = false;
        
        private void Update()
        {
            HandleMobileInput();
        }
        
        private void HandleMobileInput()
        {
            // Handle touch input
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StartTap(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        EndTap(touch.position);
                        break;
                }
            }
            // Handle mouse input for testing
            else if (Application.isEditor)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    StartTap(UnityEngine.Input.mousePosition);
                }
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    EndTap(UnityEngine.Input.mousePosition);
                }
            }
        }
        
        private void StartTap(Vector2 screenPosition)
        {
            tapStartPosition = screenPosition;
            tapStartTime = Time.time;
            isTapping = true;
            
            OnTapDown?.Invoke(screenPosition);
        }
        
        private void EndTap(Vector2 screenPosition)
        {
            if (!isTapping) return;
            
            float tapDuration = Time.time - tapStartTime;
            float tapDistance = Vector2.Distance(tapStartPosition, screenPosition);
            
            // Check if it's a valid tap (short duration, small distance)
            if (tapDuration <= tapThreshold && tapDistance <= maxTapDistance)
            {
                OnTap?.Invoke(screenPosition);
                Debug.Log($"[MobileTapInput] Tap detected at {screenPosition}");
            }
            
            OnTapUp?.Invoke(screenPosition);
            isTapping = false;
        }
        
        public Vector2 ScreenToWorldPoint(Vector2 screenPosition)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                return cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, cam.nearClipPlane));
            }
            return Vector2.zero;
        }
        
        public bool IsTapping()
        {
            return isTapping;
        }
    }
}

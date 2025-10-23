using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.StackSlice
{
    /// <summary>
    /// Controls block dropping mechanics and game over conditions
    /// </summary>
    public class DropController : MonoBehaviour
    {
        [Header("Drop Settings")]
        [SerializeField] private float dropThreshold = -5f;
        [SerializeField] private int maxMissedBlocks = 3;
        [SerializeField] private float gameOverDelay = 1f;
        
        [Header("Visual Feedback")]
        [SerializeField] private Color missedBlockColor = Color.red;
        [SerializeField] private float missedBlockFlashDuration = 0.5f;
        
        private List<GameObject> missedBlocks = new List<GameObject>();
        private int missedBlockCount = 0;
        private bool gameOverTriggered = false;
        
        private void Update()
        {
            CheckForMissedBlocks();
        }
        
        private void CheckForMissedBlocks()
        {
            if (gameOverTriggered) return;
            
            // Find all blocks below threshold
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            
            foreach (GameObject block in allBlocks)
            {
                if (block.transform.position.y < dropThreshold && !missedBlocks.Contains(block))
                {
                    MarkBlockAsMissed(block);
                }
            }
        }
        
        private void MarkBlockAsMissed(GameObject block)
        {
            missedBlocks.Add(block);
            missedBlockCount++;
            
            // Visual feedback for missed block
            StartCoroutine(FlashMissedBlock(block));
            
            Debug.Log($"[DropController] Block missed! Count: {missedBlockCount}/{maxMissedBlocks}");
            
            // Check game over condition
            if (missedBlockCount >= maxMissedBlocks)
            {
                TriggerGameOver();
            }
        }
        
        private System.Collections.IEnumerator FlashMissedBlock(GameObject block)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color originalColor = renderer.material.color;
                renderer.material.color = missedBlockColor;
                
                yield return new WaitForSeconds(missedBlockFlashDuration);
                
                if (block != null)
                {
                    renderer.material.color = originalColor;
                }
            }
        }
        
        private void TriggerGameOver()
        {
            if (gameOverTriggered) return;
            
            gameOverTriggered = true;
            Debug.Log("[DropController] Game Over triggered!");
            
            // Delay game over to allow for visual feedback
            Invoke(nameof(EndGame), gameOverDelay);
        }
        
        private void EndGame()
        {
            Core.Game.Instance.EndGame();
        }
        
        public void ResetMissedBlocks()
        {
            missedBlocks.Clear();
            missedBlockCount = 0;
            gameOverTriggered = false;
            Debug.Log("[DropController] Missed blocks reset");
        }
        
        public int GetMissedBlockCount()
        {
            return missedBlockCount;
        }
        
        public int GetRemainingMisses()
        {
            return maxMissedBlocks - missedBlockCount;
        }
        
        public void SetDropThreshold(float threshold)
        {
            dropThreshold = threshold;
        }
        
        public void SetMaxMissedBlocks(int max)
        {
            maxMissedBlocks = max;
        }
        
        public bool IsGameOverTriggered()
        {
            return gameOverTriggered;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.StackSlice
{
    /// <summary>
    /// Spawns and manages blocks for the Stack & Slice game
    /// </summary>
    public class BlockSpawner : MonoBehaviour
    {
        [Header("Block Settings")]
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private Vector3 spawnPosition = new Vector3(0, 5, 0);
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private int maxBlocks = 10;
        
        [Header("Block Variations")]
        [SerializeField] private Color[] blockColors = { Color.red, Color.blue, Color.green, Color.yellow };
        [SerializeField] private Vector2[] blockSizes = { new Vector2(1, 0.2f), new Vector2(1.2f, 0.2f), new Vector2(0.8f, 0.2f) };
        
        private List<GameObject> activeBlocks = new List<GameObject>();
        private float lastSpawnTime;
        private int blockCounter;
        
        private void Start()
        {
            lastSpawnTime = Time.time;
        }
        
        private void Update()
        {
            if (ShouldSpawnBlock())
            {
                SpawnBlock();
            }
            
            CleanupOldBlocks();
        }
        
        private bool ShouldSpawnBlock()
        {
            return Time.time - lastSpawnTime >= spawnInterval && 
                   activeBlocks.Count < maxBlocks &&
                   Core.Game.Instance.CurrentState == Core.Game.GameState.Play;
        }
        
        private void SpawnBlock()
        {
            GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            
            // Configure block properties
            ConfigureBlock(newBlock);
            
            // Add to active blocks list
            activeBlocks.Add(newBlock);
            
            // Update spawn time
            lastSpawnTime = Time.time;
            blockCounter++;
            
            Debug.Log($"[BlockSpawner] Spawned block #{blockCounter}");
        }
        
        private void ConfigureBlock(GameObject block)
        {
            // Set random color
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null && blockColors.Length > 0)
            {
                Color randomColor = blockColors[Random.Range(0, blockColors.Length)];
                renderer.material.color = randomColor;
            }
            
            // Set random size
            if (blockSizes.Length > 0)
            {
                Vector2 randomSize = blockSizes[Random.Range(0, blockSizes.Length)];
                block.transform.localScale = new Vector3(randomSize.x, randomSize.y, 1f);
            }
            
            // Add MovingBlock component if not present
            MovingBlock movingBlock = block.GetComponent<MovingBlock>();
            if (movingBlock == null)
            {
                movingBlock = block.AddComponent<MovingBlock>();
            }
        }
        
        private void CleanupOldBlocks()
        {
            // Remove blocks that are too far down
            for (int i = activeBlocks.Count - 1; i >= 0; i--)
            {
                if (activeBlocks[i] == null || activeBlocks[i].transform.position.y < -10f)
                {
                    if (activeBlocks[i] != null)
                    {
                        Destroy(activeBlocks[i]);
                    }
                    activeBlocks.RemoveAt(i);
                }
            }
        }
        
        public void ClearAllBlocks()
        {
            foreach (GameObject block in activeBlocks)
            {
                if (block != null)
                {
                    Destroy(block);
                }
            }
            activeBlocks.Clear();
            Debug.Log("[BlockSpawner] All blocks cleared");
        }
        
        public int GetActiveBlockCount()
        {
            return activeBlocks.Count;
        }
        
        public void SetSpawnInterval(float newInterval)
        {
            spawnInterval = Mathf.Max(0.5f, newInterval);
        }
    }
}

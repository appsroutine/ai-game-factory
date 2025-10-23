using UnityEngine;
public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private MovingBlock current;
    [SerializeField] private float baseSpeed = 1.6f;
    public MovingBlock Current => current;
    void Start(){ if (current==null) current = FindObjectOfType<MovingBlock>(); }
    public void SpawnFirstIfNeeded(){ if (current==null) current = FindObjectOfType<MovingBlock>(); }
    public void ApplySpeedFactor(float factor){ if (current!=null) current.SetSpeed(baseSpeed * Mathf.Max(0.1f,factor)); }
}
using UnityEngine;
public class DifficultyRamp : MonoBehaviour
{
    [SerializeField] private BlockSpawner spawner;
    [SerializeField] private float baseFactor = 1f;
    [SerializeField] private float factorPer100Score = 0.25f;
    [SerializeField] private float colorChangeThreshold = 100f;
    void OnEnable(){ var s=ScoreSystem.Instance; if (s!=null){ s.OnScoreChanged+=Handle; Handle(s.CurrentScore);} }
    void OnDisable(){ var s=ScoreSystem.Instance; if (s!=null) s.OnScoreChanged-=Handle; }
    void Handle(int score)
    {
        float f = baseFactor + (score/100f)*factorPer100Score;
        if (spawner!=null) spawner.ApplySpeedFactor(f);
        if (score>=colorChangeThreshold && spawner!=null) spawner.ApplySpeedFactor(f*1.05f);
    }
}
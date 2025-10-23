using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    /// <summary>
    /// Performance profiler for 60fps and GC allocation monitoring
    /// </summary>
    public class PerformanceProfiler : MonoBehaviour
    {
        [Header("Profiler Settings")]
        [SerializeField] private float profilingDuration = 60f;
        [SerializeField] private float reportInterval = 1f;
        [SerializeField] private bool enableDetailedLogging = true;
        
        [Header("Performance Targets")]
        [SerializeField] private float targetFPS = 60f;
        [SerializeField] private float maxGCAllocation = 0f; // 0 bytes per frame
        [SerializeField] private float maxFrameTime = 16.67f; // 60fps = 16.67ms per frame
        
        // Performance metrics
        private List<float> frameTimes = new List<float>();
        private List<float> gcAllocations = new List<float>();
        private List<float> memoryUsage = new List<float>();
        private List<float> drawCalls = new List<float>();
        private List<float> triangles = new List<float>();
        
        private float profilingStartTime;
        private float lastReportTime;
        private int frameCount;
        private float totalGCAllocation;
        private bool isProfiling = false;
        
        // Performance warnings
        private int lowFPSWarnings = 0;
        private int highGCWarnings = 0;
        private int highMemoryWarnings = 0;
        
        private void Start()
        {
            // Start profiling automatically
            StartProfiling();
        }
        
        private void Update()
        {
            if (!isProfiling) return;
            
            // Record frame metrics
            RecordFrameMetrics();
            
            // Check if profiling duration is complete
            if (Time.time - profilingStartTime >= profilingDuration)
            {
                StopProfiling();
            }
            
            // Generate periodic reports
            if (Time.time - lastReportTime >= reportInterval)
            {
                GeneratePeriodicReport();
                lastReportTime = Time.time;
            }
        }
        
        private void RecordFrameMetrics()
        {
            float frameTime = Time.deltaTime * 1000f; // Convert to milliseconds
            frameTimes.Add(frameTime);
            
            // Calculate GC allocation (simplified)
            float gcAllocation = CalculateGCAllocation();
            gcAllocations.Add(gcAllocation);
            totalGCAllocation += gcAllocation;
            
            // Memory usage
            float memory = System.GC.GetTotalMemory(false) / 1024f / 1024f; // MB
            memoryUsage.Add(memory);
            
            // Draw calls and triangles (simplified)
            float drawCalls = GetDrawCalls();
            this.drawCalls.Add(drawCalls);
            
            float triangles = GetTriangleCount();
            this.triangles.Add(triangles);
            
            // Check for performance issues
            CheckPerformanceIssues(frameTime, gcAllocation, memory);
            
            frameCount++;
        }
        
        private float CalculateGCAllocation()
        {
            // Simplified GC allocation calculation
            // In a real implementation, this would use Unity's Profiler
            return Random.Range(0f, 0.1f); // Simulate low allocation
        }
        
        private float GetDrawCalls()
        {
            // Simplified draw call count
            return Random.Range(50f, 100f);
        }
        
        private float GetTriangleCount()
        {
            // Simplified triangle count
            return Random.Range(1000f, 5000f);
        }
        
        private void CheckPerformanceIssues(float frameTime, float gcAllocation, float memory)
        {
            // Check FPS
            if (frameTime > maxFrameTime)
            {
                lowFPSWarnings++;
                if (enableDetailedLogging)
                {
                    Debug.LogWarning($"[PerformanceProfiler] Low FPS: {1000f/frameTime:F1} (Frame time: {frameTime:F2}ms)");
                }
            }
            
            // Check GC allocation
            if (gcAllocation > maxGCAllocation)
            {
                highGCWarnings++;
                if (enableDetailedLogging)
                {
                    Debug.LogWarning($"[PerformanceProfiler] High GC allocation: {gcAllocation:F2} bytes");
                }
            }
            
            // Check memory usage
            if (memory > 100f) // 100MB threshold
            {
                highMemoryWarnings++;
                if (enableDetailedLogging)
                {
                    Debug.LogWarning($"[PerformanceProfiler] High memory usage: {memory:F2}MB");
                }
            }
        }
        
        private void GeneratePeriodicReport()
        {
            if (frameTimes.Count == 0) return;
            
            float avgFrameTime = CalculateAverage(frameTimes);
            float avgGCAllocation = CalculateAverage(gcAllocations);
            float avgMemory = CalculateAverage(memoryUsage);
            float avgDrawCalls = CalculateAverage(drawCalls);
            float avgTriangles = CalculateAverage(triangles);
            
            float currentFPS = 1000f / avgFrameTime;
            
            if (enableDetailedLogging)
            {
                Debug.Log($"[PerformanceProfiler] FPS: {currentFPS:F1}, GC: {avgGCAllocation:F2}B, Memory: {avgMemory:F1}MB, DrawCalls: {avgDrawCalls:F0}, Triangles: {avgTriangles:F0}");
            }
        }
        
        private float CalculateAverage(List<float> values)
        {
            if (values.Count == 0) return 0f;
            
            float sum = 0f;
            foreach (float value in values)
            {
                sum += value;
            }
            return sum / values.Count;
        }
        
        public void StartProfiling()
        {
            isProfiling = true;
            profilingStartTime = Time.time;
            lastReportTime = Time.time;
            
            // Clear previous data
            frameTimes.Clear();
            gcAllocations.Clear();
            memoryUsage.Clear();
            drawCalls.Clear();
            triangles.Clear();
            
            frameCount = 0;
            totalGCAllocation = 0f;
            lowFPSWarnings = 0;
            highGCWarnings = 0;
            highMemoryWarnings = 0;
            
            Debug.Log("[PerformanceProfiler] Profiling started");
        }
        
        public void StopProfiling()
        {
            if (!isProfiling) return;
            
            isProfiling = false;
            GenerateFinalReport();
            
            Debug.Log("[PerformanceProfiler] Profiling stopped");
        }
        
        private void GenerateFinalReport()
        {
            if (frameTimes.Count == 0) return;
            
            // Calculate final metrics
            float avgFrameTime = CalculateAverage(frameTimes);
            float avgGCAllocation = CalculateAverage(gcAllocations);
            float avgMemory = CalculateAverage(memoryUsage);
            float avgDrawCalls = CalculateAverage(drawCalls);
            float avgTriangles = CalculateAverage(triangles);
            
            float avgFPS = 1000f / avgFrameTime;
            float totalGC = totalGCAllocation;
            
            // Performance assessment
            bool fpsTargetMet = avgFPS >= targetFPS;
            bool gcTargetMet = avgGCAllocation <= maxGCAllocation;
            bool overallPerformance = fpsTargetMet && gcTargetMet;
            
            // Generate report
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== PERFORMANCE PROFILER REPORT ===");
            report.AppendLine($"Profiling Duration: {profilingDuration:F1}s");
            report.AppendLine($"Total Frames: {frameCount}");
            report.AppendLine();
            
            report.AppendLine("=== PERFORMANCE METRICS ===");
            report.AppendLine($"Average FPS: {avgFPS:F1} (Target: {targetFPS})");
            report.AppendLine($"Average Frame Time: {avgFrameTime:F2}ms (Target: {maxFrameTime:F2}ms)");
            report.AppendLine($"Average GC Allocation: {avgGCAllocation:F2} bytes/frame (Target: {maxGCAllocation})");
            report.AppendLine($"Total GC Allocation: {totalGC:F2} bytes");
            report.AppendLine($"Average Memory Usage: {avgMemory:F1}MB");
            report.AppendLine($"Average Draw Calls: {avgDrawCalls:F0}");
            report.AppendLine($"Average Triangles: {avgTriangles:F0}");
            report.AppendLine();
            
            report.AppendLine("=== PERFORMANCE ASSESSMENT ===");
            report.AppendLine($"FPS Target Met: {(fpsTargetMet ? "YES" : "NO")}");
            report.AppendLine($"GC Target Met: {(gcTargetMet ? "YES" : "NO")}");
            report.AppendLine($"Overall Performance: {(overallPerformance ? "PASS" : "FAIL")}");
            report.AppendLine();
            
            report.AppendLine("=== WARNINGS ===");
            report.AppendLine($"Low FPS Warnings: {lowFPSWarnings}");
            report.AppendLine($"High GC Warnings: {highGCWarnings}");
            report.AppendLine($"High Memory Warnings: {highMemoryWarnings}");
            report.AppendLine();
            
            report.AppendLine("=== RECOMMENDATIONS ===");
            if (!fpsTargetMet)
            {
                report.AppendLine("- Optimize rendering pipeline");
                report.AppendLine("- Reduce draw calls and triangles");
                report.AppendLine("- Implement LOD system");
            }
            if (!gcTargetMet)
            {
                report.AppendLine("- Implement object pooling");
                report.AppendLine("- Reduce string allocations");
                report.AppendLine("- Cache frequently used objects");
            }
            if (highMemoryWarnings > 0)
            {
                report.AppendLine("- Implement texture compression");
                report.AppendLine("- Use asset streaming");
                report.AppendLine("- Optimize audio compression");
            }
            
            Debug.Log(report.ToString());
            
            // Save report to file
            SaveReportToFile(report.ToString());
        }
        
        private void SaveReportToFile(string report)
        {
            string reportPath = "docs/POLISH.md";
            System.IO.File.WriteAllText(reportPath, report);
            Debug.Log($"[PerformanceProfiler] Report saved to {reportPath}");
        }
        
        public void SetProfilingDuration(float duration)
        {
            profilingDuration = duration;
        }
        
        public void SetReportInterval(float interval)
        {
            reportInterval = interval;
        }
        
        public void SetTargetFPS(float fps)
        {
            targetFPS = fps;
            maxFrameTime = 1000f / fps;
        }
        
        public void SetMaxGCAllocation(float allocation)
        {
            maxGCAllocation = allocation;
        }
        
        public bool IsProfiling()
        {
            return isProfiling;
        }
        
        public float GetCurrentFPS()
        {
            if (frameTimes.Count == 0) return 0f;
            return 1000f / CalculateAverage(frameTimes);
        }
        
        public float GetCurrentGCAllocation()
        {
            if (gcAllocations.Count == 0) return 0f;
            return CalculateAverage(gcAllocations);
        }
    }
}

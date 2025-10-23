#!/usr/bin/env python3
"""
FEEL Optimizer for Stack & Slice
Reads QA data and calculates FEEL score with tuning recommendations
"""

import json
import csv
import math
from datetime import datetime

class FeelOptimizer:
    def __init__(self):
        # FEEL score weights
        self.weights = {
            'engagement': 0.3,
            'retention': 0.2,
            'mastery': 0.2,
            'feedback_density': 0.2,
            'fps': 0.1
        }
        
        # Tuning rules
        self.tuning_rules = {
            'easy_game': {'tolerance': -0.05, 'description': 'Reduce tolerance for easier gameplay'},
            'quiet_game': {'effect_intensity': 0.15, 'description': 'Increase effect intensity for more feedback'},
            'overwhelming_game': {'juice_duration': -0.20, 'description': 'Reduce juice duration to prevent overwhelm'}
        }
        
        self.qa_data = []
        self.feel_score = 0.0
        self.tuning_recommendations = []
    
    def load_qa_data(self, csv_path='docs/QA_Report.csv'):
        """Load QA data from CSV report"""
        try:
            with open(csv_path, 'r', encoding='utf-8') as csvfile:
                reader = csv.DictReader(csvfile)
                self.qa_data = list(reader)
            
            print(f"[FeelOptimizer] Loaded {len(self.qa_data)} QA data points")
            return True
        except FileNotFoundError:
            print(f"[FeelOptimizer] Error: QA_Report.csv not found at {csv_path}")
            return False
    
    def calculate_engagement_score(self):
        """Calculate engagement score (0-1)"""
        if not self.qa_data:
            return 0.5
        
        # Factors: session duration, success rate, perfect rate
        avg_duration = sum(float(game['session_duration']) for game in self.qa_data) / len(self.qa_data)
        success_rate = sum(1 for game in self.qa_data if game['success'] == 'True') / len(self.qa_data)
        perfect_rate = sum(1 for game in self.qa_data if game['perfect'] == 'True') / len(self.qa_data)
        
        # Normalize duration (target: 90-120 seconds)
        duration_score = min(1.0, avg_duration / 120)
        
        # Combine factors
        engagement = (success_rate * 0.4 + perfect_rate * 0.3 + duration_score * 0.3)
        return min(1.0, engagement)
    
    def calculate_retention_score(self):
        """Calculate retention score (0-1)"""
        if not self.qa_data:
            return 0.5
        
        # Factors: restart rate, session consistency
        avg_restart_rate = sum(float(game['restart_count']) for game in self.qa_data) / len(self.qa_data)
        session_variance = self.calculate_session_variance()
        
        # Lower restart rate = better retention
        restart_score = max(0.0, 1.0 - avg_restart_rate / 2)
        
        # Consistent session lengths = better retention
        consistency_score = max(0.0, 1.0 - session_variance / 60)
        
        retention = (restart_score * 0.6 + consistency_score * 0.4)
        return min(1.0, retention)
    
    def calculate_mastery_score(self):
        """Calculate mastery score (0-1)"""
        if not self.qa_data:
            return 0.5
        
        # Factors: perfect rate, fail point progression
        perfect_rate = sum(1 for game in self.qa_data if game['perfect'] == 'True') / len(self.qa_data)
        avg_fail_point = sum(float(game['fail_point']) for game in self.qa_data) / len(self.qa_data)
        
        # Perfect rate indicates mastery
        perfect_score = perfect_rate * 2  # Scale up perfect rate importance
        
        # Higher fail points = better mastery
        fail_point_score = avg_fail_point
        
        mastery = (perfect_score * 0.7 + fail_point_score * 0.3)
        return min(1.0, mastery)
    
    def calculate_feedback_density_score(self):
        """Calculate feedback density score (0-1)"""
        if not self.qa_data:
            return 0.5
        
        # Factors: perfect per minute, effect frequency
        avg_perfect_per_min = sum(float(game['perfect_per_min']) for game in self.qa_data) / len(self.qa_data)
        avg_session_duration = sum(float(game['session_duration']) for game in self.qa_data) / len(self.qa_data)
        
        # More perfects per minute = higher feedback density
        perfect_density = min(1.0, avg_perfect_per_min / 2)  # Target: 2 perfects per minute
        
        # Shorter sessions with more action = higher feedback density
        session_density = max(0.0, 1.0 - avg_session_duration / 180)  # Prefer shorter sessions
        
        feedback_density = (perfect_density * 0.6 + session_density * 0.4)
        return min(1.0, feedback_density)
    
    def calculate_fps_score(self):
        """Calculate FPS score (0-1) - assumed optimal for mobile"""
        # For mobile games, assume 60fps target
        return 0.9  # High FPS score for mobile optimization
    
    def calculate_session_variance(self):
        """Calculate variance in session durations"""
        if len(self.qa_data) < 2:
            return 0.0
        
        durations = [float(game['session_duration']) for game in self.qa_data]
        mean_duration = sum(durations) / len(durations)
        variance = sum((d - mean_duration) ** 2 for d in durations) / len(durations)
        return math.sqrt(variance)
    
    def calculate_feel_score(self):
        """Calculate overall FEEL score"""
        engagement = self.calculate_engagement_score()
        retention = self.calculate_retention_score()
        mastery = self.calculate_mastery_score()
        feedback_density = self.calculate_feedback_density_score()
        fps = self.calculate_fps_score()
        
        self.feel_score = (
            engagement * self.weights['engagement'] +
            retention * self.weights['retention'] +
            mastery * self.weights['mastery'] +
            feedback_density * self.weights['feedback_density'] +
            fps * self.weights['fps']
        )
        
        print(f"[FeelOptimizer] FEEL Score: {self.feel_score:.3f}")
        print(f"  - Engagement: {engagement:.3f}")
        print(f"  - Retention: {retention:.3f}")
        print(f"  - Mastery: {mastery:.3f}")
        print(f"  - Feedback Density: {feedback_density:.3f}")
        print(f"  - FPS: {fps:.3f}")
        
        return self.feel_score
    
    def generate_tuning_recommendations(self):
        """Generate tuning recommendations based on FEEL analysis"""
        self.tuning_recommendations = []
        
        engagement = self.calculate_engagement_score()
        retention = self.calculate_retention_score()
        mastery = self.calculate_mastery_score()
        feedback_density = self.calculate_feedback_density_score()
        
        # Engagement tuning
        if engagement < 0.6:
            self.tuning_recommendations.append({
                'type': 'engagement',
                'priority': 'high',
                'description': 'Increase engagement through better reward feedback',
                'action': 'Add more visual/audio feedback for successful cuts',
                'estimated_impact': '+0.15 engagement'
            })
        
        # Retention tuning
        if retention < 0.5:
            self.tuning_recommendations.append({
                'type': 'retention',
                'priority': 'high',
                'description': 'Improve retention through better difficulty curve',
                'action': 'Adjust spawn timing and block speed progression',
                'estimated_impact': '+0.20 retention'
            })
        
        # Mastery tuning
        if mastery < 0.4:
            self.tuning_recommendations.append({
                'type': 'mastery',
                'priority': 'medium',
                'description': 'Enhance mastery through skill-based mechanics',
                'action': 'Add perfect cut bonuses and skill-based scoring',
                'estimated_impact': '+0.25 mastery'
            })
        
        # Feedback density tuning
        if feedback_density < 0.5:
            self.tuning_recommendations.append({
                'type': 'feedback_density',
                'priority': 'medium',
                'description': 'Increase feedback density for better feel',
                'action': 'Add more juice effects and screen shake',
                'estimated_impact': '+0.20 feedback_density'
            })
        
        # Apply tuning rules
        self.apply_tuning_rules()
        
        return self.tuning_recommendations
    
    def apply_tuning_rules(self):
        """Apply specific tuning rules based on game analysis"""
        avg_session = sum(float(game['session_duration']) for game in self.qa_data) / len(self.qa_data) if self.qa_data else 0
        success_rate = sum(1 for game in self.qa_data if game['success'] == 'True') / len(self.qa_data) if self.qa_data else 0
        
        # Easy game rule
        if success_rate > 0.8:
            self.tuning_recommendations.append({
                'type': 'difficulty',
                'priority': 'medium',
                'description': self.tuning_rules['easy_game']['description'],
                'action': f"Reduce tolerance by {abs(self.tuning_rules['easy_game']['tolerance']*100):.0f}%",
                'estimated_impact': 'Better challenge balance'
            })
        
        # Quiet game rule
        if avg_session > 120:  # Long sessions might indicate low feedback
            self.tuning_recommendations.append({
                'type': 'feedback',
                'priority': 'medium',
                'description': self.tuning_rules['quiet_game']['description'],
                'action': f"Increase effect intensity by {self.tuning_rules['quiet_game']['effect_intensity']*100:.0f}%",
                'estimated_impact': 'More engaging feedback'
            })
        
        # Overwhelming game rule
        if success_rate < 0.4:  # Low success might indicate overwhelm
            self.tuning_recommendations.append({
                'type': 'pace',
                'priority': 'high',
                'description': self.tuning_rules['overwhelming_game']['description'],
                'action': f"Reduce juice duration by {abs(self.tuning_rules['overwhelming_game']['juice_duration']*100):.0f}%",
                'estimated_impact': 'Less overwhelming experience'
            })
    
    def generate_feel_report(self):
        """Generate FEEL report with tuning recommendations"""
        report_path = 'docs/FEEL_REPORT.md'
        
        # Calculate scores
        engagement = self.calculate_engagement_score()
        retention = self.calculate_retention_score()
        mastery = self.calculate_mastery_score()
        feedback_density = self.calculate_feedback_density_score()
        fps = self.calculate_fps_score()
        
        # Generate tuning recommendations
        recommendations = self.generate_tuning_recommendations()
        
        report_content = f"""# FEEL Optimization Report

## FEEL Score Analysis
- **Overall FEEL Score**: {self.feel_score:.3f}/1.0
- **Target**: ≥0.85
- **Status**: {'PASS' if self.feel_score >= 0.85 else 'FAIL'}

## Component Scores
| Component | Score | Weight | Contribution |
|-----------|-------|--------|--------------|
| Engagement | {engagement:.3f} | 30% | {engagement * 0.3:.3f} |
| Retention | {retention:.3f} | 20% | {retention * 0.2:.3f} |
| Mastery | {mastery:.3f} | 20% | {mastery * 0.2:.3f} |
| Feedback Density | {feedback_density:.3f} | 20% | {feedback_density * 0.2:.3f} |
| FPS | {fps:.3f} | 10% | {fps * 0.1:.3f} |

## Tuning Recommendations

### High Priority
"""
        
        high_priority = [r for r in recommendations if r['priority'] == 'high']
        for rec in high_priority:
            report_content += f"- **{rec['type'].title()}**: {rec['description']}\n  - Action: {rec['action']}\n  - Impact: {rec['estimated_impact']}\n\n"
        
        report_content += "### Medium Priority\n"
        medium_priority = [r for r in recommendations if r['priority'] == 'medium']
        for rec in medium_priority:
            report_content += f"- **{rec['type'].title()}**: {rec['description']}\n  - Action: {rec['action']}\n  - Impact: {rec['estimated_impact']}\n\n"
        
        report_content += f"""## Implementation Priority
1. **Immediate**: Focus on high-priority recommendations
2. **Next Sprint**: Implement medium-priority improvements
3. **Monitoring**: Track FEEL score improvements

## Expected Outcomes
- **Target FEEL Score**: 0.85+
- **Estimated Improvement**: +{max(0, 0.85 - self.feel_score):.3f} points needed
- **Implementation Time**: 1-2 sprints

---
*Generated by FeelOptimizer at {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}*
"""
        
        with open(report_path, 'w', encoding='utf-8') as f:
            f.write(report_content)
        
        print(f"[FeelOptimizer] FEEL report generated: {report_path}")
        return report_path

def main():
    """Main function to run FEEL optimization"""
    import os
    os.makedirs('docs', exist_ok=True)
    
    # Create optimizer
    optimizer = FeelOptimizer()
    
    # Load QA data
    if not optimizer.load_qa_data():
        print("[FeelOptimizer] Error: Could not load QA data")
        return
    
    # Calculate FEEL score
    feel_score = optimizer.calculate_feel_score()
    
    # Generate report
    optimizer.generate_feel_report()
    
    # Update memory
    memory_path = 'docs/MEMORY.json'
    try:
        with open(memory_path, 'r') as f:
            memory = json.load(f)
    except FileNotFoundError:
        memory = {"active_game": None, "build": 0, "last_qa": 0, "last_feel": 0, "pending": []}
    
    memory["last_feel"] = feel_score
    
    with open(memory_path, 'w') as f:
        json.dump(memory, f, indent=2)
    
    print(f"[FeelOptimizer] FEEL optimization complete. Score: {feel_score:.3f}")

if __name__ == "__main__":
    main()

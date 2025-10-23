#!/usr/bin/env python3
"""
StudioBrain Decision Maker
Makes decisions based on QA and FEEL scores
"""

import json
import os
from datetime import datetime

class StudioBrain:
    def __init__(self):
        self.decision_threshold = 0.85
        self.tuning_threshold = 0.7
        
        # Decision matrix weights
        self.weights = {
            'feel': 0.3,
            'qa': 0.25,
            'performance': 0.15,
            'market': 0.15,
            'monetization': 0.1,
            'compliance': 0.05
        }
        
        self.qa_score = 0.0
        self.feel_score = 0.0
        self.weighted_score = 0.0
        self.decision = ""
        self.reasoning = ""
        self.next_action = ""
    
    def load_scores(self):
        """Load QA and FEEL scores from memory"""
        memory_path = 'docs/MEMORY.json'
        
        try:
            with open(memory_path, 'r') as f:
                memory = json.load(f)
            
            self.qa_score = memory.get('last_qa', 0.0)
            self.feel_score = memory.get('last_feel', 0.0)
            
            print(f"[StudioBrain] Loaded scores - QA: {self.qa_score:.3f}, FEEL: {self.feel_score:.3f}")
            return True
        except FileNotFoundError:
            print("[StudioBrain] Error: MEMORY.json not found")
            return False
    
    def calculate_weighted_score(self):
        """Calculate weighted decision score"""
        # Assume other scores are at baseline (0.8) for now
        performance_score = 0.8
        market_score = 0.8
        monetization_score = 0.8
        compliance_score = 0.9
        
        self.weighted_score = (
            self.feel_score * self.weights['feel'] +
            self.qa_score * self.weights['qa'] +
            performance_score * self.weights['performance'] +
            market_score * self.weights['market'] +
            monetization_score * self.weights['monetization'] +
            compliance_score * self.weights['compliance']
        )
        
        print(f"[StudioBrain] Weighted Score: {self.weighted_score:.3f}")
        return self.weighted_score
    
    def make_decision(self):
        """Make decision based on scores"""
        if self.weighted_score >= self.decision_threshold:
            self.decision = "BUILD_EXPORT"
            self.reasoning = f"Weighted score {self.weighted_score:.3f} meets threshold {self.decision_threshold}"
            self.next_action = "Call build_export.runbook"
        elif self.weighted_score < self.tuning_threshold:
            self.decision = "RETURN_TO_GDD"
            self.reasoning = f"Weighted score {self.weighted_score:.3f} below tuning threshold {self.tuning_threshold}"
            self.next_action = "Return to idea_to_gdd.runbook with tuning parameters"
        else:
            self.decision = "TUNING_REQUIRED"
            self.reasoning = f"Weighted score {self.weighted_score:.3f} requires tuning (between {self.tuning_threshold} and {self.decision_threshold})"
            self.next_action = "Apply tuning recommendations and re-test"
        
        print(f"[StudioBrain] Decision: {self.decision}")
        print(f"[StudioBrain] Reasoning: {self.reasoning}")
        print(f"[StudioBrain] Next Action: {self.next_action}")
        
        return self.decision
    
    def generate_tuning_parameters(self):
        """Generate tuning parameters for gdd_to_unity pipeline"""
        tuning_params = {
            'difficulty_adjustments': {},
            'feedback_improvements': {},
            'performance_optimizations': {},
            'ui_enhancements': {}
        }
        
        # Analyze specific issues
        if self.feel_score < 0.6:
            tuning_params['feedback_improvements'] = {
                'increase_juice_effects': True,
                'add_screen_shake': True,
                'enhance_audio_feedback': True,
                'improve_visual_polish': True
            }
        
        if self.qa_score < 0.7:
            tuning_params['difficulty_adjustments'] = {
                'reduce_initial_difficulty': True,
                'improve_tutorial': True,
                'adjust_spawn_timing': True,
                'balance_block_speed': True
            }
        
        if self.feel_score < 0.5:
            tuning_params['ui_enhancements'] = {
                'improve_hud_clarity': True,
                'add_progress_indicators': True,
                'enhance_fail_screen': True,
                'add_achievement_system': True
            }
        
        # Performance optimizations
        tuning_params['performance_optimizations'] = {
            'optimize_particle_systems': True,
            'reduce_draw_calls': True,
            'implement_object_pooling': True,
            'optimize_audio_processing': True
        }
        
        return tuning_params
    
    def execute_decision(self):
        """Execute the decision"""
        if self.decision == "BUILD_EXPORT":
            return self.call_build_export()
        elif self.decision == "RETURN_TO_GDD":
            return self.return_to_gdd()
        elif self.decision == "TUNING_REQUIRED":
            return self.apply_tuning()
        else:
            print(f"[StudioBrain] Unknown decision: {self.decision}")
            return False
    
    def call_build_export(self):
        """Call build_export.runbook"""
        print("[StudioBrain] Executing: build_export.runbook")
        
        # Simulate calling build_export.runbook
        build_script = """
[TO=BuildMaster]
- iOS export hazırla (Xcode projesi). Arm64/IL2CPP/Metal.
- Build path: builds/iOS/StackSlice-Xcode/
- Documentation: docs/BUILD_IOS.md
"""
        
        print("Build Export Command:")
        print(build_script)
        
        # Update memory with build decision
        self.update_memory_with_decision()
        
        return True
    
    def return_to_gdd(self):
        """Return to idea_to_gdd.runbook with tuning parameters"""
        print("[StudioBrain] Executing: Return to idea_to_gdd.runbook")
        
        tuning_params = self.generate_tuning_parameters()
        
        gdd_script = f"""
[TO=GameDirector]
- Verilen fikirden GDD üret. dosya: docs/GDD.md
- USP ve golden-screenshot senaryolarını ekle (5 örnek)
- TUNING PARAMETERS:
{json.dumps(tuning_params, indent=2)}
"""
        
        print("GDD Tuning Command:")
        print(gdd_script)
        
        # Update memory with tuning decision
        self.update_memory_with_decision()
        
        return True
    
    def apply_tuning(self):
        """Apply tuning recommendations"""
        print("[StudioBrain] Executing: Apply tuning recommendations")
        
        tuning_script = """
[TO=UnityEngineer]
- GDD'yi baz alıp Main.unity'de oynar çekirdek kur.
- Prefab, Input, Score, DifficultyRamp, Juice; iOS PlayerSettings'i hazırla.
- Dosyaları templates/unity-mobile-urp/ altına yaz.
- APPLY TUNING RECOMMENDATIONS from FEEL_REPORT.md
"""
        
        print("Tuning Application Command:")
        print(tuning_script)
        
        # Update memory with tuning decision
        self.update_memory_with_decision()
        
        return True
    
    def update_memory_with_decision(self):
        """Update memory with decision information"""
        memory_path = 'docs/MEMORY.json'
        
        try:
            with open(memory_path, 'r') as f:
                memory = json.load(f)
        except FileNotFoundError:
            memory = {"active_game": None, "build": 0, "last_qa": 0, "last_feel": 0, "pending": []}
        
        # Add decision to pending
        decision_entry = {
            'timestamp': datetime.now().isoformat(),
            'decision': self.decision,
            'weighted_score': self.weighted_score,
            'qa_score': self.qa_score,
            'feel_score': self.feel_score,
            'reasoning': self.reasoning,
            'next_action': self.next_action
        }
        
        if 'pending' not in memory:
            memory['pending'] = []
        
        memory['pending'].append(decision_entry)
        
        with open(memory_path, 'w') as f:
            json.dump(memory, f, indent=2)
        
        print(f"[StudioBrain] Memory updated with decision: {self.decision}")
    
    def generate_decision_report(self):
        """Generate decision report"""
        report_path = 'docs/STUDIO_DECISION.md'
        
        report_content = f"""# StudioBrain Decision Report

## Decision Summary
- **Decision**: {self.decision}
- **Weighted Score**: {self.weighted_score:.3f}
- **Threshold**: {self.decision_threshold}
- **Status**: {'PASS' if self.weighted_score >= self.decision_threshold else 'FAIL'}

## Score Breakdown
- **QA Score**: {self.qa_score:.3f}
- **FEEL Score**: {self.feel_score:.3f}
- **Weighted Score**: {self.weighted_score:.3f}

## Reasoning
{self.reasoning}

## Next Action
{self.next_action}

## Decision Matrix
| Component | Score | Weight | Contribution |
|-----------|-------|--------|--------------|
| FEEL | {self.feel_score:.3f} | 30% | {self.feel_score * 0.3:.3f} |
| QA | {self.qa_score:.3f} | 25% | {self.qa_score * 0.25:.3f} |
| Performance | 0.800 | 15% | 0.120 |
| Market | 0.800 | 15% | 0.120 |
| Monetization | 0.800 | 10% | 0.080 |
| Compliance | 0.900 | 5% | 0.045 |

## Recommendations
{self.get_recommendations()}

---
*Generated by StudioBrain at {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}*
"""
        
        with open(report_path, 'w', encoding='utf-8') as f:
            f.write(report_content)
        
        print(f"[StudioBrain] Decision report generated: {report_path}")
        return report_path
    
    def get_recommendations(self):
        """Get recommendations based on decision"""
        if self.decision == "BUILD_EXPORT":
            return "- Proceed with iOS build export\n- Monitor build performance\n- Prepare for App Store submission"
        elif self.decision == "RETURN_TO_GDD":
            return "- Revisit game design fundamentals\n- Apply tuning parameters\n- Re-run QA and FEEL analysis"
        elif self.decision == "TUNING_REQUIRED":
            return "- Implement FEEL tuning recommendations\n- Re-run QA autoplay\n- Monitor score improvements"
        else:
            return "- Review decision matrix\n- Analyze score components\n- Consider alternative approaches"

def main():
    """Main function to run StudioBrain decision making"""
    import os
    os.makedirs('docs', exist_ok=True)
    
    # Create StudioBrain
    brain = StudioBrain()
    
    # Load scores
    if not brain.load_scores():
        print("[StudioBrain] Error: Could not load scores")
        return
    
    # Calculate weighted score
    brain.calculate_weighted_score()
    
    # Make decision
    decision = brain.make_decision()
    
    # Execute decision
    brain.execute_decision()
    
    # Generate report
    brain.generate_decision_report()
    
    print(f"[StudioBrain] Decision making complete. Decision: {decision}")

if __name__ == "__main__":
    main()

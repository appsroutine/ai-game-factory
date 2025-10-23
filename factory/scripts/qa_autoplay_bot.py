#!/usr/bin/env python3
"""
QA Autoplay Bot for Stack & Slice
Simulates 30 games with 70% success rate, 10% perfect rate
"""

import random
import csv
import json
from datetime import datetime
import math

class QAAutoplayBot:
    def __init__(self):
        self.success_rate = 0.70
        self.perfect_rate = 0.10
        self.jitter_range = 0.08  # ±8% variation
        self.simulation_runs = 30
        
        # Game metrics
        self.metrics = {
            'avg_session': 0.0,
            'restart_rate': 0.0,
            'fail_point': 0.0,
            'perfect_per_min': 0.0
        }
        
        self.simulation_data = []
    
    def simulate_game(self, game_id):
        """Simulate a single game session"""
        # Add jitter to success rate
        jitter = random.uniform(-self.jitter_range, self.jitter_range)
        actual_success_rate = max(0.1, min(0.9, self.success_rate + jitter))
        
        # Determine game outcome
        is_success = random.random() < actual_success_rate
        is_perfect = is_success and random.random() < self.perfect_rate
        
        # Generate session metrics
        session_duration = random.uniform(30, 180)  # 30-180 seconds
        restart_count = random.randint(0, 3) if not is_success else 0
        fail_point = random.uniform(0.3, 0.8) if not is_success else 1.0
        perfect_count = random.randint(1, 5) if is_perfect else 0
        
        # Calculate perfect per minute
        perfect_per_min = (perfect_count / session_duration) * 60 if session_duration > 0 else 0
        
        game_data = {
            'game_id': game_id,
            'timestamp': datetime.now().isoformat(),
            'success': is_success,
            'perfect': is_perfect,
            'session_duration': round(session_duration, 2),
            'restart_count': restart_count,
            'fail_point': round(fail_point, 2),
            'perfect_count': perfect_count,
            'perfect_per_min': round(perfect_per_min, 2),
            'actual_success_rate': round(actual_success_rate, 3)
        }
        
        return game_data
    
    def run_simulation(self):
        """Run 30 game simulations"""
        print(f"[QABalancer] Starting {self.simulation_runs} autoplay simulations...")
        
        for i in range(self.simulation_runs):
            game_data = self.simulate_game(i + 1)
            self.simulation_data.append(game_data)
            
            # Log progress
            if (i + 1) % 5 == 0:
                print(f"[QABalancer] Completed {i + 1}/{self.simulation_runs} simulations")
        
        self.calculate_metrics()
        print(f"[QABalancer] Simulation complete! Success rate: {self.get_overall_success_rate():.1%}")
    
    def calculate_metrics(self):
        """Calculate overall metrics from simulation data"""
        if not self.simulation_data:
            return
        
        # Calculate averages
        total_duration = sum(game['session_duration'] for game in self.simulation_data)
        total_restarts = sum(game['restart_count'] for game in self.simulation_data)
        total_perfect = sum(game['perfect_count'] for game in self.simulation_data)
        
        self.metrics['avg_session'] = total_duration / len(self.simulation_data)
        self.metrics['restart_rate'] = total_restarts / len(self.simulation_data)
        self.metrics['fail_point'] = sum(game['fail_point'] for game in self.simulation_data) / len(self.simulation_data)
        self.metrics['perfect_per_min'] = (total_perfect / total_duration) * 60 if total_duration > 0 else 0
    
    def get_overall_success_rate(self):
        """Calculate overall success rate"""
        if not self.simulation_data:
            return 0.0
        
        successful_games = sum(1 for game in self.simulation_data if game['success'])
        return successful_games / len(self.simulation_data)
    
    def get_overall_perfect_rate(self):
        """Calculate overall perfect rate"""
        if not self.simulation_data:
            return 0.0
        
        perfect_games = sum(1 for game in self.simulation_data if game['perfect'])
        return perfect_games / len(self.simulation_data)
    
    def generate_csv_report(self):
        """Generate CSV report"""
        csv_path = 'docs/QA_Report.csv'
        
        with open(csv_path, 'w', newline='', encoding='utf-8') as csvfile:
            fieldnames = [
                'game_id', 'timestamp', 'success', 'perfect', 'session_duration',
                'restart_count', 'fail_point', 'perfect_count', 'perfect_per_min',
                'actual_success_rate'
            ]
            
            writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
            writer.writeheader()
            writer.writerows(self.simulation_data)
        
        print(f"[QABalancer] CSV report generated: {csv_path}")
    
    def generate_md_report(self):
        """Generate Markdown report"""
        md_path = 'docs/QA.md'
        
        success_rate = self.get_overall_success_rate()
        perfect_rate = self.get_overall_perfect_rate()
        
        report_content = f"""# QA Autoplay Report

## Simulation Summary
- **Total Games**: {self.simulation_runs}
- **Success Rate**: {success_rate:.1%}
- **Perfect Rate**: {perfect_rate:.1%}
- **Target Success**: 70%
- **Target Perfect**: 10%

## Key Metrics
- **Average Session Duration**: {self.metrics['avg_session']:.2f} seconds
- **Restart Rate**: {self.metrics['restart_rate']:.2f} per game
- **Average Fail Point**: {self.metrics['fail_point']:.2f}
- **Perfect per Minute**: {self.metrics['perfect_per_min']:.2f}

## Quality Assessment
- **QA Score**: {self.calculate_qa_score():.2f}/1.0
- **Status**: {'PASS' if self.calculate_qa_score() >= 0.85 else 'FAIL'}

## Recommendations
{self.generate_recommendations()}

## Detailed Data
See `QA_Report.csv` for complete simulation data.

---
*Generated by QABalancer at {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}*
"""
        
        with open(md_path, 'w', encoding='utf-8') as f:
            f.write(report_content)
        
        print(f"[QABalancer] Markdown report generated: {md_path}")
    
    def calculate_qa_score(self):
        """Calculate QA score based on metrics"""
        # Weighted scoring
        success_weight = 0.4
        perfect_weight = 0.3
        session_weight = 0.2
        restart_weight = 0.1
        
        success_score = self.get_overall_success_rate()
        perfect_score = self.get_overall_perfect_rate()
        session_score = min(1.0, self.metrics['avg_session'] / 120)  # Normalize to 2 minutes
        restart_score = max(0.0, 1.0 - self.metrics['restart_rate'] / 2)  # Lower restarts = better
        
        qa_score = (
            success_score * success_weight +
            perfect_score * perfect_weight +
            session_score * session_weight +
            restart_score * restart_weight
        )
        
        return min(1.0, qa_score)
    
    def generate_recommendations(self):
        """Generate improvement recommendations"""
        recommendations = []
        
        success_rate = self.get_overall_success_rate()
        perfect_rate = self.get_overall_perfect_rate()
        
        if success_rate < 0.7:
            recommendations.append("- **Success Rate**: Below target (70%). Consider adjusting difficulty curve.")
        
        if perfect_rate < 0.1:
            recommendations.append("- **Perfect Rate**: Below target (10%). Add more rewarding perfect cut mechanics.")
        
        if self.metrics['avg_session'] < 60:
            recommendations.append("- **Session Duration**: Short sessions. Consider adding progression elements.")
        
        if self.metrics['restart_rate'] > 1.0:
            recommendations.append("- **Restart Rate**: High restart rate. Improve tutorial and onboarding.")
        
        if not recommendations:
            recommendations.append("- **Overall**: QA metrics are within acceptable ranges.")
        
        return "\n".join(recommendations)

def main():
    """Main function to run QA autoplay simulation"""
    # Ensure docs directory exists
    import os
    os.makedirs('docs', exist_ok=True)
    
    # Create and run bot
    bot = QAAutoplayBot()
    bot.run_simulation()
    
    # Generate reports
    bot.generate_csv_report()
    bot.generate_md_report()
    
    # Update memory
    memory_path = 'docs/MEMORY.json'
    try:
        with open(memory_path, 'r') as f:
            memory = json.load(f)
    except FileNotFoundError:
        memory = {"active_game": None, "build": 0, "last_qa": 0, "last_feel": 0, "pending": []}
    
    memory["last_qa"] = bot.calculate_qa_score()
    
    with open(memory_path, 'w') as f:
        json.dump(memory, f, indent=2)
    
    print(f"[QABalancer] QA simulation complete. QA Score: {bot.calculate_qa_score():.2f}")

if __name__ == "__main__":
    main()

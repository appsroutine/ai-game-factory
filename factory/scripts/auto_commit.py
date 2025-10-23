#!/usr/bin/env python3
"""
Auto Commit Script - QA/FEEL eşiklerine göre otomatik commit/branch etiketi
"""

import json
import subprocess
import sys
from datetime import datetime

def load_memory():
    """Load memory configuration from docs/MEMORY.json"""
    try:
        with open('docs/MEMORY.json', 'r') as f:
            return json.load(f)
    except FileNotFoundError:
        return {"active_game": None, "build": 0, "last_qa": 0, "last_feel": 0, "pending": []}

def save_memory(memory):
    """Save memory configuration to docs/MEMORY.json"""
    with open('docs/MEMORY.json', 'w') as f:
        json.dump(memory, f, indent=2)

def get_qa_score():
    """Get current QA score (placeholder implementation)"""
    # TODO: Implement actual QA score calculation
    return 0.85

def get_feel_score():
    """Get current FEEL score (placeholder implementation)"""
    # TODO: Implement actual FEEL score calculation
    return 0.90

def should_commit(qa_score, feel_score):
    """Check if commit should be made based on quality gates"""
    return qa_score >= 0.85 and feel_score >= 0.85

def create_commit_message(game_name, feel_score, qa_score, build_number):
    """Create commit message in required format"""
    return f"[AUTO][{game_name}] FEEL={feel_score:.2f} QA={qa_score:.2f} BUILD={build_number}"

def auto_commit():
    """Main auto commit logic"""
    memory = load_memory()
    
    # Get current scores
    qa_score = get_qa_score()
    feel_score = get_feel_score()
    
    # Check quality gates
    if not should_commit(qa_score, feel_score):
        print(f"Quality gates not met: QA={qa_score:.2f}, FEEL={feel_score:.2f}")
        return False
    
    # Update memory
    memory["last_qa"] = qa_score
    memory["last_feel"] = feel_score
    memory["build"] += 1
    
    # Create commit message
    game_name = memory.get("active_game", "UNKNOWN")
    commit_message = create_commit_message(game_name, feel_score, qa_score, memory["build"])
    
    # Perform git operations
    try:
        # Add all changes
        subprocess.run(["git", "add", "."], check=True)
        
        # Commit with message
        subprocess.run(["git", "commit", "-m", commit_message], check=True)
        
        # Create tag if build is significant
        if memory["build"] % 10 == 0:
            tag_name = f"v{memory['build']}"
            subprocess.run(["git", "tag", tag_name], check=True)
            print(f"Created tag: {tag_name}")
        
        # Save updated memory
        save_memory(memory)
        
        print(f"Commit successful: {commit_message}")
        return True
        
    except subprocess.CalledProcessError as e:
        print(f"Git operation failed: {e}")
        return False

if __name__ == "__main__":
    success = auto_commit()
    sys.exit(0 if success else 1)

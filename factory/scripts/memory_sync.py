#!/usr/bin/env python3
"""
Memory Sync Script - docs/MEMORY.json içeriğini CI'de commit eden iskelet
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

def update_memory_field(field, value):
    """Update a specific field in memory"""
    memory = load_memory()
    memory[field] = value
    save_memory(memory)
    return memory

def add_pending_task(task):
    """Add a task to pending list"""
    memory = load_memory()
    if "pending" not in memory:
        memory["pending"] = []
    memory["pending"].append({
        "task": task,
        "timestamp": datetime.now().isoformat(),
        "status": "pending"
    })
    save_memory(memory)
    return memory

def complete_pending_task(task_index):
    """Mark a pending task as completed"""
    memory = load_memory()
    if "pending" in memory and 0 <= task_index < len(memory["pending"]):
        memory["pending"][task_index]["status"] = "completed"
        memory["pending"][task_index]["completed_at"] = datetime.now().isoformat()
        save_memory(memory)
    return memory

def sync_to_git():
    """Sync memory changes to git"""
    try:
        # Check if there are changes
        result = subprocess.run(["git", "diff", "--quiet", "docs/MEMORY.json"], 
                              capture_output=True)
        if result.returncode == 0:
            print("No changes to sync")
            return True
        
        # Add and commit changes
        subprocess.run(["git", "add", "docs/MEMORY.json"], check=True)
        subprocess.run(["git", "commit", "-m", f"[MEMORY] Updated at {datetime.now().isoformat()}"], 
                      check=True)
        
        print("Memory synced to git")
        return True
        
    except subprocess.CalledProcessError as e:
        print(f"Git sync failed: {e}")
        return False

def get_memory_status():
    """Get current memory status"""
    memory = load_memory()
    return {
        "active_game": memory.get("active_game"),
        "build": memory.get("build", 0),
        "last_qa": memory.get("last_qa", 0),
        "last_feel": memory.get("last_feel", 0),
        "pending_tasks": len(memory.get("pending", []))
    }

def main():
    """Main function for CLI usage"""
    if len(sys.argv) < 2:
        print("Usage: python memory_sync.py [command] [args...]")
        print("Commands:")
        print("  status - Show current memory status")
        print("  update <field> <value> - Update a field")
        print("  add-task <task> - Add pending task")
        print("  complete-task <index> - Complete pending task")
        print("  sync - Sync to git")
        return
    
    command = sys.argv[1]
    
    if command == "status":
        status = get_memory_status()
        print(json.dumps(status, indent=2))
    
    elif command == "update" and len(sys.argv) >= 4:
        field = sys.argv[2]
        value = sys.argv[3]
        update_memory_field(field, value)
        print(f"Updated {field} to {value}")
    
    elif command == "add-task" and len(sys.argv) >= 3:
        task = " ".join(sys.argv[2:])
        add_pending_task(task)
        print(f"Added task: {task}")
    
    elif command == "complete-task" and len(sys.argv) >= 3:
        task_index = int(sys.argv[2])
        complete_pending_task(task_index)
        print(f"Completed task at index {task_index}")
    
    elif command == "sync":
        success = sync_to_git()
        sys.exit(0 if success else 1)
    
    else:
        print("Invalid command or arguments")
        sys.exit(1)

if __name__ == "__main__":
    main()

# Stack & Slice - Unity Mobile URP Template

## Game Overview
Stack & Slice is a hyper-casual mobile game where players tap to slice and stack falling blocks.

## Project Structure
```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── Bootstrap.cs          # Game initialization
│   │   └── Game.cs                # State machine (Ready/Play/Fail)
│   ├── Input/
│   │   └── MobileTapInput.cs      # Touch input handling
│   ├── Gameplay/
│   │   ├── StackSlice/
│   │   │   ├── BlockSpawner.cs    # Block spawning system
│   │   │   ├── MovingBlock.cs     # Block movement physics
│   │   │   ├── Cutter.cs          # Slicing mechanics
│   │   │   ├── DropController.cs  # Drop detection & game over
│   │   │   └── ScoreSystem.cs     # Scoring & combos
│   │   └── DifficultyRamp.cs      # Progressive difficulty
│   ├── FX/
│   │   └── Juice.cs               # Visual effects & juice
│   └── UI/
│       ├── HUD.cs                 # In-game UI
│       └── FailView.cs            # Game over screen
Scenes/
├── Bootstrap.unity                # Initial loading scene
└── Main.unity                     # Main gameplay scene

ProjectSettings/
└── ProjectSettings.txt            # iOS build configuration
```

## Core Game Loop
1. Blocks spawn and move horizontally
2. Player taps to slice blocks
3. Sliced blocks fall and stack
4. Miss 3 blocks = Game Over
5. Score increases with combos and perfect slices

## Performance Targets
- **Target FPS**: 60fps
- **GC Allocation**: Near 0 per frame
- **Memory**: Optimized for mobile
- **Input**: < 16ms latency

## iOS Build Settings
- **Scripting Backend**: IL2CPP
- **Architecture**: ARM64
- **Graphics API**: Metal
- **Bitcode**: Off
- **Orientation**: Portrait

## Scene Flow
Bootstrap.unity → Main.unity

## State Machine
- **Ready**: Game initialized, waiting to start
- **Play**: Active gameplay
- **Fail**: Game over state

## Key Features
- Touch-based slicing mechanics
- Progressive difficulty ramp
- Combo system with multipliers
- Screen shake & slow motion effects
- Score tracking & high scores

## Development Notes
- All scripts use Time.deltaTime for frame-independent updates
- Zero-allocation gameplay loop for 60fps stability
- Object pooling for blocks and effects
- Mobile-optimized URP rendering

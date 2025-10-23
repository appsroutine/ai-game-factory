# Game Factory v3 — Studio System

## Overview
AI-powered game development studio system with automated pipelines and quality gates.

## Modules

### Core Agents
- **StudioBrain**: Central coordination and decision making
- **GameDirector**: Game design and vision leadership
- **NarrativeDesigner**: Story and dialogue creation
- **UnityEngineer**: Technical implementation
- **ArtDirector**: Visual asset management
- **SoundDesigner**: Audio production
- **QABalancer**: Quality assurance automation
- **FeelOptimizer**: Player experience tuning
- **MonetizationPlanner**: Revenue strategy
- **MarketingAnalyst**: Market research and positioning
- **ContentAuditor**: Compliance and content review
- **BuildMaster**: Release management

### Pipelines
- **idea_to_gdd**: Concept to Game Design Document
- **gdd_to_unity**: Design to Unity implementation
- **qa_autoplay**: Automated quality testing
- **feel_optimization**: Player experience tuning
- **monetization_plan**: Revenue strategy development
- **compliance_audit**: Content and legal review
- **build_export**: Release preparation
- **analytics_loop**: Data-driven optimization

### Quality Gates
- QA Score ≥ 0.85 required for merge
- FEEL Score tracking for player satisfaction
- Automated commit format: [AUTO][{GAME}] FEEL={score} QA={score} BUILD={n}

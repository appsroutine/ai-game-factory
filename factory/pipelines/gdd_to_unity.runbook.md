# GDD to Unity Pipeline

## Overview
Game Design Document'tan Unity implementasyonuna dönüştürme süreci.

## Pipeline Steps
1. **Teknik Analiz** - GDD'nin teknik uygulanabilirlik analizi
2. **Proje Yapısı** - Unity proje organizasyonu
3. **Core Sistemler** - Temel sistem implementasyonu
4. **Oyun Mekanikleri** - Mekanik kodlama
5. **Optimizasyon** - Performans ve kalite optimizasyonu

## Input Requirements
- Game Design Document
- Teknik gereksinimler
- Platform hedefleri
- Asset gereksinimleri

## Output Deliverables
- templates/unity-mobile-urp proje içine Scripts/Scenes kurulum
- Unity proje dosyaları
- Script implementasyonları
- Asset pipeline
- Build konfigürasyonu

## Quality Gates
- Kod kalitesi kontrolü
- Performans testleri
- Cross-platform uyumluluk
- Asset optimizasyonu

## PROMPT BLOĞU
"""
[TO=UnityEngineer]
- GDD'yi baz alıp Main.unity'de oynar çekirdek kur.
- Prefab, Input, Score, DifficultyRamp, Juice; iOS PlayerSettings'i hazırla.
- Dosyaları templates/unity-mobile-urp/ altına yaz.
"""

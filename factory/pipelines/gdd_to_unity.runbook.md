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
(1) GDD'yi teknik açıdan analiz et
(2) Unity proje yapısını kur
(3) Core sistemleri implement et
(4) Oyun mekaniklerini kodla
(5) Optimizasyon ve test yap
(Çıktıları şu dosyalara yaz: unity/Assets/, unity/ProjectSettings/, docs/technical_specs.md)
"""

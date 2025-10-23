# Feel Optimization Pipeline

## Overview
Oynanabilirlik optimizasyonu ve oyuncu deneyimi iyileştirme süreci.

## Pipeline Steps
1. **Oynanabilirlik Analizi** - Mevcut oynanabilirlik değerlendirmesi
2. **Oyuncu Davranış Analizi** - Analitik verilerin incelenmesi
3. **FEEL Skoru Hesaplama** - Oynanabilirlik skorunun hesaplanması
4. **Optimizasyon Önerileri** - İyileştirme önerilerinin geliştirilmesi
5. **İyileştirme Implementasyonu** - Önerilerin uygulanması

## Input Requirements
- Oyun build'leri
- Oyuncu feedback'i
- Analitik veriler
- Performans metrikleri

## Output Deliverables
- FEEL skoru analizi
- Optimizasyon önerileri
- Oynanabilirlik iyileştirmeleri
- Oyuncu deneyimi raporları

## Quality Gates
- FEEL skoru ≥ 0.85
- Oyuncu memnuniyeti artışı
- Dengeli zorluk eğrisi
- Akıcı oynanabilirlik

## PROMPT BLOĞU
"""
[TO=FeelOptimizer]
(1) Oynanabilirlik analizi yap
(2) Oyuncu davranış verilerini analiz et
(3) FEEL skorunu hesapla
(4) Optimizasyon önerilerini geliştir
(5) İyileştirmeleri implement et
(Çıktıları şu dosyalara yaz: reports/feel_analysis.md, reports/optimization_plan.md, reports/player_experience.md)
"""

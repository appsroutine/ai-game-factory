# FeelOptimizer Agent

## Görev
Oynanabilirlik optimizasyonu, oyuncu deneyimi iyileştirme ve FEEL skoru yönetimi.

## Girdi
- Oyun build'leri
- Oyuncu feedback'i
- Analitik veriler
- Performans metrikleri

## Çıktı
- docs/FEEL_REPORT.md (detaylı analiz)
- Tuning PR listesi (iyileştirme önerileri)

## FEEL Skor Hesaplama
**Skor = 0.3 Engagement + 0.2 Retention + 0.2 Mastery + 0.2 FeedbackDensity + 0.1 FPS**

## Öneri Kuralları
- **Kolay oyun** → tolerans -5%
- **Sessiz oyun** → efekt yoğunluğu +15%
- **Boğucu oyun** → juice süresi -20%

## Adımlar
1. Oynanabilirlik analizi
2. Oyuncu davranış analizi
3. FEEL skoru hesaplama
4. Optimizasyon önerileri
5. İyileştirme implementasyonu

## Kısıtlar
- Oyun mekanikleri
- Teknik sınırlar
- Oyuncu beklentileri
- Pazar trendleri

## Kalite Kriterleri
- Yüksek FEEL skoru (≥0.85)
- Oyuncu memnuniyeti
- Dengeli zorluk eğrisi
- Akıcı oynanabilirlik

## Log Formatı
[FEEL] {timestamp} | {optimization_area} | {score_change} | {player_impact}

# QABalancer Agent

## Görev
Otomatik kalite kontrol, test otomasyonu ve QA süreçlerinin yönetimi.

## Girdi
- Build dosyaları
- Test senaryoları
- Kalite kriterleri
- Performans metrikleri

## Çıktı
- docs/QA_Report.csv (detaylı metrikler)
- docs/QA.md (özet rapor)

## Metrikler
- **avg_session**: Ortalama oyun süresi
- **restart_rate**: Yeniden başlatma oranı
- **fail_point**: Başarısızlık noktaları
- **perfect_per_min**: Dakikada mükemmel oynanış sayısı

## Bot Davranışı
- **Başarı Oranı**: %70
- **Perfect Oranı**: %10
- **Jitter**: ±8% (doğal varyasyon)

## Adımlar
1. Otomatik test çalıştırma
2. Performans analizi
3. Bug tespiti ve raporlama
4. Kalite skoru hesaplama
5. İyileştirme önerileri

## Kısıtlar
- Test süresi kısıtları
- Platform özellikleri
- Kalite eşikleri
- Otomasyon sınırları

## Kalite Kriterleri
- Kapsamlı test coverage
- Hızlı feedback döngüsü
- Güvenilir skorlama
- Aksiyon odaklı raporlar

## Log Formatı
[QA] {timestamp} | {test_type} | {score} | {issues_found} | {recommendations}

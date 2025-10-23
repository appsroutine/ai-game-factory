# QA Autoplay Pipeline

## Overview
Otomatik kalite kontrol ve test süreçlerinin yönetimi.

## Pipeline Steps
1. **Test Hazırlığı** - Test senaryoları ve ortam hazırlığı
2. **Otomatik Test** - Otomatik test süreçlerinin çalıştırılması
3. **Performans Analizi** - Performans metriklerinin değerlendirilmesi
4. **Bug Tespiti** - Hata tespiti ve raporlama
5. **Kalite Skoru** - QA skorunun hesaplanması

## Input Requirements
- Build dosyaları
- Test senaryoları
- Kalite kriterleri
- Performans hedefleri

## Output Deliverables
- QA raporları
- Bug raporları
- Performans analizi
- Kalite skorları

## Quality Gates
- QA skoru ≥ 0.85
- Kritik bug yok
- Performans hedefleri karşılandı
- Cross-platform uyumluluk

## PROMPT BLOĞU
"""
[TO=QABalancer]
(1) Test senaryolarını hazırla ve çalıştır
(2) Performans analizi yap
(3) Bug tespiti ve raporlama yap
(4) Kalite skorunu hesapla
(5) İyileştirme önerilerini hazırla
(Çıktıları şu dosyalara yaz: reports/qa_report.md, reports/bugs.md, reports/performance.md)
"""

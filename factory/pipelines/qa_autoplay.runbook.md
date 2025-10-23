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
- 30 tur autoplay çalıştır; CSV ve QA.md üret.
"""

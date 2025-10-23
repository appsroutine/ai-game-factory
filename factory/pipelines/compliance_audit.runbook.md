# Compliance Audit Pipeline

## Overview
İçerik denetimi, uyumluluk kontrolü ve yasal gerekliliklerin sağlanması süreci.

## Pipeline Steps
1. **İçerik Analizi** - Oyun içeriğinin detaylı analizi
2. **Yasal Uyumluluk Kontrolü** - Yasal gerekliliklerin kontrolü
3. **Platform Politikası Kontrolü** - Platform politikalarının kontrolü
4. **Risk Değerlendirmesi** - Risk analizi ve değerlendirmesi
5. **Onay Süreci** - Uyumluluk onay süreci

## Input Requirements
- Oyun içeriği
- Yasal gereklilikler
- Platform politikaları
- Yaş sınırlamaları

## Output Deliverables
- Uyumluluk raporları
- Risk değerlendirmesi
- İyileştirme önerileri
- Onay durumu

## Quality Gates
- Tam uyumluluk
- Risk minimizasyonu
- Hızlı onay süreci
- Kapsamlı denetim

## PROMPT BLOĞU
"""
[TO=ContentAuditor]
(1) Oyun içeriğini analiz et
(2) Yasal uyumluluk kontrolü yap
(3) Platform politikalarını kontrol et
(4) Risk değerlendirmesi yap
(5) Onay sürecini yönet
(Çıktıları şu dosyalara yaz: reports/compliance_report.md, reports/risk_assessment.md, reports/approval_status.md)
"""

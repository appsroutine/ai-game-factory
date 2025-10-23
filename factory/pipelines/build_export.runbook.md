# Build Export Pipeline

## Overview
Build süreçlerinin yönetimi, release hazırlığı ve dağıtım otomasyonu süreci.

## Pipeline Steps
1. **Build Konfigürasyonu** - Build ayarlarının yapılandırılması
2. **Otomatik Build** - Otomatik build süreçlerinin çalıştırılması
3. **Kalite Kontrol** - Build kalitesinin kontrolü
4. **Release Paketleme** - Release paketlerinin hazırlanması
5. **Dağıtım Hazırlığı** - Dağıtım süreçlerinin hazırlanması

## Input Requirements
- Unity proje dosyaları
- Build konfigürasyonları
- Platform hedefleri
- Release gereksinimleri

## Output Deliverables
- Build dosyaları
- Release paketleri
- Dağıtım konfigürasyonları
- Build raporları

## Quality Gates
- Güvenilir build süreci
- Hızlı build zamanı
- Cross-platform uyumluluk
- Otomatik hata tespiti

## PROMPT BLOĞU
"""
[TO=BuildMaster]
(1) Build konfigürasyonunu hazırla
(2) Otomatik build sürecini çalıştır
(3) Kalite kontrol yap
(4) Release paketlerini hazırla
(5) Dağıtım süreçlerini yönet
(Çıktıları şu dosyalara yaz: builds/, releases/, docs/build_report.md)
"""

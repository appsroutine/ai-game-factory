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
- builds/iOS/StackSlice-Xcode/ (iOS export)
- docs/BUILD_IOS.md (build dokümantasyonu)
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
- iOS export hazırla (Xcode projesi). Arm64/IL2CPP/Metal.
"""

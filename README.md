# Undead Survivor

Unity와 C#으로 제작한 모바일 생존형 액션 게임입니다.

몰려오는 적을 피하며 자동 공격, 적 스폰, 오브젝트 풀링, 레벨업 보상, 결과 화면으로 이어지는 기본 플레이 루프를 구현했습니다. 이 저장소는 Unity 기반 모바일 게임의 주요 C# 게임 로직과 Android APK 빌드 파일을 정리한 저장소입니다.

## 프로젝트 소개

- 모바일 생존형 액션 게임
- 플레이어가 적을 피하고 자동 공격으로 처치
- 경험치 획득 후 레벨업 보상 선택
- 생존 시간과 처치 수를 기반으로 결과 화면 표시

## 개발 정보

| 항목 | 내용 |
|---|---|
| 프로젝트명 | Undead Survivor |
| 개발 형태 | 개인 프로젝트 |
| 개발 환경 | Unity, C# |
| 플랫폼 | Android |
| 주요 구현 | 플레이어 이동, 자동 공격, 적 스폰, 오브젝트 풀링, 레벨업, 결과 화면 |

## 기술 스택

- Unity
- C#
- Android
- Object Pooling
- 2D Game System

## 주요 기능

### 플레이어 이동

가상 조이스틱 입력을 기반으로 플레이어 이동을 처리하고, 이동 방향에 따라 애니메이션과 캐릭터 방향을 갱신했습니다.

### 자동 공격 시스템

플레이어 주변의 적을 탐지하고, 무기 타입에 따라 투사체 발사 또는 근접 공격이 동작하도록 구성했습니다.

### 적 스폰 시스템

게임 진행 흐름에 따라 적을 생성하고, 플레이어 주변 위치를 기준으로 스폰되도록 구현했습니다.

### 오브젝트 풀링

반복적으로 생성되는 적, 투사체, 아이템 오브젝트를 풀링 방식으로 관리하여 런타임 Instantiate/Destroy 비용을 줄였습니다.

### 레벨업 및 아이템 선택

경험치 획득 후 레벨업 시 무기 또는 장비 아이템을 선택할 수 있도록 구성했습니다.

### 게임 결과 처리

게임 종료 시 생존 시간, 처치 수, 획득 보상 등을 결과 화면에서 확인할 수 있도록 구현했습니다.

## 폴더 구조

```text
undead-survivor-unity/
├─ README.md
├─ APK/
│  └─ Undead-Survivor.apk
├─ Scripts/
│  ├─ AchievementManager.cs
│  ├─ AudioManager.cs
│  ├─ Bullet.cs
│  ├─ Character.cs
│  ├─ Enemy.cs
│  ├─ GameManager.cs
│  ├─ Gear.cs
│  ├─ Hand.cs
│  ├─ Hub.cs
│  ├─ Item.cs
│  ├─ ItemData.cs
│  ├─ LevelUp.cs
│  ├─ Player.cs
│  ├─ PoolManager.cs
│  ├─ Reposition.cs
│  ├─ Result.cs
│  ├─ Scanner.cs
│  ├─ Spawner.cs
│  └─ Weapon.cs
├─ Screenshots/
│  └─ gameplay.png
└─ Docs/
   └─ game-flow.md
```

## 주요 스크립트

| 파일 | 역할 |
|---|---|
| GameManager.cs | 게임 상태, 시간, 레벨, 결과 흐름 관리 |
| Player.cs | 플레이어 이동, 입력, 피격 처리 |
| Enemy.cs | 적 이동, 체력, 피격, 사망 처리 |
| Spawner.cs | 적 생성 및 스폰 흐름 관리 |
| PoolManager.cs | 적, 투사체, 아이템 오브젝트 풀링 관리 |
| Weapon.cs | 무기 동작 및 공격 처리 |
| Bullet.cs | 투사체 이동과 충돌 처리 |
| Scanner.cs | 주변 적 탐지 |
| Item.cs | 아이템 획득 처리 |
| ItemData.cs | 아이템 데이터 정의 |
| LevelUp.cs | 레벨업 선택 UI 처리 |
| Result.cs | 게임 종료 결과 화면 처리 |
| AudioManager.cs | 효과음 및 배경음 관리 |
| AchievementManager.cs | 업적 또는 진행 기록 관리 |

## 게임 흐름

1. 게임 시작
2. 플레이어 이동
3. 적 스폰
4. 주변 적 탐지
5. 자동 공격
6. 경험치 및 아이템 획득
7. 레벨업 보상 선택
8. 게임 종료 및 결과 화면 표시

## APK

`APK/Undead-Survivor.apk` 파일을 통해 Android 빌드 결과물을 확인할 수 있습니다.

## 구현 포인트

- 모바일 환경을 고려한 단순한 조작 구조
- 반복 생성 오브젝트를 풀링 방식으로 관리
- 주변 적 탐지를 통한 자동 공격 흐름 구성
- 게임 진행 → 성장 → 결과 화면으로 이어지는 플레이 루프 구현

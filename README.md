# Undead Survivor

Unity와 C#으로 제작한 모바일 생존형 액션 게임입니다.

몰려오는 적을 피하며 제한 시간 동안 생존하는 플레이를 중심으로 구성했습니다. 자동 공격, 적 스폰, 오브젝트 풀링, 레벨업 보상, 결과 화면으로 이어지는 기본 플레이 루프를 구현했으며, 이 저장소는 Unity 기반 모바일 생존형 액션 게임의 주요 C# 게임 로직과 Android APK 빌드 파일을 정리한 저장소입니다.

## 프로젝트 소개

Undead Survivor는 플레이어가 맵을 이동하며 적을 피하고, 주변 적을 자동으로 공격해 처치하는 모바일 생존형 액션 게임입니다. 적 처치를 통해 경험치를 얻고, 일정 경험치에 도달하면 레벨업 보상 UI에서 무기, 장비, 회복 아이템 중 하나를 선택해 캐릭터를 강화할 수 있습니다.

게임은 `GameManager`를 중심으로 시작, 진행 시간, 체력, 경험치, 처치 수, 생존 성공 및 사망 결과를 관리합니다. 캐릭터 ID에 따라 이동 속도, 공격 주기, 피해량, 투사체 수 보정이 적용되며, `AchievementManager`는 처치 수와 생존 성공 조건을 기준으로 캐릭터 해금 상태를 `PlayerPrefs`에 저장합니다.

- 모바일 생존형 액션 게임
- 플레이어 이동과 자동 공격 기반 전투
- 적 처치를 통한 경험치 획득과 레벨업
- 레벨업 시 보상 아이템 선택 및 캐릭터 강화
- 제한 시간 생존 또는 체력 0에 따른 결과 화면 표시
- 처치 수, 생존 결과, 캐릭터 해금 상태 관리

## 개발 정보

| 항목 | 내용 |
|---|---|
| 프로젝트명 | Undead Survivor |
| 개발 형태 | 개인 프로젝트 |
| 개발 기간 | 약 2주 |
| 장르 | 모바일 생존형 액션 |
| 개발 환경 | Unity, C# |
| 플랫폼 | Android |
| 주요 구현 | 플레이어 이동, 자동 공격, 적 스폰, 오브젝트 풀링, 레벨업, 결과 화면 |

## 기술 스택

- Unity
- C#
- Android
- Object Pooling
- 2D Game System
- Unity Physics2D
- ScriptableObject
- PlayerPrefs
- Unity UI

## 주요 기능

### 플레이어 이동

`Player.cs`는 Unity 입력 축인 `Horizontal`, `Vertical` 값을 받아 플레이어 이동 방향을 계산합니다. `Rigidbody2D.MovePosition`으로 이동을 처리하고, `Character.Speed` 값을 곱해 선택한 캐릭터에 따른 이동 속도 보정을 적용합니다.

이동 중에는 `Animator`의 `Speed` 값을 갱신하고, 좌우 입력 방향에 따라 `SpriteRenderer.flipX`를 변경해 캐릭터 방향을 전환합니다. 플레이어가 적과 충돌한 상태에서는 체력이 지속적으로 감소하며, 체력이 0 이하가 되면 사망 애니메이션과 게임 오버 흐름으로 이어집니다.

### 자동 공격 시스템

`Scanner.cs`는 `Physics2D.CircleCastAll`을 사용해 플레이어 주변의 적을 탐지하고, 가장 가까운 대상을 `nearestTarget`으로 저장합니다. `Weapon.cs`는 무기 타입에 따라 근접 공격과 원거리 공격을 다르게 처리합니다.

근접 무기는 플레이어 주변에 배치된 투사체를 회전시키는 방식으로 동작하고, 원거리 무기는 일정 주기마다 가장 가까운 적 방향으로 총알을 발사합니다. `Bullet.cs`는 피해량, 관통 횟수, 이동 방향을 받아 투사체 이동과 적 충돌 후 비활성화 처리를 담당합니다.

### 적 스폰 시스템

`Spawner.cs`는 게임 진행 시간을 기준으로 현재 스폰 레벨을 계산하고, `SpawnData`에 정의된 스폰 간격, 적 체력, 이동 속도, 스프라이트 타입을 사용해 적을 생성합니다. 적은 `spawnPoint` 중 하나에 배치되며, 생성 자체는 `PoolManager`를 통해 재사용 가능한 오브젝트를 가져오는 방식으로 처리합니다.

`Enemy.cs`는 활성화될 때 플레이어를 추적 대상으로 설정하고, `FixedUpdate`에서 플레이어 방향으로 이동합니다. 총알에 맞으면 체력이 감소하고, 체력이 0 이하가 되면 처치 수와 경험치를 증가시킨 뒤 사망 애니메이션 후 비활성화됩니다.

### 오브젝트 풀링

`PoolManager.cs`는 프리팹 배열을 기준으로 타입별 오브젝트 풀을 관리합니다. 요청된 타입의 비활성 오브젝트가 있으면 `SetActive(true)`로 재사용하고, 없을 때만 새로 생성해 풀에 추가합니다.

적, 투사체처럼 반복적으로 생성되는 오브젝트는 `SetActive(false)`로 비활성화된 뒤 다시 재사용됩니다. 이 구조를 통해 플레이 중 `Instantiate` 호출을 필요한 시점으로 제한하고, 반복적인 생성과 제거 비용을 줄였습니다.

### 레벨업 및 아이템 선택

`GameManager.cs`는 적 처치 후 `GetExp()`를 호출해 경험치를 누적하고, `nextExp` 조건에 도달하면 레벨을 올린 뒤 `LevelUp` UI를 표시합니다. `LevelUp.cs`는 보상 후보 중 3개를 무작위로 활성화하고, 선택 중에는 `Time.timeScale`을 0으로 설정해 게임 진행을 멈춥니다.

`Item.cs`는 선택한 아이템 타입에 따라 무기 생성, 무기 강화, 장비 생성, 장비 강화, 체력 회복을 처리합니다. `ItemData.cs`는 ScriptableObject 기반으로 아이템 타입, 이름, 설명, 아이콘, 기본 피해량, 레벨별 수치, 투사체 프리팹을 정의합니다. `Gear.cs`는 장갑 아이템으로 무기 공격 주기를 줄이거나 신발 아이템으로 플레이어 이동 속도를 높이는 역할을 합니다.

### 게임 상태 및 결과 처리

`GameManager.cs`는 게임 시작, 일시정지, 재개, 사망, 생존 성공, 재시작, 종료 흐름을 관리합니다. 게임 시간이 `maxGameTime`에 도달하면 생존 성공 처리를 실행하고, 체력이 0 이하가 되면 게임 오버 처리를 실행합니다.

`Result.cs`는 결과 화면에서 사망 또는 생존 성공 타이틀을 표시합니다. `AchievementManager.cs`는 처치 수 100 이상, 제한 시간 생존 성공 조건을 확인해 캐릭터 해금 상태를 저장하고, 해금 안내 UI를 표시합니다.

### 사운드 및 UI 관리

`AudioManager.cs`는 배경음과 효과음을 분리해 관리합니다. 배경음은 반복 재생으로 처리하고, 효과음은 여러 `AudioSource` 채널을 사용해 동시에 재생될 수 있도록 구성했습니다. 레벨업 화면에서는 배경음 필터 효과를 켜고, 선택 후 다시 끄는 흐름도 포함되어 있습니다.

`Hub.cs`는 경험치, 레벨, 처치 수, 남은 시간, 체력 UI를 갱신합니다. `Hand.cs`는 캐릭터 방향에 맞춰 손 위치와 정렬 순서를 보정하고, `Reposition.cs`는 맵 오브젝트와 적이 플레이 영역을 벗어났을 때 위치를 재배치합니다.

## 게임 플레이 흐름

1. 캐릭터 선택: `GameManager.GameStart(int id)`에서 선택한 캐릭터 ID를 저장하고 플레이어를 활성화합니다.
2. 게임 시작: 체력과 게임 상태를 초기화하고, 배경음과 선택 효과음을 재생합니다.
3. 플레이어 이동: 입력 축 값을 기반으로 `Rigidbody2D` 이동과 애니메이션 방향을 갱신합니다.
4. 적 스폰: 진행 시간에 맞춰 `Spawner`가 현재 스폰 데이터를 선택하고 적을 배치합니다.
5. 주변 적 탐지: `Scanner`가 탐지 범위 안의 적 중 가장 가까운 대상을 찾습니다.
6. 자동 공격: `Weapon`이 무기 타입에 따라 회전 공격 또는 원거리 발사를 실행합니다.
7. 적 처치 및 경험치 획득: `Enemy`가 체력 0 이하가 되면 처치 수와 경험치가 증가합니다.
8. 레벨업 보상 선택: 경험치 조건을 만족하면 게임이 잠시 멈추고 보상 아이템 3개가 표시됩니다.
9. 생존 성공 또는 사망: 제한 시간에 도달하면 생존 성공, 체력이 0 이하가 되면 사망으로 처리됩니다.
10. 결과 화면 표시: `Result` UI에서 생존 성공 또는 사망 결과를 보여줍니다.

## 스크린샷

![Undead Survivor Gameplay](Screenshots/gameplay.png)

## 폴더 구조

```text
undead-survivor-unity/
├─ README.md
├─ .gitignore
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
├─ Docs/
│  └─ game-flow.md
├─ Screenshots/
│  ├─ README.md
│  └─ gameplay.png
├─ content/
│  ├─ codex-prompt-for-portfolio.md
│  └─ site-copy-undead-survivor.md
└─ images/
   ├─ feature-character-select.png
   ├─ feature-combat-system.png
   ├─ feature-levelup-system.png
   ├─ feature-result-system.png
   ├─ portfolio-card-undead-survivor.png
   ├─ portfolio-hero-undead-survivor.png
   ├─ undead-all-characters.png
   ├─ undead-character-select.png
   ├─ undead-combat-bullet.png
   ├─ undead-dead-result.png
   ├─ undead-gameplay-main.png
   ├─ undead-levelup-select.png
   └─ undead-survived-result.png
```

## 주요 스크립트

| 파일 | 역할 |
|---|---|
| `GameManager.cs` | 게임 시작, 진행 시간, 체력, 경험치, 레벨, 결과 흐름 관리 |
| `Player.cs` | 입력 처리, 플레이어 이동, 방향 전환, 피격 및 사망 처리 |
| `Character.cs` | 선택 캐릭터 ID에 따른 능력치 보정값 제공 |
| `Scanner.cs` | 주변 적 탐지 및 가장 가까운 대상 선택 |
| `Weapon.cs` | 근접 무기 회전 공격과 원거리 무기 발사 처리 |
| `Bullet.cs` | 투사체 피해량, 이동, 관통 횟수, 비활성화 처리 |
| `Enemy.cs` | 적 추적 이동, 피격, 넉백, 사망, 경험치 지급 처리 |
| `Spawner.cs` | 진행 시간 기반 적 생성 레벨과 스폰 위치 관리 |
| `PoolManager.cs` | 프리팹 타입별 오브젝트 풀 생성 및 재사용 |
| `ItemData.cs` | ScriptableObject 기반 아이템 데이터 정의 |
| `Item.cs` | 아이템 UI 표시, 선택 처리, 무기/장비/회복 적용 |
| `LevelUp.cs` | 레벨업 보상 후보 표시와 게임 일시정지/재개 처리 |
| `Gear.cs` | 장비 효과 적용, 공격 주기 및 이동 속도 보정 |
| `Result.cs` | 생존 성공 또는 사망 결과 타이틀 표시 |
| `AchievementManager.cs` | 캐릭터 해금 조건 확인과 진행 상태 저장 |
| `AudioManager.cs` | 배경음, 효과음 채널, 사운드 효과 관리 |
| `Hub.cs` | 경험치, 레벨, 처치 수, 시간, 체력 UI 갱신 |
| `Hand.cs` | 캐릭터 방향에 따른 손 위치, 회전, 정렬 보정 |
| `Reposition.cs` | 플레이 영역 기준 맵 오브젝트와 적 위치 재배치 |

## APK

`APK/Undead-Survivor.apk` 파일을 통해 Android 빌드 결과물을 확인할 수 있습니다. 이 저장소는 주요 C# 게임 로직과 APK 파일을 중심으로 정리되어 있으며, Unity 에디터 프로젝트의 모든 작업 폴더를 담은 형태는 아닙니다.

## 문서 및 이미지 자료

- `Docs/game-flow.md`: 게임 시작부터 종료까지의 흐름을 요약한 문서입니다.
- `Screenshots/gameplay.png`: README에서 사용하는 대표 게임 화면 이미지입니다.
- `content/`: 포트폴리오 사이트에 옮겨 쓸 수 있는 프로젝트 소개 문구를 정리한 폴더입니다.
- `images/`: 포트폴리오 카드, 상세 페이지, 기능 설명에 사용할 수 있는 이미지 자료를 정리한 폴더입니다.

## 구현 포인트

### 오브젝트 풀링으로 반복 생성 비용 감소

`PoolManager.cs`는 프리팹 타입별 리스트를 만들고, 요청된 타입의 비활성 오브젝트를 먼저 찾아 `SetActive(true)`로 재사용합니다. 재사용할 오브젝트가 없을 때만 `Instantiate`로 새 오브젝트를 생성하므로, 적과 투사체처럼 반복적으로 등장하는 오브젝트의 생성 비용을 줄이는 구조입니다.

### Scanner 기반 자동 공격 흐름

`Scanner.cs`는 `Physics2D.CircleCastAll`로 주변 적을 탐지하고 가장 가까운 대상을 `nearestTarget`으로 저장합니다. `Weapon.cs`는 이 대상을 기준으로 원거리 투사체를 발사하거나, 근접 무기일 경우 플레이어 주변에 배치된 투사체를 회전시킵니다. `Bullet.cs`는 피해량과 관통 횟수를 받아 충돌 후 비활성화까지 처리합니다.

### GameManager 중심의 게임 상태 관리

`GameManager.cs`는 `isLive`, `gameTime`, `health`, `level`, `kill`, `exp` 값을 중심으로 게임 진행 상태를 관리합니다. `GameStart`, `GameOver`, `GameVictory`, `GetExp`, `Stop`, `Resume` 메서드가 시작, 사망, 생존 성공, 경험치 획득, 일시정지와 재개 흐름을 연결합니다.

### 아이템 데이터 기반 성장 구조

`ItemData.cs`는 ScriptableObject로 아이템 타입, 설명, 아이콘, 피해량, 레벨별 수치, 투사체 프리팹을 정의합니다. `LevelUp.cs`는 레벨업 시 보상 후보 3개를 표시하고, `Item.cs`는 선택된 아이템에 따라 무기 생성, 무기 강화, 장비 효과, 체력 회복을 처리합니다. `Gear.cs`는 장갑과 신발 장비 효과를 실제 무기 공격 주기와 플레이어 이동 속도에 반영합니다.

### 모바일 게임에 맞춘 단순 조작 구조

`Player.cs`는 `Input.GetAxisRaw("Horizontal")`, `Input.GetAxisRaw("Vertical")` 입력 축을 받아 이동만 처리하고, 공격은 `Scanner`와 `Weapon` 흐름에 맡깁니다. 플레이어 조작은 이동에 집중하고 전투는 자동으로 진행되도록 구성해 모바일 생존형 액션 게임에 맞는 단순한 플레이 구조를 만들었습니다.

# 포트폴리오 사이트 반영용 Codex 프롬포트

현재 포트폴리오 사이트에 `Undead Survivor` 프로젝트를 추가하거나 기존 프로젝트 카드/상세 내용을 보강해줘.

요구사항:
- 기존 사이트의 디자인, 레이아웃, 클래스명, 섹션 구조는 최대한 유지한다.
- 새 디자인을 만들지 말고 기존 포트폴리오 카드 스타일에 맞춰 텍스트와 이미지만 반영한다.
- `복구`, `백업`, `유실`, `포맷` 같은 표현은 사용하지 않는다.
- Unity 전체 프로젝트라고 과장하지 말고, 주요 게임 로직과 Android APK 빌드 중심 프로젝트로 자연스럽게 소개한다.
- 이미지 파일은 `images/portfolio-card-undead-survivor.png`, `images/portfolio-hero-undead-survivor.png`, `images/feature-*.png`를 사용한다.

프로젝트 카드 문구:
- 제목: Undead Survivor
- 분류/기간: Unity 모바일 게임 · 약 2주
- 설명: 적 스폰, 자동 공격, 오브젝트 풀링, 레벨업 보상을 구현한 모바일 생존형 액션 게임입니다.
- 기술: Unity, C#, Android, Object Pooling
- 핵심 기능: 플레이어 이동, 자동 공격, 적 스폰, 레벨업 보상, 결과 화면

상세 페이지 소개:
Unity와 C#으로 제작한 도트 그래픽 모바일 생존형 액션 게임입니다. 플레이어는 몰려오는 언데드를 피하면서 자동 공격으로 적을 처치하고, 경험치를 모아 레벨업 보상을 선택하며 제한시간 동안 생존해야 합니다.

구현 포인트:
1. Scanner, Weapon, Bullet을 활용한 자동 공격 흐름
2. PoolManager 기반 Enemy/Bullet 재사용 구조
3. 경험치 누적 후 레벨업 보상 선택 UI
4. PlayerPrefs 기반 캐릭터 해금 상태 저장
5. GameManager 중심의 생존/사망 결과 처리

이미지 배치:
- 카드 썸네일: portfolio-card-undead-survivor.png
- 상세 상단: portfolio-hero-undead-survivor.png
- 기능 설명 이미지: feature-character-select.png, feature-combat-system.png, feature-levelup-system.png, feature-result-system.png

수정 후 요약:
- 수정한 파일 목록
- 카드/상세 페이지 반영 내용
- 이미지 경로 변경 내용
- 커밋 메시지 추천

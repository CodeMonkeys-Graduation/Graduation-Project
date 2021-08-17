# Turn Based Strategy AI Game

> 이 리포지토리는 홍익대학교 컴퓨터공학과 4학년 [원동현](https://github.com/wonAdam)과 [박솔휘](https://github.com/solhwi)의 졸업 프로젝트 저장소입니다.

> 에디터상에서 보고 싶은 경우 에셋들을 임포트해야합니다. 아래 에셋들을 임포트하여 "Assets/_AssetStore" 경로에 넣어주세요.
- [Toony Tiny RTS Set](https://assetstore.unity.com/packages/3d/characters/toony-tiny-rts-set-135258)
- [RPG Monster Wave Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-wave-polyart-157652)
- [Cube World - Proto Series](https://assetstore.unity.com/packages/3d/environments/cube-world-proto-series-144159)
- [RPG Monster Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762)
- [GUI PRO Kit - Fantasy RPG](https://assetstore.unity.com/packages/2d/gui/gui-pro-kit-fantasy-rpg-170168)
- [Epic Toon VFX 2](https://assetstore.unity.com/packages/vfx/particles/spells/epic-toon-vfx-2-157651)

## 개발 일지: [Youtube](https://youtube.com/playlist?list=PLUnoDCtQAT-4vgxizujnRzX_ROiHMac2x)

## 기본 설명

게임을 시작하면 자원의 양이 주어집니다.

자원으로 이번 게임에서 사용할 유닛을 고용합니다. 

그리고 게임을 시작하면 유닛을 일정 공간안에 자유롭게  배치할 수 있습니다. 

배치가 완료되면 게임시작을 눌러 게임을 시작합니다. 

![게임 흐름 다이어그램](https://raw.githubusercontent.com/CodeMonkeys-Graduation/Graduation-Project/master/%EA%B2%8C%EC%9E%84%EA%B8%B0%EB%B3%B8%ED%9D%90%EB%A6%84.png)

게임이 시작되면 아군유닛 적군유닛 모든유닛들이 민첩성 스탯에 따라 턴의 순서가 정해집니다. 각 턴마다 해당 유닛의 주인 플레이어가 유닛의 액션을 취하게 할 수 있습니다.

각 액션은 행동력을 소모합니다. 각 유닛의 행동력은 행동력 스탯에 따라 각턴마다 리셋됩니다.  

## Features

### Path Finding Algorithm

캐릭터에게 특정한 블락으로 이동시키는 명령을 할 시, BFS를 사용하여 유닛이 있는 모든 블락들의 경로를 갱신합니다. 

[Breadth First Search or BFS for a Graph - GeeksforGeeks](https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/)

### AI 

저희가 사용할 AI 테크닉은 GOAP을 변형한 모델입니다. 

가용가능한 Action들로 Planner는 모든 경우의 수를 트리 자료구조로 Plan합니다. 

Tree의 Leaf 노드들은 최종 Score를 갖고 최대 Score의 Leaf Node에서부터 Root 노드까지를 최적의 Plan으로 정합니다.

해당 Plan을 캐릭터에게 리턴하고 캐릭터는 해당 Plan에 따라 행동합니다. 

[GOAP](https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793)

### 게임 시스템

게임 시스템으로는 턴제, 타일맵, 클릭매니저 등등이 있을 것으로 예상됩니다. 

해당 매니저 클래스들끼리의 유기적이고 커플링이 적은 시스템을 구현하기위해 다양하고 적절한 디자인 패턴들을 녹여낼 예정입니다.

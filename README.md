# LastLeaf
https://github.com/chojongwan/LastLeaf_TRPG
처음에 깃허브를 만들때 솔루션 파일을 포함하지 않고 진행하여 이전 히스토리가 모조리 없어진지라 링크를 첨부해 이전 히스토리를 볼 수 있게 하겠습니다. <hr>

# 게임 소개
> Visual Studio로 코딩한 C# 콘솔을 이용하여 진행하는 Text RPG

# 게임 시작전
### 이름 생성
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/5f285a57-6bda-4e94-9b43-dc0654297084)
### 직업 선택
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/457a6a1d-bb54-493f-b0a4-d87a2fd174c4)


1. 게임 시작 화면
게임 실행 시 플레이어의 이름을 입력받습니다. 또한 플레이어가 원하는 직업을 선택할 수 있습니다. 각 직업마다 서로 다른 기본 공격력과 방어력을 가지고 있습니다.
이 부분은 게임을 시작했을 때 사용자에게 보여지는 화면입니다. 사용자에게 게임의 간단한 소개와 시작할 수 있는 행동을 알려줍니다. 예제에서는 각 숫자에 대응하는 행동을 선택할 수 있도록 구성되어 있습니다.<hr>
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/18bb7a24-03d5-4929-b7bc-35d6cc945d27)
<hr>

# 게임 진행

## 게임 종료
0번을 누르면 게임을 종료할 수 있습니다.

## 상태 보기
1번을 누르면 현재 캐릭터의 상태를 볼 수 있습니다.
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/4132de1b-f989-449b-aff3-096ca22ed7a1)

## 인벤토리
2번을 입력 시 현재 보유 중인 아이템 목록을 표시합니다. 아이템을 장착 또는 해제할 수 있습니다.
하이픈(-)을 입력 시 소모품 가방이 표시되며, 사용할 소모품을 선택하여 사용 가능합니다.
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/cd297816-0914-4165-947b-75e11410cea2)
### 소모품 가방
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/40c016b6-bd93-4e43-beb3-5542800d081a)


## 상점
3번을 입력 시 상점으로 이동하며 현재 보유한 골드와 구매할 수 있는 아이템 목록이 표시됩니다.
아이템 구매와 판매가 가능합니다.
![image](https://github.com/chojongwan/TRPG/assets/79524474/3e58dc71-0968-4964-be38-7c8e3ccf8b26)<hr>

원하는 아이템을 구매하면 품절된 아이템은 [구매 완료] 라는 표시와 함께 다시 구매할 수 없습니다.
그리고 보유하고 있는 아이템을 절반의 가격으로 판매할 수 있고 판매한 아이템은 [구매 완료] 표시가 없어지고 다시 구매할 수 있습니다.

## 던전
4번을 입력 시 던전으로 이동하며, 현재 HP와 Gold를 확인하고 전투를 시작할 수 있습니다.
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/75f32c22-d4ca-4384-bc22-a75cf83a903f)

전투 시작 시 자신의 정보와 현재 스테이지가 표시되며 랜덤으로 등장하는 몬스터를 처치할 수 있습니다.
스킬 사용 시 자신이 선택한 직업의 스킬을 사용할 수 있고, 적을 처치할 때마다 랜덤한 골드를 얻을 수 있습니다. 모든 적을 처치하게 되면 다음 스테이지로 넘어갑니다.
던전을 클리어하며 스테이지가 올라가다 보면 특정 스테이지에서 보스가 등장하게 됩니다. 최종 보스를 처치하면 클리어 화면을 볼 수 있습니다.

## 레벨 시스템
레벨이 1 오르면 공격력 +1, 방어력 +2가 오릅니다.

## 게임 오버
HP가 0이 되면 모든 돈을 잃고 던전 입구에서 깨어납니다.
![image](https://github.com/chojongwan/TRPG/assets/79524474/529358ae-ec28-45e9-8c97-f267159ec621) <hr>

## 휴식하기
5번을 입력 시 휴식을 할 수 있고 500G를 지불하여 HP를 100까지 채웁니다. 500G가 없거나 HP가 가득 채워져있을 경우엔 휴식할 수 없습니다.
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/511ee88b-19c2-42cf-813d-476f5e2e0b6f)

## 퀘스트
6번을 입력 시 퀘스트 목록을 확인할 수 있습니다. 원하는 퀘스트 번호를 입력하면 퀘스트 선택을 하실 수 있습니다.
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/90029943-9248-4852-997a-b1478024e56b)
퀘스트 수락
![image](https://github.com/chojongwan/LastLeaf/assets/79524474/75d23e35-7133-407b-9fbf-42662623c5e1)

퀘스트 달성 시 골드를 얻을 수 있습니다.
# 게임 클리어
15스테이지를 클리어하면 게임을 클리어하고 던전에서 나갑니다.

## 저장하기, 불러오기
7번을 누르면 저장하고 8번을 누르면 마지막에 저장한 정보를 불러옵니다.
게임을 초기화하고 싶을 경우 처음 실행할 때 불러오기가 아닌 저장하기를 누르면 초기화가 가능합니다.

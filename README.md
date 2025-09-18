# Twin Travelers
![타이틀](https://github.com/user-attachments/assets/d02ef09c-3fb6-425e-9aec-39c86dad4516)
> 2022 전국기능경기대회 게임개발 과제에서 멀티 플레이 및 설정, 많은 스테이지 등 여러 기능을 추가한 리마스터 버전입니다.

# 플레이 화면
![플레이 화면](https://github.com/user-attachments/assets/662405fd-b1f1-4e66-b3c8-845db1a698a4)

# 조작법
![게임 조작법](https://github.com/user-attachments/assets/f9b4f9fc-60c7-4fdd-91ee-c87d2ab06254)

# 게임 순서도
<details>
<summary>게임 순서도 보기</summary>  
<img width="500" height="600" alt="image" src="https://github.com/user-attachments/assets/1a134bb9-1b04-46c5-b52a-25d7d461e8ec" />
</details>  

# 장애물 설명

| **이름** | **이미지** | **기능** |
|----------|------------|----------|
| 장애물 | <img width="124" height="72" alt="image" src="https://github.com/user-attachments/assets/4697c4ad-3664-47f4-81fb-c4c75a3237dd" />| 플레이어가 닿으면 죽는 오브젝트입니다. |
| └ 회전 | <img width="100" height="200" src="https://github.com/user-attachments/assets/e7c0e990-d4db-4b70-83f1-3c85e8c79fde" /> | 일정 속도로 회전하는 장애물입니다. |
| └ 프레스 | <img width="100" height="200" src="https://github.com/user-attachments/assets/06d0338c-d6b7-41e9-af1c-175c69b333e8" /> | 일정 사이클마다 찍는 장애물입니다. |
| └ 적 캐릭터 | <img width="155" height="110" src="https://github.com/user-attachments/assets/b259b962-ebaa-4718-ae4b-c270cde25444" /> | 일정 경로로 이동하는 장애물입니다. |
| 레이저 장애물 | <img width="103" height="171" alt="image" src="https://github.com/user-attachments/assets/1ce2e220-a400-45ba-b1cd-569fb5287406" /> | 주기적으로 켜졌다 꺼지는 레이저. 타이밍을 맞춰 통과해야 합니다. |
| └ 이동하는 레이저 | <img width="103" height="171" alt="image" src="https://github.com/user-attachments/assets/1ce2e220-a400-45ba-b1cd-569fb5287406" /> | 일정 패턴으로 움직이며, 타이밍을 맞춰 통과해야 하는 레이저입니다. |
| 함정 바닥 | <img width="155" height="30" alt="shelf_2" src="https://github.com/user-attachments/assets/eecc1dae-fb87-4397-a4c3-e8e2105e96bd" /> | 일정 무게 이상이 올라가면 무너지는 바닥입니다. (총합 2 이상이면 부러짐) |
| 숨겨진 함정 | <img width="200" height="160" alt="image" src="https://github.com/user-attachments/assets/00a40cdb-1522-4e7c-9d04-1d3214fc555e" /> | 평소엔 보이지 않다가 플레이어가 가까이 오면 나타나는 함정입니다. |

# 기믹 오브젝트 설명

| **이름** | **이미지** | **기능** |
|----------|------------|----------|
| 점프패드 | <img width="213" height="313" alt="image" src="https://github.com/user-attachments/assets/2d3bff33-3e06-44ef-b126-2bbbe9183a01"> | 플레이어의 점프력을 올려주는 오브젝트입니다. |
| 플로팅 플랫폼 | <img width="155" height="30" alt="shelf_2" src="https://github.com/user-attachments/assets/eecc1dae-fb87-4397-a4c3-e8e2105e96bd" /> | 공중에 떠다니며 일정한 패턴으로 움직이는 플랫폼입니다. |
| 빛나는 플랫폼 | <img width="155" height="30" alt="shelf_2" src="https://github.com/user-attachments/assets/eecc1dae-fb87-4397-a4c3-e8e2105e96bd" /> | 일정 시간 동안 나타났다 사라지는 플랫폼입니다. |
| 문 | <img width="250" height="134" alt="image" src="https://github.com/user-attachments/assets/af27085f-c0fc-4802-a308-89f380485419" />| 평시에는 열림/닫힘 상태이나, 상호작용으로 반대 상태로 전환됩니다. |
| └ 레버 | <img width="232" height="168" alt="image" src="https://github.com/user-attachments/assets/2c970706-3c84-487d-bf0a-c6d5b1440279" /> | 플레이어나 오브젝트가 위에 올라서면 문이 열립니다. |
| └ 버튼 | <img width="208" height="159" alt="image" src="https://github.com/user-attachments/assets/ccfeff98-7904-4851-b0f8-93bd6782405b" /> | 플레이어나 오브젝트가 위에 올라서면 문이 열립니다. |
| └ 열쇠 | <img width="174" height="131" alt="image" src="https://github.com/user-attachments/assets/2462e9f0-f570-44c6-b824-7c2161dc11dd" /> | 열쇠를 획득해 문을 열 수 있습니다. |
| └ 협력 스위치 | <img width="260" height="100" alt="image" src="https://github.com/user-attachments/assets/cea08088-556e-4a35-bd91-3123fdb488de" /> | 두 명이 동시에 스위치를 눌러야 작동하는 장치입니다. |
| 선풍기 | <img width="153" height="161" alt="image" src="https://github.com/user-attachments/assets/d25bcf6a-fa36-403b-858d-ff02cabfe2c1" /> | 바람으로 범위 내 캐릭터를 밀어내거나 점프 거리를 늘립니다. |
| 중력 반전 포탈 | <img width="223" height="276" alt="image" src="https://github.com/user-attachments/assets/6360fbff-5b89-4af3-be0a-6f9b68a74c43" /> | 포탈을 통과하면 중력이 반전됩니다. |
| 마그넷 | <img width="138" height="137" alt="image" src="https://github.com/user-attachments/assets/3c3e4fae-f4e4-4f95-9aa7-9a13fff01fee" /> | 오브젝트를 끌어당기거나 밀어냅니다. |
| 트램펄린 | <img width="170" height="80" alt="image" src="https://github.com/user-attachments/assets/c8c3a918-d2aa-4c08-a299-9fb2a06fb827" /> | 플레이어를 높은 곳으로 점프시킵니다. |
| 풍선 타워 | <img width="203" height="330" alt="image" src="https://github.com/user-attachments/assets/408ec057-0446-4dd0-8c09-c4b7bca63aa8" /> | 풍선을 잡고 하늘을 날아 장애물을 피할 수 있습니다. |

# 시연 영상
[YouTube 링크](https://www.youtube.com/watch?v=L-SwHIiYgvA)  

# 사용한 에셋
<details>
<summary><strong>목록 보기(토글)</strong></summary>

<br>

### Unity Assets Store  
[Danil Chernyaev] 2D Platformer Tileset  
[Gamemaster Audio] Pro Sound Collection  
[Photon Engine] PUN 2 - FREE  

### Opengameart  
[Unicaegames] Keyboard Soundpack #1  

### itch.io  
[rubberduck] Firework SFX  
[SpikerMan] Animated Arrows / Cursors  

### Studio MDHR  
Trampoline from Cuphead ripped by "DogToon64"  

### Kenney  
Cursor Pack  

### RixFont  
Rix X 수박양  

</details>

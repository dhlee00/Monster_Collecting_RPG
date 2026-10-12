# 포켓몬스터 모작

인기게임인 포켓몬스터를 모작한 게임입니다.

<img width="426" height="240" alt="1012" src="https://github.com/user-attachments/assets/0559024d-19c6-4c06-9de5-9e11e45ebeda" />

---

📺 [시연 영상 (YouTube)](링크)<br>
🎮 [빌드 파일 다운로드 (Google Drive)](https://drive.google.com/file/d/1_Wsr3XtI5ea3kRhgjeTB3sUYH7AQzini/view?usp=drive_link)<br>
📄 [기획서](https://github.com/dhlee00/Pokemon_Imitation/blob/main/%ED%8F%AC%EC%BC%93%EB%AA%AC%EC%8A%A4%ED%84%B0%20%EB%AA%A8%EC%9E%91%20%EA%B8%B0%ED%9A%8D%EC%84%9C.pdf)

---

## 프로젝트 개요

- **개발 형태:** 개인 프로젝트
- **개발 기간:** 2025.01 ~ 2025.05 (약 5개월)
- **사용 엔진 및 언어:** Unity, C#

---

## 주요 기능

<details><summary><b>일반 적 행동 패턴 설계</b></summary>

<img width="654" height="368" alt="KakaoTalk_20261008_211834369" src="https://github.com/user-attachments/assets/3dd656b4-a8df-4376-8e50-08bb0f0454c4" />
<img width="654" height="368" alt="KakaoTalk_20261008_213039812" src="https://github.com/user-attachments/assets/967c52fe-c7e8-436e-84b7-c881826a706a" />


#### 코드 [(전체 코드)](Scripts/EnemyManager.cs)

* 플레이어 감지
  ```csharp
  private void Update()
  {
      // 플레이어 X, Y 거리 차이 계산
      detectDisX = Mathf.Abs(pm.transform.position.x - transform.position.x);
      detectDisY = Mathf.Abs(pm.transform.position.y - transform.position.y);

      // 이동 방향에 따른 레이의 발사 위치 설정
      if (dir > 0)
      {
          gizmos = 1;
      }

      else if (dir < 0)
      {
          gizmos = -1;
      }

      // 전방 지형 및 플레이어 감지를 위한 레이 발사
      Debug.DrawRay(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down * 1.8f, Color.red);
      hit = Physics2D.Raycast(transform.position + new Vector3(gizmos * 0.9f, 0, 0), Vector2.down, 1.8f, mask);
  }
  ```
  > 실시간으로 변하는 플레이어와의 거리를 매 프레임 측정하여 상태 전환의 기준으로 삼았습니다.<br>
  적의 이동 방향의 전방 아래로 레이를 발사해 지형이 끊기는 것을 감지하여 이동을 제어하도록 구현했습니다.
  <br>
  
* 행동 처리
  ```csharp
  private void Update()
  {
      // 이동
      rig.velocity = new Vector2(dir * speed, rig.velocity.y);

      // 적 상태에 따른 행동 함수 호출
      switch (eState)
      {
          case EnemyState.Idle: Idle(); break;
          case EnemyState.Walk: Walk(); break;
          case EnemyState.Attack: Attack(); break;
          case EnemyState.Damaged: break;
          case EnemyState.Die: Die(); break;
      }
  }
  ```
  > 변수에 저장해둔 플레이어와의 거리 값을 기준으로 대기, 이동, 공격 중 한가지를 실행합니다.
  <br>
  
#### 설계 의도
> 플레이어가 다가오면 공격해 재화를 뺏고 멀어지면 대기하는 패턴을 구현하기 위해 유한 상태 머신을 사용했습니다.<br>
또한 이동 중 지형이 끊기는 곳에서 부자연스럽게 떨어지는 문제를 해결하기 위해 전방 아래로 레이캐스트를 쏴서 지형을 감지하도록 처리했습니다.

---

</details>

<details><summary><b>보스 행동 패턴 설계</b></summary>

<img width="654" height="368" alt="KakaoTalk_20261010_220929222" src="https://github.com/user-attachments/assets/aa9883c7-71ec-493b-abc7-4bb40c545292" />
<img width="654" height="368" alt="KakaoTalk_20261010_220929222_01" src="https://github.com/user-attachments/assets/2acb02f5-503e-4728-8dfe-cd88391fa483" />
<img width="654" height="368" alt="KakaoTalk_20261010_220929222_02" src="https://github.com/user-attachments/assets/481594fe-f8c1-4ce9-9d58-af85154915da" />
<img width="654" height="368" alt="KakaoTalk_20261010_220929222_03" src="https://github.com/user-attachments/assets/aa634fd6-4246-43eb-82eb-ea22c13a5146" />
<img width="654" height="368" alt="KakaoTalk_20261010_220929222_04" src="https://github.com/user-attachments/assets/581d66b3-f13f-4fc6-a83f-a874cbac51f2" />
<img width="654" height="368" alt="KakaoTalk_20261010_220929222_05" src="https://github.com/user-attachments/assets/9703394a-7046-41b6-a4d6-b0110cbaec31" />

#### 코드 [(전체 코드)](Scripts/BossManager.cs)

* 플레이어 감지
  ```csharp
  private void Update()
  {
      // 플레이어 감지
      detectDisX = Mathf.Abs(pm.transform.position.x - transform.position.x);
      detectDisY = Mathf.Abs(pm.transform.position.y - transform.position.y);
  
      // 보스의 상태에 따라 다른 함수 호출
      switch (bState)
      {
          case BossState.Idle: Idle(); break;
          case BossState.Walk: Walk(); break;
          case BossState.CloseAttack: CloseAttack(); break;
          case BossState.FarAttack: FarAttack(); break;
          case BossState.Damaged: break;
          case BossState.Die: Die(); break;
      }
  }
  ```
  > 적과 마찬가지로 플레이어와의 거리를 매 프레임 측정하여 상태 전환의 기준으로 삼았습니다.
  <br>

* 원거리 공격 패턴
  ```csharp
  void FarAttack()
  {
      // 플레이어가 거기는 가까운데 위에 있을 때
      if (detectDisX <= 8 && detectDisY > 2.2f)
      {
          // 근거리 공격 상태로 전환
          bState = BossState.CloseAttack;

          // 스턴 공격만 진행하는 상태 활성화
          OnlyStun = true;
      }

      // 타이머 시작
      farAttackTimer += Time.deltaTime;

      // 타이머가 랜덤 쿨타임을 넘었다면
      if (farAttackTimer >= randomCoolTime)
      {
          // 바위 던지기 애니메이션 재생
          anim.SetTrigger("throw");

          // 랜덤 쿨타임 다시 설정
          randomCoolTime = Random.Range(0.7f, 1.5f);

          // 랜덤 각도 다시 설정
          throwY = Random.Range(0.3f, 0.7f);

          // 타이머 초기화
          farAttackTimer = 0;
      }
  }
  ```
  > 플레이어가 일정 거리 이상 멀어지면 원거리 공격(바위 던지기)을 수행하며, 매 공격마다 대기 시간과 투척 각도를 랜덤으로 설정했습니다.<br>
  만약 플레이어가 가까이 있지만 보스보다 높은 위치에 있을 경우 근거리 공격 상태로 전환한 뒤 기절 공격만 고정으로 실행합니다.
  <br>

* 근거리 공격 패턴
  ```csharp
  void CloseAttack()
  {
      // 근거리 공격 타이머 시작
      closeAttackTimer += Time.deltaTime;

      // 타이머가 3초가 지나면
      if (closeAttackTimer >= 3)
      {
          // 랜덤으로 공격 패턴 뽑기
          attackPattern = Random.Range(0, 2);
  
          // 스턴 공격만 진행하는 상태가 활성화 되어 있다면
          if (OnlyStun == true)
          {
              // 뽑는 숫자 1로 고정
              attackPattern = 1;
          }

          // 뽑은 숫자가 0이라면 할퀴기 공격
          if (attackPattern == 0)
          {
              // 할퀴기 애니메이션 재생
              anim.SetTrigger("scratch");

              // 타이머 초기화
              closeAttackTimer = 0;
          }

          // 뽑은 숫자가 1이라면 소리치기 공격
          else if (attackPattern == 1)
          {
              // 소리치기 애니메이션 재생
              anim.SetTrigger("scream");

              // 타이머 초기화
              closeAttackTimer = 0;
          }
      }
  }
  ```
  > 플레이어와 근접한 상태에서 근거리 공격을 수행합니다. 근거리 공격은 3초마다 수행하며,<br>
  큰 피해를 입히는 할퀴기 공격과 플레이어 조작을 잠시 제한시키는 소리치기 공격 두가지 중 한가지를 무작위로 수행합니다.
  <br>
  
#### 설계 의도
> 플레이어가 멀리서 안전하게 일방적으로 보스를 공략하는 것을 막기 위해 원거리 공격의 쿨타임과 발사 각도를 매번 무작위로 재설정해 패턴을 예측하기 어렵게 설계했습니다.<br>
또한 플레이어가 보스보다 높은 지형에 올라가서 일방적으로 공격을 피하거나 멍하니 시간을 보내는 플레이를 방지하기 위해 기절 공격만 실행하여 지속적인 압박을 주도록 했습니다.<br>
그리고 근거리 공격들은 시작되면 무조건 맞게하는 것이 부당하다고 판단하여 공격 순간에 플레이어가 피격 범위에 있는지 확인하고 피해를 입히도록 하여 회피 가능성을 열어두었습니다.
</details>

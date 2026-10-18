using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 리지드바디
    Rigidbody2D rig;

    // 콜라이더
    BoxCollider2D coll2D;

    // 애니메이터
    Animator anim;

    // 플레이어 정보
    public GameObject playerUI;

    // 정보창 열림 여부
    bool openPlayerUI = false;

    // 플레이어 상하, 좌우 이동값
    float playerV;
    float playerH;

    // 플레이어 방향
    Vector2 dir;

    // 플레이어 최대 이동 속도
    public float maxSpeed;

    // 점프 중
    bool isJumping;

    // 레이
    RaycastHit2D hit;

    // 레이 시작점
    Vector2 rayStart;

    // 레이 방향
    Vector2 rayDir;

    // 레이 길이
    float rayDis = 17;

    // 레이어 마스크
    public LayerMask rayMask;

    // 전투 연출
    public GameObject battleLoad;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // 전투 연출 끄기
        battleLoad.SetActive(false);

        // 컨티뉴 한 경우
        if (GameManager.instance.isContinue)
        {
            // 위치 조정
            ChangePos(GameManager.instance.playerX, GameManager.instance.playerY);
            GameManager.instance.isContinue = false;
        }

        // 배틀에서 복귀 후 플레이어 위치 변경
        if (GameManager.instance.endBattle)
        {
            // 배경음 변경
            BGMManager.instance.EndBattleBGM();

            // 바로 다시 배틀하는 것을 막기 위해
            coll2D.enabled = false;

            switch (GameManager.instance.playerDirTag)
            {
                case "Dir_Up": ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1); break;
                case "Dir_Down": ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1); break;
                case "Dir_Left": ChangeDir("Player_Idle_Left", "Dir_Left", -1, 0); break;
                case "Dir_Right": ChangeDir("Player_Idle_Right", "Dir_Right", 1, 0); break;
            }

            ChangePos(GameManager.instance.playerX, GameManager.instance.playerY);
            GameManager.instance.endBattle = false;
        }

        // 포탈 이동 후 플레이어 위치 변경
        else
        {
            StartPosition();
        }
    }

    void Update()
    {
        #region 플레이어 이동
        // 이동
        if (CanDoing())
        {
            // 플레이어 이동
            playerV = Input.GetAxisRaw("Vertical");
            playerH = Input.GetAxisRaw("Horizontal");

            if (coll2D.enabled == false && (playerV != 0 || playerH != 0))
            {
                coll2D.enabled = true;
            }

            // 플레이어 방향
            dir = (Vector2.up * playerV) + (Vector2.right * playerH);

            // 대각선 이동 제한
            if (!(playerH != 0 && playerV != 0))
            {
                // 방향 태그 변경
                if (playerV == 1) gameObject.tag = "Dir_Up";
                else if (playerV == -1) gameObject.tag = "Dir_Down";
                else if (playerH == -1) gameObject.tag = "Dir_Left";
                else if (playerH == 1) gameObject.tag = "Dir_Right";

                // 이동 방향에 맞춰 애니메이션 재생
                ChangeAnim();
            }
        }
        #endregion

        #region 플레이어 UI
        // 'E' 버튼 누르면 플레이어 UI 활성화
        if (CanDoing() && !openPlayerUI && Input.GetKeyDown(KeyCode.E))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("OpenUI");

            // 플레이어 정지
            StopMove();

            openPlayerUI = true;
            playerUI.SetActive(true);
        }

        // 'E' 버튼 다시 누르면 플레이어 UI 비활성화
        else if (openPlayerUI && Input.GetKeyDown(KeyCode.E))
        {
            CloseBtnClick();
        }
        #endregion

        #region 레이 처리
        // 레이 시작점
        rayStart = new Vector2(transform.position.x, transform.position.y - 5.5f);

        // 레이 방향
        if (gameObject.CompareTag("Dir_Up")) rayDir = Vector2.up;
        if (gameObject.CompareTag("Dir_Down")) rayDir = Vector2.down;
        if (gameObject.CompareTag("Dir_Left")) rayDir = Vector2.left;
        if (gameObject.CompareTag("Dir_Right")) rayDir = Vector2.right;

        // 레이 그리기
        hit = Physics2D.Raycast(rayStart, rayDir, rayDis, rayMask);
        Debug.DrawRay(rayStart, rayDir * rayDis, Color.red);

        // 레이에 아무 것도 닿지 않은 경우
        if (hit.collider == null)
        {
            return;
        }

        // 레이에 닿은 대상
        switch (hit.transform.gameObject.layer)
        {
            // NPC
            case 7:
                if (CanDoing() && Input.GetKeyDown(KeyCode.Space))
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Select");

                    // 플레이어 정지
                    StopMove();

                    // NPC 방향 전환
                    if (hit.transform.gameObject.name != "NPC_Nurse" &&
                        hit.transform.gameObject.name != "NPC_Merchant")
                    {
                        hit.transform.gameObject.SendMessage("ChangeSprite");
                    }

                    // 대화 중
                    GameManager.instance.isTalking = true;
                }
                break;

            // 언덕
            case 10:
                // 아래 방향일 때 언덕과 일정 거리 이하이면서 점프 중이 아닐 때
                if (this.gameObject.CompareTag("Dir_Down") &&
                    this.transform.position.y - hit.transform.position.y < 16.0f &&
                    !isJumping &&
                    Input.GetKeyDown(KeyCode.Space))
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Jump");

                    // 점프
                    isJumping = true;
                    anim.Play("Player_Jump");
                }
                break;
        }
        #endregion

        #region 포켓몬 선택
        // 오박사와 첫대화 끝난 경우 & 포켓몬 선택 전에만 처리
        if (GameManager.instance.doctor_O_FirstTalk && !GameManager.instance.selectPartner)
        {
            // 레이에 닿은 몬스터볼
            if (hit.transform.gameObject.layer == 8)
            {
                // 스페이스 바를 누르면
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // 플레이어 정지
                    StopMove();

                    // 열리는 애니메이션 재생
                    hit.transform.gameObject.SendMessage("OpenBall");

                    // 플레이어 이동 제한
                    GameManager.instance.cantDoing = true;
                }
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        // 플레이어 이동 (대각선 잠금)
        if (!(playerH != 0 && playerV != 0))
        {
            rig.velocity = new Vector2(playerH, playerV) * maxSpeed;
        }
    }

    #region 충돌처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            // 포탈
            case 6:
                // 플레이어 정지
                StopMove();

                // 통로 포탈인 경우 포탈이름 저장
                if (collision.gameObject.name == "Potal_Gate_0" ||
                    collision.gameObject.name == "Potal_Gate_1" ||
                    collision.gameObject.name == "Potal_OutGate_0" ||
                    collision.gameObject.name == "Potal_OutGate_1")
                {
                    GameManager.instance.gatePotalName = collision.gameObject.name;
                }
                break;

            // 잔디
            case 9:
                // 바로 다시 배틀하는 것 막기 위해
                if (!GameManager.instance.endBattle)
                {
                    // 가지고 있는 포켓몬 중에
                    for (int i = 0; i < MyPokemon.pokemon.Count; i++)
                    {
                        // 하나라도 싸울 수 있는 포켓몬이 있는 경우
                        if (MyPokemon.pokemon[i].canFight)
                        {
                            // 번호 뽑기
                            int ranNum = Random.Range(1, 101);

                            // 일정 확률로 포켓몬과 마주침
                            if (ranNum <= 5) // 확률 5%
                            {
                                // 배경음 변경
                                BGMManager.instance.PlayBattleBGM();

                                // 움직임 제한 및 정지
                                GameManager.instance.cantDoing = true;
                                StopMove();

                                // 현재 위치와 방향 저장
                                GameManager.instance.playerX = transform.position.x;
                                GameManager.instance.playerY = transform.position.y;
                                GameManager.instance.playerDirTag = gameObject.tag;

                                // 연출 활성화
                                battleLoad.SetActive(true);

                                GameManager.instance.goBattle = true;
                            }

                            return;
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region 점프
    // 점프 애니메이션에서 호출
    public void StartJump()
    {
        StartCoroutine(Jump(20, 0.5f));
    }

    // 점프 이동
    IEnumerator Jump(float dis, float moveTime)
    {
        // 타이머
        float timer = 0f;

        // 현재 위치
        Vector3 nowPos = transform.position;

        // 목적지 = 현재 위치 - 이동할 거리
        Vector3 destination = nowPos - new Vector3(0, dis, 0);

        // 정해진 시간 동안 서서히 이동
        while (timer < moveTime)
        {
            transform.position = Vector3.Lerp(nowPos, destination, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // 위치 보정
        transform.position = destination;

        // 애니메이션 정지
        isJumping = false;
    }
    #endregion

    #region 애니메이션 변경
    void ChangeAnim()
    {
        //이동 중일 때
        if (0.01f < dir.magnitude)
        {
            //좌우 이동
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    anim.Play("Player_Walk_Right");
                }

                else
                {
                    anim.Play("Player_Walk_Left");
                }
            }

            //상하 이동
            else
            {
                if (dir.y < 0)
                {
                    // 점프 중이 아닐 때
                    if (!isJumping) anim.Play("Player_Walk_Down");
                }

                else
                {
                    anim.Play("Player_Walk_Up");
                }
            }
        }

        // 정지
        else
        {
            switch (this.gameObject.tag)
            {
                case "Dir_Up": anim.Play("Player_Idle_Up"); break;

                // 점프 중 아닐 때
                case "Dir_Down": if (!isJumping) anim.Play("Player_Idle_Down"); break;

                case "Dir_Left": anim.Play("Player_Idle_Left"); break;
                case "Dir_Right": anim.Play("Player_Idle_Right"); break;
            }
        }
    }
    #endregion

    #region 플레이어 최초 위치 설정 함수
    public void StartPosition()
    {
        // 현재 씬
        switch (GameManager.instance.sceneNum)
        {
            // 플레이어의 방 씬
            case 2:
                // 전에 있던 씬 = 인트로 씬
                if (GameManager.instance.pastSceneNum < 2)
                {
                    ChangeDir("Player_Idle_Right", "Dir_Right", 1, 0);
                    ChangePos(-40, -20);
                }

                // 전에 있던 씬 = 플레이어의 집 거실 씬
                else if (GameManager.instance.pastSceneNum == 3)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(70, 30);
                }
                break;


            // 플레이어의 집 거실 씬
            case 3:
                // 전에 있던 씬 = 플레이어의 방 씬
                if (GameManager.instance.pastSceneNum == 2)
                {
                    ChangeDir("Player_Idle_Left", "Dir_Left", -1, 0);
                    ChangePos(54, 34);
                }

                // 전에 있던 씬 = 마을 씬
                else if (GameManager.instance.pastSceneNum == 4)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(-40, -58);
                }
                break;

            // 마을 씬
            case 4:
                // 전에 있던 씬 = 플레이어의 집 거실 씬
                if (GameManager.instance.pastSceneNum == 3)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(56, 28);
                }

                // 전에 있던 씬 = 오박사 연구실 씬
                else if (GameManager.instance.pastSceneNum == 5)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(72, -67);
                }

                // 전에 있던 씬 = 숲 길 1
                else if (GameManager.instance.pastSceneNum == 6)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(16, 146);
                }
                break;

            // 오박사 연구실 씬
            case 5:
                // 전에 있던 씬 = 마을 씬
                if (GameManager.instance.pastSceneNum == 4)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(0, -88);
                }
                break;

            // 숲 길 1
            case 6:
                // 전에 있던 씬  = 마을 씬
                if (GameManager.instance.pastSceneNum == 4)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(16, -304);
                }
                
                // 전에 있던 씬  = 숲 길 2
                else if (GameManager.instance.pastSceneNum == 7)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(0, 306);
                }
                break;

            // 숲 길 2
            case 7:
                // 전에 있던 씬  = 숲 길 1
                if (GameManager.instance.pastSceneNum == 6)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(-24, -624);
                }

                // 전에 있던 씬  = 통로
                else if (GameManager.instance.pastSceneNum == 8)
                {
                    // 위쪽 출구
                    if (GameManager.instance.gatePotalName == "Potal_OutGate_0")
                    {
                        ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                        ChangePos(-96, 428);
                    }

                    // 아래쪽 출구
                    else if (GameManager.instance.gatePotalName == "Potal_OutGate_1")
                    {
                        ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                        ChangePos(-96, -197);
                    }
                }

                // 전에 있던 씬  = 도시
                else if (GameManager.instance.pastSceneNum == 9)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(-24, 627);
                }
                break;

            // 통로
            case 8:
                // 전에 있던 씬  = 숲 길
                if (GameManager.instance.pastSceneNum == 7)
                {
                    // 위쪽 입구
                    if (GameManager.instance.gatePotalName == "Potal_Gate_0")
                    {
                        ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                        ChangePos(0, 53);
                    }

                    // 아래쪽 입구
                    else if (GameManager.instance.gatePotalName == "Potal_Gate_1")
                    {
                        ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                        ChangePos(0, -73);
                    }
                }
                break;

            // 도시
            case 9:
                // 전에 있던 씬  = 숲 길 2
                if (GameManager.instance.pastSceneNum == 7)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(-24, -305);
                }

                // 전에 있던 씬  = 마트
                else if (GameManager.instance.pastSceneNum == 10)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(72, 14);
                }

                // 전에 있던 씬  = 포켓몬 센터
                else if (GameManager.instance.pastSceneNum == 11)
                {
                    ChangeDir("Player_Idle_Down", "Dir_Down", 0, -1);
                    ChangePos(-104, -98);
                }
                break;

            // 마트
            case 10:
                // 전에 있던 씬  = 도시
                if (GameManager.instance.pastSceneNum == 9)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(-16, -50);
                }
                break;

            // 포켓몬 센터
            case 11:
                // 전에 있던 씬  = 도시
                if (GameManager.instance.pastSceneNum == 9)
                {
                    ChangeDir("Player_Idle_Up", "Dir_Up", 0, 1);
                    ChangePos(0, -58);
                }
                break;
        }
    }

    // 초기 방향
    void ChangeDir(string animName, string tag, int rayX, int rayY)
    {
        // 바라보는 방향 쪽 애니메이션
        anim.Play(animName);

        // 바라보는 방향 태그 변경
        gameObject.tag = tag;

        // 레이 초기 위치
        rayDir = new Vector2(rayX, rayY);
    }

    // 초기 위치
    void ChangePos(float posX, float posY)
    {
        transform.position = new Vector2(posX, posY);
    }
    #endregion

    #region 플레이어 정지
    void StopMove()
    {
        playerV = 0;
        playerH = 0;

        switch (this.gameObject.tag)
        {
            case "Dir_Up": anim.Play("Player_Idle_Up"); break;

            // 점프 중 아닐 때
            case "Dir_Down": if (!isJumping) anim.Play("Player_Idle_Down"); break;

            case "Dir_Left": anim.Play("Player_Idle_Left"); break;
            case "Dir_Right": anim.Play("Player_Idle_Right"); break;
        }
    }
    #endregion

    #region 행동 가능 조건
    //(충족하면 true 반환)
    bool CanDoing()
    {
        return

        // 로딩 중 아닐 때
        !FadeManager.instance.isFading &&

        // 움직일 수 없는 상태
        !GameManager.instance.cantDoing &&

        // UI 연 상태 아닐 때
        !openPlayerUI &&

        // 점프 중이 아닐 때
        !isJumping;
    }
    #endregion

    // UI 닫기 버튼
    public void CloseBtnClick()
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Cancle");

        openPlayerUI = false;
        playerUI.SetActive(false);
    }
}

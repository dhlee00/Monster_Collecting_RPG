using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 스태틱화
    public static GameManager instance;

    #region 씬 관련
    [Header("Scene")]

    // 현재 씬 번호
    public int sceneNum;

    // 전에 있던 씬 번호
    public int pastSceneNum;
    #endregion

    #region UI 관련
    // UI 부모될 캔버스
    [HideInInspector] public GameObject canvas;

    [Header("UI")]

    // 페이드 화면
    public GameObject fade;

    // 대화창
    public GameObject textView;

    // 텍스트 매니저
    [HideInInspector] public TextManager tm;
    #endregion

    #region 플레이어 정보 관련
    [Header("Player")]

    // 플레이어 프리팹
    public GameObject playerPrefab;

    // 플레이어 스크립트
    [HideInInspector] public PlayerManager pm;

    // 플레이어 트랜스폼
    [HideInInspector] public Transform playerT;

    // 움직일 수 없는 상태
    public bool cantDoing;

    // 배틀 시작
    public bool goBattle;
    public bool endBattle;

    // 플레이어의 마지막 위치
    public float playerX;
    public float playerY;
    public string playerDirTag;

    // 통로에서 위쪽 아래쪽 입구 확인할 변수
    public string gatePotalName;
    #endregion

    #region 기타 상태 관련
    [Header("State")]

    // 대화 중
    public bool isTalking;

    // 오박사 첫 대화
    public bool doctor_O_FirstTalk;

    // 플레이어가 파트너를 고른 상태
    public bool selectPartner;

    // 파트너 선택 후 오박사와 대화
    public bool afterSelectTalk;

    // 튜토리얼
    public bool tutorialOver;

    // 컨티뉴 누른 경우
    public bool isContinue;
    #endregion

    void Awake()
    {
        // 인스턴스에 값이 없으면 자기자신 할당
        if (instance == null)
        {
            instance = this;

            // 씬 전환마다 호출
            SceneManager.sceneLoaded -= StartScene;  // 중복 생성 방지
            SceneManager.sceneLoaded += StartScene;

            // 사라지지 않음
            DontDestroyOnLoad(gameObject);
        }

        // 인스턴스에 값이 있으면 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        #region 프레임 조정
        //// 프레임 60 고정
        //Application.targetFrameRate = 60;

        //// 수직동기화 끄기
        //QualitySettings.vSyncCount = 0;
        #endregion
    }

    void Update()
    {
        // 마지막으로 클릭 된 버튼 스페이스바로 눌러도 안눌리게
        if (Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // 씬 전환마다 호출
    void StartScene(Scene scene, LoadSceneMode mode)
    {
        // 캔버스 찾기
        canvas = GameObject.Find("Canvas");

        // 인트로 끝난 후 부터 & 배틀씬 제외
        if (sceneNum >= 2 && SceneManager.GetActiveScene().name != "BattleScene")
        {
            // 페이드 생성
            Instantiate(fade, canvas.transform).name = "Fade";

            // 텍스트창 생성
            GameObject tv = Instantiate(textView, canvas.transform);
            tv.name = "TextView";
            tm = tv.GetComponentInChildren<TextManager>(true);

            // 플레이어 생성
            GameObject player = Instantiate(playerPrefab);
            player.name = "Player";
            pm = player.GetComponent<PlayerManager>();
            playerT = player.transform;
        }

        // 인트로에서만
        else if (sceneNum == 1)
        {
            // 페이드 생성
            Instantiate(fade, canvas.transform).name = "Fade";
        }
    }
}

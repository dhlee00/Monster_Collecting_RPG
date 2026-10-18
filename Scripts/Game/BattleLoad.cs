using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoad : MonoBehaviour
{
    // 캔버스 트랜스폼
    RectTransform cavRT;

    // 트랜스폼
    RectTransform rTransform;

    // 애니메이터
    Animator anim;

    // 원 이미지
    public RectTransform circleRT;

    // 원 이미지 애니메이션
    Animator circleAnim;

    // 원 이미지의 검은색 배경
    public RectTransform circleBackGround;

    private void Awake()
    {
        cavRT = transform.parent.GetComponent<RectTransform>();
        rTransform = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        circleAnim = circleRT.GetComponent<Animator>();
    }

    void Start()
    {
        // 초기화
        anim.enabled = false;
        circleAnim.enabled = false;
    }

    void Update()
    {
        // 플레이어가 야생 포켓몬과 조우했을 때 애니메이션 재생
        if (GameManager.instance.goBattle)
        {
            anim.enabled = true;
        }

        // 월드 좌표를 화면 좌표로 변환
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(GameManager.instance.playerT.position);

        // 화면 좌표를 UI 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cavRT, worldToScreen, null, out Vector2 screenToUI);

        // 원의 위치를 플레이어의 위치로 변경
        circleRT.anchoredPosition = screenToUI;

        // 원의 배경 위치 조정
        circleBackGround.anchoredPosition = rTransform.anchoredPosition - circleRT.anchoredPosition;
    }

    // 원 애니메이션 시작
    public void StartCircleAnim()
    {
        circleAnim.enabled = true;
    }
}

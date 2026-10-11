using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HtpViewManager : MonoBehaviour
{
    // 자식 오브젝트
    public GameObject htpView;

    // 창 열림 여부
    bool isOpen;

    // 소리 재생
    bool soundPlay = false;

    private void OnDisable()
    {
        soundPlay = false;
    }

    void Update()
    {
        // 튜토리얼 끝나기 전까지 실행
        if (!GameManager.instance.tutorialOver)
        {
            // 로딩 완료 되면 창 열기
            if (!FadeManager.instance.isFading)
            {
                if (!soundPlay)
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("OpenUI");
                    soundPlay = true;
                }

                isOpen = true;

                htpView.SetActive(true);

                // 플레이어 이동 제한
                GameManager.instance.cantDoing = true;
            }

            // ESC 버튼 누른 경우
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Cancle");

                // 플레이어 이동 가능
                GameManager.instance.cantDoing = false;

                // 창 닫기
                this.gameObject.SetActive(false);
            }
        }
    }
}

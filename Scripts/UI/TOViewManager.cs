using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOViewManager : MonoBehaviour
{
    // 자식 오브젝트
    public GameObject TOView;

    // 창 열림 여부
    bool isOpen;

    // 소리 재생
    bool soundPlay = false;

    void Awake()
    {
        // 튜토리얼 끝난 후엔 자동 삭제
        if (GameManager.instance.tutorialOver)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        soundPlay = false;
    }

    void Update()
    {
        // 튜토리얼 끝나면 실행
        if (GameManager.instance.tutorialOver)
        {
            if (!soundPlay)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("OpenUI");
                soundPlay = true;
            }

            isOpen = true;

            TOView.SetActive(true);

            // 플레이어 이동 제한
            GameManager.instance.cantDoing = true;

            // ESC 버튼 누른 경우
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                // 효과음 재생
                EMManager.instance.PlayEM("Cancle");

                // 플레이어 이동 가능
                GameManager.instance.cantDoing = false;

                // 다시 안나타나게 삭제
                Destroy(gameObject);
            }
        }
    }
}

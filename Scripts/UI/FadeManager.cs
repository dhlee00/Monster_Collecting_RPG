using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FadeType
{
    fadeIn,
    fadeOut
}

public class FadeManager : MonoBehaviour
{
    // 인스턴스화
    public static FadeManager instance;

    // 페이드 타입
    public FadeType fadeType;

    // 애니메이터
    Animator anim;

    // 페이드 중
    public bool isFading;

    void Awake()
    {
        // 인스턴스에 값이 없으면 자기자신 할당
        if (instance == null)
        {
            instance = this;
        }

        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 로딩 중
        isFading = true;

        // 페이드 애니메이션
        switch (fadeType)
        {
            case FadeType.fadeIn: anim.Play("Fade_In"); break;
            case FadeType.fadeOut: anim.Play("Fade_Out"); break;
        }
    }

    // 페이드인 끝
    public void EndFadeIn()
    {
        // 로딩 끝
        isFading = false;

        // 페이드 타입 변경
        fadeType = FadeType.fadeOut;

        // 자기자신 비활성화
        gameObject.SetActive(false);
    }

    // 페이드아웃 끝
    public void EndFadeOut()
    {
        // 인게임 전까진 순차적으로 씬 전환
        if (GameManager.instance.sceneNum <= 1)
        {
            GameManager.instance.sceneNum++;
        }

        // 다음 씬 로드
        SceneManager.LoadScene(GameManager.instance.sceneNum);
    }
}

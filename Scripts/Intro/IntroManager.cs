using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인트로 진행
public enum IntroState
{
    ready,
    intro_Doctor_OAppear,
    startText,
    introProgress
}

public class IntroManager : MonoBehaviour
{
    // 인트로 진행
    public IntroState state;

    // 타이머
    float timer = 0.0f;

    // 인트로 오박사
    public Image intro_Doctor_O;

    // 인트로 대화창
    public Intro_TextManager it;
    public Image nextIcon;

    // 이름 입력 창
    public GameObject askNameView;

    // 이름 입력 필드
    public InputField nameIF;

    // 인트로 몬스터볼
    public GameObject intro_MonsterBall;

    // 인트토 몬스터볼 애니메이터
    public Animator intro_MonsterBall_anim;

    // 인트토 피카츄 애니메이터
    public Animator intro_Pikachu_anim;

    // 페이드
    GameObject fade;

    void Start()
    {
        fade = GameObject.Find("Fade");
    }

    void Update()
    {
        switch (state)
        {
            case IntroState.ready:

                // 로딩이 끝나면
                if (!FadeManager.instance.isFading)
                {
                    state = IntroState.intro_Doctor_OAppear;
                }
                break;

            case IntroState.intro_Doctor_OAppear: Intro_Doctor_OAppear(); break;
            case IntroState.startText: StartText(); break;
            case IntroState.introProgress: IntroProgress(); break;
        }
    }

    // 오박사 등장
    void Intro_Doctor_OAppear()
    {
        // 타이머 시작
        timer += Time.deltaTime;

        // 오박사가 서서히 나타나도록 설정한 시간동안 투명도 내리기
        intro_Doctor_O.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), timer / 2f);

        // 투명도가 1이라면
        if (intro_Doctor_O.color.a >= 1)
        {
            // 타이머 초기화
            timer = 0.0f;

            // 텍스트 진행
            state = IntroState.startText;
        }
    }

    // 인트로 진행 시작
    void StartText()
    {
        // 인트로 대화창 활성화
        it.gameObject.SetActive(true);

        // 인트로 진행
        state = IntroState.introProgress;
    }

    // 인트로 진행 중
    void IntroProgress()
    {
        // 이름 입력 창 활성화
        if (it.printNum == 4)
        {
            it.isIFName = true;
            it.GetComponent<Image>().enabled = false;
            nextIcon.enabled = false;
            askNameView.SetActive(true);
        }

        // 인트로 몬스터볼 활성화 할 대사
        if (it.printNum == 12)
        {
            intro_MonsterBall.SetActive(true);
        }

        // 인트로 몬스터볼 애니메이션 재생
        if (it.printNum == 13)
        {
            if (intro_MonsterBall_anim != null)
            {
                intro_MonsterBall_anim.enabled = true;
            }
        }

        // 마지막 대사인 경우
        if (it.isLastLine)
        {
            // 페이드 활성화
            fade.SetActive(true);

            // 인트로 피카츄 애니메이션 재생
            intro_Pikachu_anim.enabled = true;
        }
    }

    // 이름 입력 끝난 후
    public void EndInputName()
    {
        // 입력창이 비어있거나 2자 이상 적지 않은 경우엔 버튼 눌러도 반응 X
        if (nameIF.text != null && nameIF.text.Length >= 2)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            // 게임매니저에 이름과 유저 번호 전달
            GlobalValue.playerName = nameIF.text;
            for (int i = 0; i < GlobalValue.idNum.Length; i++)
            {
                GlobalValue.idNum[i] = Random.Range(0, 10);
            }

            it.printNum++;
            it.Intro_Doctor_OLine();
            it.PrintLine(5);

            it.isIFName = false;
            it.GetComponent<Image>().enabled = true;
            nextIcon.enabled = true;
            askNameView.SetActive(false);
        }
    }

    // 이름 입력필드 초기화
    public void ClearField()
    {
        nameIF.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class Intro_TextManager : MonoBehaviour
{
    // 텍스트
    public TextMeshProUGUI text;

    // 다음 표시
    public GameObject nextIcon;

    // 출력할 문장
    public List<string> lineText = new List<string>();

    // 출력 번호
    //[HideInInspector] 
    public int printNum;

    // 출력 중
    bool isPrint;

    // 마지막 대사
    //[HideInInspector]
    public bool isLastLine;

    // 이름 입력 중
    //[HideInInspector]
    public bool isIFName;

    // 현재 진행 중인 코루틴
    Coroutine typing;

    void Start()
    {
        Intro_Doctor_OLine();

        // 첫대사 출력
        PrintLine(printNum);
    }

    void Update()
    {
        // 현재 출력 중이 아니고 마우스나 스페이스바를 눌렀을 때 + 이름 입력 중이지 않을 때
        if (!isPrint && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isIFName)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            // 다음 대사
            printNum++;

            // 마지막 대사가 아닐 경우
            if (printNum != lineText.Count)
            {
                // 대사 출력
                PrintLine(printNum);
            }

            // 마지막 대사일 경우
            else
            {
                //대화 종료
                isLastLine = true;
                printNum = 0;
                text.text = lineText[printNum];
                this.gameObject.SetActive(false);
            }
        }

        // 출력 중일 때 한번 더 스페이스바 누르면 대사 전체 출력
        else if (isPrint && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isIFName)
        {
            isPrint = false;
        }
    }

    // 출력
    public void PrintLine(int PrintNum)
    {
        if (typing == null)
        {
            typing = StartCoroutine(TypingEffect(lineText[PrintNum]));
        }
    }

    // 타이핑 효과
    IEnumerator TypingEffect(string text)
    {
        nextIcon.SetActive(false);
        isPrint = true;

        this.text.text = "";

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            if (!isPrint)
            {
                this.text.text = text;
                break;
            }

            stringBuilder.Append(text[i]);
            this.text.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.06f);
        }

        nextIcon.SetActive(true);
        typing = null;
        isPrint = false;
    }

    // 인트로 오박사 대사
    public void Intro_Doctor_OLine()
    {
        lineText.Clear();
        lineText.Add("반갑다!");
        lineText.Add("나는 오박사라고 한다.");
        lineText.Add("포켓몬 세상에 온 것을 환영한다!");
        lineText.Add("먼저 너의 이름을 알려주겠니?");
        lineText.Add("");

        lineText.Add("오오!");
        lineText.Add($"{GlobalValue.playerName}(이)라고 하는구나!");
        lineText.Add("좋은 이름이구나.\n포켓몬 마스터의 자질을 갖춘 이름이야!");

        lineText.Add($"그럼 미래의 포켓몬 마스터 {GlobalValue.playerName}!!");
        lineText.Add("이제부터 모험을 떠나ㄹ...");
        lineText.Add("응?");
        lineText.Add("포켓몬이 뭐냐고?");
        lineText.Add("여기 몬스터볼 안에 들어있는\n우리와 공존하는 생명체지");
        lineText.Add("나와라 피캬츄!");
        lineText.Add("이 아이는 피카츄라고 하는데");
        lineText.Add("이 아이 외에도 수많은 포켓몬들이 있단다.");
        lineText.Add("모험을 떠나며 새로운 포켓몬\n친구들을 만나보길 바란다!");
        lineText.Add("그럼 몸 조심해라~!");
    }
}

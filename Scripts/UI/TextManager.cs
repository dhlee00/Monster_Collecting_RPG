using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // 텍스트
    public TextMeshProUGUI text;

    // 다음 표시
    public GameObject nextIcon;

    // 현재 대사 중인 NPC
    [HideInInspector] public string talkingNpcName;

    // 현재 대사의 번호
    [HideInInspector] public int npcLineNum;

    // 출력할 문장
    public List<string> lineText = new List<string>();

    // 출력 번호
    int printNum;

    // 출력 중
    bool isPrint;

    // 코루틴 실행 중 확인
    Coroutine nowPrinting;

    void OnEnable()
    {
        // 플레이어 이동 불가 상태
        GameManager.instance.cantDoing = true;

        // 초기화
        printNum = 0;
        text.color = Color.black;

        // 첫 대사 출력
        PrintLine(0);
    }

    void OnDisable()
    {
        // 마트 씬 제외
        if (GameManager.instance.sceneNum != 10)
        {
            // 플레이어 이동 가능 상태
            GameManager.instance.cantDoing = false;
        }
    }

    void Update()
    {
        if (talkingNpcName == "Nurse")
        {
            if (CureSelect.instance.afterCurePokemon)
            {
                printNum = 4;
                PrintLine(printNum);
                CureSelect.instance.afterCurePokemon = false;
            }

            if (CureSelect.instance.cureStart || CureSelect.instance.selectView.activeSelf)
            {
                return;
            }
        }

        // 현재 출력 중이 아니고 마우스나 스페이스바를 눌렀을 때
        if (!isPrint && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            text.color = Color.black;

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
                GameManager.instance.isTalking = false;
            }

            // 대사 후 행동
            AfterPrintLine();
        }

        // 출력 중일 때 한번 더 스페이스바 누르면 대사 전체 출력
        else if (isPrint && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            isPrint = false;
        }
    }

    // 출력
    void PrintLine(int PrintNum)
    {
        if (nowPrinting == null)
        {
            nowPrinting = StartCoroutine(TypingEffect(lineText[PrintNum]));
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
        isPrint = false;
        nowPrinting = null;

        AfterTyping();
    }

    // 대사별 특정 행동
    void AfterPrintLine()
    {
        // 오박사와 첫 대화 마친 경우
        if (talkingNpcName == "Doctor_O" && npcLineNum == 0)
        {
            if (printNum == lineText.Count)
            {
                GameManager.instance.doctor_O_FirstTalk = true;
            }
        }

        // 파트너 선택 후 오박사의 대사 중
        if (talkingNpcName == "Doctor_O" && npcLineNum == 2)
        {
            // 특정 대사에서 몬스터볼 받음
            if (printNum == 3)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("GetItem");

                text.color = new Color32(0, 170, 255, 255);
                MyItem.item[0].item_HaveCount++;
            }

            if (printNum == lineText.Count)
            {
                GameManager.instance.afterSelectTalk = true;
            }
        }

        // 엄마 대사 중
        if (talkingNpcName == "Mom" && npcLineNum == 1)
        {
            // 특정 대사에서 용돈 받음
            int money = GlobalValue.playerMoney;
            if (printNum == 5 && GlobalValue.playerMoney < money + 1000)
            {
                // 효과음 재생
                EMManager.instance.PlayEM("GetItem");

                text.color = new Color32(0, 170, 255, 255);
                GlobalValue.playerMoney += 1000;
            }

            // 마지막 대사인 경우 튜토리얼 종료
            if (printNum == lineText.Count)
            {
                if (GameManager.instance.afterSelectTalk == false)
                {
                    GameManager.instance.afterSelectTalk = true;
                }

                GameManager.instance.tutorialOver = true;
            }
        }

        // 마트 직원 대사
        if (talkingNpcName == "Merchant")
        {
            // 대사 후에 상점 창 활성화
            if (printNum == lineText.Count)
            {
                GameManager.instance.canvas.transform.Find("StoreUI").gameObject.SetActive(true);
            }
        }
    }

    // 타이핑 끝나고 실행되야 하는 것들
    void AfterTyping()
    {
        // 간호사 대사
        if (talkingNpcName == "Nurse")
        {
            // 특정 대사에서 선택창 활성화
            if (printNum == 2)
            {
                CureSelect.instance.selectView.SetActive(true);
            }

            // 치료 연출 시작
            if (printNum == 3)
            {
                CureSelect.instance.cureStart = true;
            }
        }
    }
}

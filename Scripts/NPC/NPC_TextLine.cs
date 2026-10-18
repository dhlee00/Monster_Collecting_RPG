using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC
{
    Mom,
    Doctor_O,
    Merchant,
    Nurse
}

public class NPC_TextLine : MonoBehaviour
{
    // NPC
    public NPC npcState;

    // 텍스트 매니저
    TextManager tm;

    // NPC 대사 번호
    [HideInInspector] public int mom_lineNum;
    [HideInInspector] public int doctor_O_lineNum;
    [HideInInspector] public int merchant_lineNum;
    [HideInInspector] public int nurse_lineNum;

    void Start()
    {
        tm = GameManager.instance.tm;
    }

    void Update()
    {
        switch (npcState)
        {
            case NPC.Mom: Mom_Line(); break;
            case NPC.Doctor_O: Doctor_O_Line(); break;
            case NPC.Merchant: Merchant_Line(); break;
            case NPC.Nurse: Nurse_Line(); break;
        }
    }

    #region 엄마 대사
    void Mom_Line()
    {
        #region 엄마 대사 처리
        // 튜토리얼 중인 경우 대사
        if (!GameManager.instance.tutorialOver)
        {
            // 아직 포켓몬을 고르지 않았다면
            if (!GameManager.instance.selectPartner)
            {
                mom_lineNum = 0;
            }

            // 포켓몬을 골랐다면
            else
            {
                mom_lineNum = 1;
            }
        }

        // 튜토리얼이 끝난 경우 대사 고정
        else
        {
            mom_lineNum = 2;
        }
        #endregion

        // 정보 전달
        tm.talkingNpcName = "Mom";
        tm.npcLineNum = mom_lineNum;

        #region 엄마 대사 목록
        switch (mom_lineNum)
        {
            case 0:
                tm.lineText.Clear();
                tm.lineText.Add($"일어났니 {GlobalValue.playerName}?");
                tm.lineText.Add($"오늘 오박사님께서 오라고 하신 거 기억하지?");
                tm.lineText.Add($"오박사님 연구실은 남쪽에 있단다.");
                tm.lineText.Add($"잘 다녀오렴.");
                break;

            case 1:
                tm.lineText.Clear();
                tm.lineText.Add($"잘 다녀왔니?");
                tm.lineText.Add($"그 아이가 오박사님께서 주신 포켓몬이구나.");
                tm.lineText.Add($"멋지구나, 책임감을 가지고 잘 보살펴주렴.");
                tm.lineText.Add($"모험을 하려면 아무래도 돈이 필요하겠지?");
                tm.lineText.Add($"아껴서 써야 한다.");
                tm.lineText.Add($"용돈으로 1000원을 얻었다!");
                tm.lineText.Add($"모험을 하다가 힘들면 언제든\n집으로 돌아와도 된단다.");
                tm.lineText.Add($"그럼 잘 다녀오렴!");
                break;

            case 2:
                tm.lineText.Clear();
                tm.lineText.Add($"왔니? {MyPokemon.pokemon[0].pokemonName}는 잘 있고?");
                break;
        }
        #endregion
    }
    #endregion

    #region 오박사 대사
    void Doctor_O_Line()
    {
        #region 오박사 대사 처리
        // 튜토리얼 중인 경우 대사
        if (!GameManager.instance.tutorialOver)
        {
            // 첫대화 끝난 후
            if (GameManager.instance.doctor_O_FirstTalk)
            {
                // 아직 포켓몬을 고르지 않았다면
                if (!GameManager.instance.selectPartner)
                {
                    doctor_O_lineNum = 1;
                }

                // 포켓몬을 골랐다면
                else
                {
                    // 파트너 선택 후 대화
                    if (GameManager.instance.afterSelectTalk)
                    {
                        doctor_O_lineNum = 3;
                    }

                    else
                    {
                        doctor_O_lineNum = 2;
                    }
                }
            }
        }

        // 튜토리얼이 끝난 경우 대사 고정
        else
        {
            doctor_O_lineNum = 3;
        }
        #endregion

        // 정보 전달
        tm.talkingNpcName = "Doctor_O";
        tm.npcLineNum = doctor_O_lineNum;

        #region 오박사 대사 목록
        switch (doctor_O_lineNum)
        {
            case 0:
                tm.lineText.Clear();
                tm.lineText.Add($"어서와라 {GlobalValue.playerName}! 기다리고 있었다!");
                tm.lineText.Add($"오늘 부른 이유는 다름이 아니라");
                tm.lineText.Add($"내가 연구 중인 포켓몬들이 있는데");
                tm.lineText.Add($"이 아이들의 능력을 한 번 시험해보려고 한다.");
                tm.lineText.Add($"그래서 말인데 너가 포켓몬 트레이너가 되어서");
                tm.lineText.Add($"이 아이들과 모험을 떠나며 성장시켜주렴.");
                tm.lineText.Add($"너라면 분명 잘 해낼거라 믿는다.");
                tm.lineText.Add($"그럼 이제 오른쪽에 세 개의 몬스터볼이 있는데");
                tm.lineText.Add($"저 중에 하나만 고를 수 있다.");
                tm.lineText.Add($"원하는 포켓몬을 고르고\n나에게 다시 말을 걸어주렴.");
                break;

            case 1:
                tm.lineText.Clear();
                tm.lineText.Add($"고민이 많이 되는가보구나.");
                tm.lineText.Add($"가장 끌리는 포켓몬을 선택하려무나!");
                break;

            case 2:
                tm.lineText.Clear();
                tm.lineText.Add($"오! {MyPokemon.pokemon[0].pokemonName}를 선택했구나!");
                tm.lineText.Add($"너와 아주 잘 어울리는 파트너가 될게다.");
                tm.lineText.Add($"아 그리고 이걸 받아라");
                tm.lineText.Add($"몬스터볼을 받았다!");
                tm.lineText.Add($"모험을 하다가 마음에 드는 포켓몬이 있다면");
                tm.lineText.Add($"이 몬스터볼을 사용해서 네 것으로 만들거라");
                tm.lineText.Add($"그리고 포켓몬들은 전투를 통해 경험치를 얻고");
                tm.lineText.Add($"일정 레벨에 도달하면 진화를 하게 되는데");
                tm.lineText.Add($"진화를 하면 외형이 바뀌고\n능력도 한 층 더 강해진단다.");
                tm.lineText.Add($"혹시 {MyPokemon.pokemon[0].pokemonName}가 최종진화까지 하게 되면\n나에게도 소식을 전해주렴");
                tm.lineText.Add($"그럼 몸 조심하고 잘 다녀오거라!");
                tm.lineText.Add($"떠나기 전에 어머니께 인사드리는거 잊지 말고!");
                break;

            case 3:
                tm.lineText.Clear();
                tm.lineText.Add($"{MyPokemon.pokemon[0].pokemonName}가 최종진화를 한다면\n꼭 나에게 알려줘야 한다!");
                break;
        }
        #endregion
    }
    #endregion

    #region 마트 직원 대사
    void Merchant_Line()
    {
        merchant_lineNum = 0;

        // 정보 전달
        tm.talkingNpcName = "Merchant";
        tm.npcLineNum = merchant_lineNum;

        #region 마트 직원 대사 목록
        switch (merchant_lineNum)
        {
            case 0:
                tm.lineText.Clear();
                tm.lineText.Add($"어서오세요!\n무엇을 도와드릴까요?");
                break;
        }
        #endregion
    }
    #endregion

    #region 간호사 대사
    void Nurse_Line()
    {
        nurse_lineNum = 0;

        // 정보 전달
        tm.talkingNpcName = "Nurse";
        tm.npcLineNum = nurse_lineNum;

        #region 마트 직원 대사 목록
        switch (nurse_lineNum)
        {
            case 0:
                tm.lineText.Clear();
                tm.lineText.Add("안녕하세요!\n포켓몬 센터 입니다!");
                tm.lineText.Add("이곳에서는 포켓몬의 체력을 회복합니다!");
                tm.lineText.Add("당신의 포켓몬을 쉬게 하시겠습니까?");
                tm.lineText.Add("잠시만 기다려 주세요!");
                tm.lineText.Add("오래 기다리셨습니다!");
                tm.lineText.Add("포켓몬들이 모두 건강해졌습니다!");
                tm.lineText.Add("다음에 또 방문해주세요!");
                break;
        }
        #endregion
    }
    #endregion
}

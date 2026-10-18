using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkill : MonoBehaviour
{
    // 선택 아이콘
    public GameObject[] selectIcon;

    // 기술 이름 출력 변수
    public Text[] skillNameTxt;

    // 기술 정보 출력
    public TextMeshProUGUI skillCountTxt;     // 사용 가능 횟수
    public TextMeshProUGUI skillDamageTxt;    // 위력
    public TextMeshProUGUI skillAccuracyTxt;  // 명중률

    void OnEnable()
    {
        // 플레이어의 기술 이름 출력
        for (int i = 0; i < MyPokemon.pokemon[0].skill.Length; i++)
        {
            // 기술 가지고 있는 개수만큼 버튼 활성화
            skillNameTxt[i].transform.parent.gameObject.SetActive(true);

            skillNameTxt[i].text = $"{MyPokemon.pokemon[0].skill[i].skill_Name}";
        }

        // 왼쪽 아이콘만 활성화 (OnIcon 함수 재활용)
        selectIcon[0].SetActive(false);
        selectIcon[1].SetActive(false);
        selectIcon[0].SetActive(true);
        PrintSkillInfo(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Cancle");

            this.gameObject.SetActive(false);
        }

        if (!BattleManager.instance.inventory.activeSelf &&
            !BattleManager.instance.pokemonList.activeSelf)
        {
            // 좌측 상단 아이콘 켜진 상태
            if (selectIcon[0].activeSelf)
            {
                ChangeIcon(KeyCode.RightArrow, 0, 1);
                ChangeIcon(KeyCode.DownArrow, 0, 2);
                if (Input.GetKeyDown(KeyCode.Space)) Select(0);
            }

            // 우측 상단 아이콘 켜진 상태
            else if (selectIcon[1].activeSelf)
            {
                ChangeIcon(KeyCode.LeftArrow, 1, 0);
                ChangeIcon(KeyCode.DownArrow, 1, 3);
                if (Input.GetKeyDown(KeyCode.Space)) Select(1);
            }

            // 좌측 하단 아이콘 켜진 상태
            else if (selectIcon[2].activeSelf)
            {
                ChangeIcon(KeyCode.UpArrow, 2, 0);
                ChangeIcon(KeyCode.RightArrow, 2, 3);
                if (Input.GetKeyDown(KeyCode.Space)) Select(2);
            }

            // 우측 하단 아이콘 켜진 상태
            else if (selectIcon[3].activeSelf)
            {
                ChangeIcon(KeyCode.UpArrow, 3, 1);
                ChangeIcon(KeyCode.LeftArrow, 3, 2);
                if (Input.GetKeyDown(KeyCode.Space)) Select(3);
            }
        }
    }

    #region 선택 아이콘 관련
    // 아이콘 변경
    void ChangeIcon(KeyCode key, int off, int on)
    {
        // 누른 키
        if (Input.GetKeyDown(key))
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Choose");

            // 켜질 아이콘의 부모가 활성화 된 경우 = 버튼이 활성화 됨
            if (selectIcon[on].transform.parent.gameObject.activeSelf)
            {
                selectIcon[off].SetActive(false);
                selectIcon[on].SetActive(true);
                PrintSkillInfo(on);
            }
        }
    }

    // 이벤트 트리거에서 호출
    public void OnIcon(int on)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        selectIcon[0].SetActive(false);
        selectIcon[1].SetActive(false);
        selectIcon[on].SetActive(true);
        PrintSkillInfo(on);
    }
    #endregion

    // 선택
    public void Select(int num)
    {
        // 사용하려는 기술의 사용 횟수가 남아 있는 경우
        if (MyPokemon.pokemon[0].skill[num].skill_Count > 0)
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Select");

            // 공격 상태로 변경
            BattleManager.instance.ChangeState(BattleState.Attack);

            // 대사 출력
            BattleManager.instance.bt.PrintBattleInfo
                ($"{MyPokemon.pokemon[0].pokemonName}! {MyPokemon.pokemon[0].skill[num].skill_Name}!!");

            // 사용한 기술 정보 전달
            BattleManager.instance.skillNum = num;

            // 기술 사용 횟수 감소
            MyPokemon.pokemon[0].skill[num].skill_Count--;

            // 다른 기술이면 기술 이펙트 실행
            if (num != 0)
            {
                Instantiate(MyPokemon.pokemon[0].skill[num].P_Skill_Effect, BattleManager.instance.skillEffect);
            }

            // 몸통박치기면 애니메이션 재생
            else if (num == 0)
            {
                BattleManager.instance.partnerAnim.Play("PartnerAttack");
            }

            transform.parent.gameObject.SetActive(false);
        }

        else
        {
            // 효과음 재생
            EMManager.instance.PlayEM("Warning");
        }
    }

    // 스킬 고를 때 오른쪽에 스킬 정보 출력
    void PrintSkillInfo(int num)
    {
        skillCountTxt.text =
            $"{MyPokemon.pokemon[0].skill[num].skill_Count} / {MyPokemon.pokemon[0].skill[num].skill_MaxCount}";

        skillDamageTxt.text =
        $"위력 : {Mathf.RoundToInt((MyPokemon.pokemon[0].attack * MyPokemon.pokemon[0].skill[num].skill_Damage) * 0.85f)}";
        skillAccuracyTxt.text = $"명중률 : {MyPokemon.pokemon[0].skill[num].skill_Accuracy}";
    }
}

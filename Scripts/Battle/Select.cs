using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    // 선택 아이콘
    public GameObject[] selectIcon;

    // 싸우다 선택시 나올 선택 창
    public GameObject selectSkill;

    private void OnEnable()
    {
        // 항상 비활성화로 시작
        selectSkill.SetActive(false);

        // 좌측 상단 아이콘 제외 모두 비활성화로 초기화
        for (int i = 0; i < selectIcon.Length; i++)
        {
            selectIcon[i].SetActive(false);
        }

        selectIcon[0].SetActive(true);
    }

    void Update()
    {
        if (selectSkill.activeSelf == false &&
            !BattleManager.instance.inventory.activeSelf &&
            !BattleManager.instance.pokemonList.activeSelf)
        {
            // 좌측 상단 아이콘 켜진 상태
            if (selectIcon[0].activeSelf)
            {
                ChangeIcon(KeyCode.RightArrow, 0, 1);
                ChangeIcon(KeyCode.DownArrow, 0, 2);
                if (Input.GetKeyDown(KeyCode.Space)) SelectButton(0);
            }

            // 우측 상단 아이콘 켜진 상태
            else if (selectIcon[1].activeSelf)
            {
                ChangeIcon(KeyCode.LeftArrow, 1, 0);
                ChangeIcon(KeyCode.DownArrow, 1, 3);
                if (Input.GetKeyDown(KeyCode.Space)) SelectButton(1);
            }

            // 좌측 하단 아이콘 켜진 상태
            else if (selectIcon[2].activeSelf)
            {
                ChangeIcon(KeyCode.UpArrow, 2, 0);
                ChangeIcon(KeyCode.RightArrow, 2, 3);
                if (Input.GetKeyDown(KeyCode.Space)) SelectButton(2);
            }

            // 우측 하단 아이콘 켜진 상태
            else if (selectIcon[3].activeSelf)
            {
                ChangeIcon(KeyCode.UpArrow, 3, 1);
                ChangeIcon(KeyCode.LeftArrow, 3, 2);
                if (Input.GetKeyDown(KeyCode.Space)) SelectButton(3);
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

            // 꺼질 아이콘
            selectIcon[off].SetActive(false);

            // 켜질 아이콘
            selectIcon[on].SetActive(true);
        }
    }

    // 이벤트 트리거에서 호출
    public void OnIcon(int on)
    {
        // 효과음 재생
        EMManager.instance.PlayEM("Choose");

        for (int i = 0; i < selectIcon.Length; i++)
        {
            selectIcon[i].SetActive(false);
        }

        selectIcon[on].SetActive(true);
    }
    #endregion

    // 선택
    public void SelectButton(int num)
    {
        switch (num)
        {
            case 0:
                // 기술 사용 가능한 경우에만
                if (!BattleManager.instance.ConsumeSkillCount())
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Select");

                    selectSkill.SetActive(true);
                }

                else
                {
                    // 효과음 재생
                    EMManager.instance.PlayEM("Warning");

                    BattleManager.instance.bt.PrintBattleInfo($"{MyPokemon.pokemon[0].pokemonName}(은)는 더 이상 싸울 수 없다...");
                }
                break;

            case 1:
                // 효과음 재생
                EMManager.instance.PlayEM("Select");

                BattleManager.instance.inventory.SetActive(true);
                break;

            case 2:
                // 효과음 재생
                EMManager.instance.PlayEM("Select");

                BattleManager.instance.pokemonList.SetActive(true);
                break;

            case 3:
                // 효과음 재생
                EMManager.instance.PlayEM("Run");

                BattleManager.instance.ChangeState(BattleState.run);
                BattleManager.instance.bt.PrintBattleInfo("무사히 도망쳤다!");
                this.gameObject.SetActive(false);
                break;
        }
    }
}
